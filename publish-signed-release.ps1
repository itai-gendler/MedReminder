param(
    [Parameter(Position = 0)]
    [ValidatePattern('^[0-9]+\.[0-9]+\.[0-9]+$')]
    [string]$Version,

    # SHA-1 thumbprint of the Certum certificate exposed by SimplySign
    # Desktop. Falls back to the CERTUM_CERT_THUMBPRINT environment variable.
    [string]$CertificateThumbprint = $env:CERTUM_CERT_THUMBPRINT,

    # RFC 3161 timestamp server passed to release.ps1 -LocalBuild.
    [string]$TimestampUrl = "http://time.certum.pl",

    # Resume on an existing tag and release: skip the Git release and the
    # wait for the CI workflow.
    [switch]$SkipGitRelease,

    # Maximum wait for the CI workflow run to appear after the tag push.
    [int]$WorkflowStartTimeoutSeconds = 300,

    # Do not publish the Microsoft Store MSI on the GitHub Pages branch.
    [switch]$SkipStorePages,

    # Base URL GitHub Pages serves the store branch from
    # (docs/PACKAGING.md §26).
    [string]$StorePagesUrl = "https://vger70.github.io/MedReminder",

    # Do not upload the Microsoft Store MSI to Cloudflare R2.
    [switch]$SkipR2,

    # Only upload the Store MSI of an existing local build
    # (dist\<version>\) to Cloudflare R2, to test the credentials and the
    # public URL. Nothing else runs: no Git, no CI wait, no signing.
    [switch]$TestR2,

    [Alias("h")]
    [switch]$Help
)

function Show-Usage {
    Write-Host ""
    Write-Host "Signed Release Script" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "USAGE:"
    Write-Host "  .\publish-signed-release.ps1 <version> [-CertificateThumbprint <sha1>] [-TimestampUrl <url>] [-SkipGitRelease] [-SkipR2] [-SkipStorePages]"
    Write-Host "  .\publish-signed-release.ps1 <version> -TestR2"
    Write-Host "  .\publish-signed-release.ps1 -Help"
    Write-Host ""
    Write-Host "EXAMPLES:"
    Write-Host "  .\publish-signed-release.ps1 2.12.1"
    Write-Host "  .\publish-signed-release.ps1 2.12.1 -CertificateThumbprint 0123456789ABCDEF0123456789ABCDEF01234567"
    Write-Host "  .\publish-signed-release.ps1 2.12.1 -SkipGitRelease"
    Write-Host "  .\publish-signed-release.ps1 2.12.1 -TestR2"
    Write-Host ""
    Write-Host "DESCRIPTION:"
    Write-Host "  Publishes a release and replaces its CI-built assets with Certum-signed ones:"
    Write-Host "    0. check prerequisites (gh login, signing certificate, Google Drive and"
    Write-Host "       Cloudflare R2 variables, clean tree, tag not used)"
    Write-Host "    1. git checkout main, git pull, .\release.ps1 <version>"
    Write-Host "       (commit, push, tag v<version>; the tag triggers the CI release)"
    Write-Host "    2. wait for the CI workflow, then git checkout v<version>"
    Write-Host "    3. .\release.ps1 <version> -LocalBuild (signed and timestamped),"
    Write-Host "       then signtool verify on both MSIs"
    Write-Host "    4. gh release upload --clobber: signed ZIPs, MSIs and SHA256SUMS.txt"
    Write-Host "    5. upload the Store MSI to Cloudflare R2 (aws s3api), served at"
    Write-Host "       https://<MEDREMINDER_CLOUDFLARE_DOWNLOAD>/<version>/MedReminder-win-x64-net10.msi"
    Write-Host "    6. publish the Store MSI on the 'store' branch served by GitHub Pages,"
    Write-Host "       at <StorePagesUrl>/<version>/MedReminder-win-x64-net10.msi"
    Write-Host "    7. git checkout main (always, also on failure)"
    Write-Host ""
    Write-Host "  -SkipGitRelease resumes from step 2 on an existing tag and release,"
    Write-Host "  e.g. after a signing failure. -SkipR2 skips step 5 and -SkipStorePages"
    Write-Host "  skips step 6, e.g. when the version is already published there."
    Write-Host "  -TestR2 runs step 5 alone on an existing dist\<version>\ build."
    Write-Host ""
    Write-Host "REQUIREMENTS:"
    Write-Host "  - GitHub CLI (gh) logged in: gh auth login"
    Write-Host "  - SimplySign Desktop running and connected"
    Write-Host "  - Windows SDK (signtool.exe), .NET 10 SDK"
    Write-Host "  - MEDREMINDER_GOOGLE_CLIENT_ID, MEDREMINDER_GOOGLE_CLIENT_SECRET"
    Write-Host "  - unless -SkipR2: AWS CLI (aws) and MEDREMINDER_CLOUDFLARE_TOKEN,"
    Write-Host "    _SECRET, _ENDPOINT, _BUCKET, _DOWNLOAD"
    Write-Host ""
}

if ($Help -or [string]::IsNullOrWhiteSpace($Version)) {
    Show-Usage
    exit 0
}

# No global $ErrorActionPreference = 'Stop': under Windows PowerShell 5.1
# it turns any stderr output of git/gh into a terminating error. Exit codes
# are checked explicitly instead.

$TagName      = "v$Version"
$WorkflowFile = "dotnet-desktop.yml"
$ReleaseScript = Join-Path $PSScriptRoot "release.ps1"
$DistDir      = Join-Path $PSScriptRoot "dist\$Version"
$StoreMsiName = "MedReminder-win-x64-net10.msi"
$StoreBranch  = "store"
# Versions kept on the store branch: the new one and the previous one,
# whose URL may still be in a submission under certification.
$StoreKeep    = 4
$Assets       = @(
    "MedReminder-win-x64-net10.zip",
    "MedReminder-win-x64.zip",
    "MedReminder-win-x64.msi",
    "MedReminder-win-x64-net10.msi",
    "SHA256SUMS.txt"
)
# release.ps1 -LocalBuild compiles without Google Drive when these are
# unset, and step 4 would replace the CI assets, which have it.
$GoogleEnvVars = @(
    "MEDREMINDER_GOOGLE_CLIENT_ID",
    "MEDREMINDER_GOOGLE_CLIENT_SECRET"
)
$R2EnvVars    = @(
    "MEDREMINDER_CLOUDFLARE_TOKEN",
    "MEDREMINDER_CLOUDFLARE_SECRET",
    "MEDREMINDER_CLOUDFLARE_ENDPOINT",
    "MEDREMINDER_CLOUDFLARE_BUCKET",
    "MEDREMINDER_CLOUDFLARE_DOWNLOAD"
)

function Write-Step {
    param([string]$Text)
    Write-Host ""
    Write-Host "==> $Text" -ForegroundColor Cyan
}

function Invoke-Native {
    param(
        [scriptblock]$Command,
        [string]$Description
    )

    & $Command
    if ($LASTEXITCODE -ne 0) {
        throw "$Description failed (exit code $LASTEXITCODE)."
    }
}

function Find-SignTool {
    $cmd = Get-Command signtool.exe -ErrorAction SilentlyContinue
    if ($cmd) { return $cmd.Source }

    $found = Get-ChildItem "${env:ProgramFiles(x86)}\Windows Kits\10\bin\*\x64\signtool.exe" -ErrorAction SilentlyContinue |
        Sort-Object FullName -Descending |
        Select-Object -First 1
    if ($found) { return $found.FullName }

    throw "signtool.exe not found. Install the Windows SDK."
}

function Assert-EnvVars {
    param(
        [string[]]$Names,
        [string]$Purpose
    )

    $missing = @($Names | Where-Object { [string]::IsNullOrWhiteSpace([Environment]::GetEnvironmentVariable($_)) })
    if ($missing.Count -gt 0) {
        throw "Environment variable(s) not set: $($missing -join ', ') ($Purpose)."
    }
}

function Assert-R2Prerequisites {
    Assert-EnvVars $R2EnvVars "needed for the Cloudflare R2 upload; -SkipR2 skips it"
    # The dashboard shows the S3 API URL with the bucket appended; the
    # bucket goes in MEDREMINDER_CLOUDFLARE_BUCKET instead.
    $endpoint = $null
    if (-not [Uri]::TryCreate($env:MEDREMINDER_CLOUDFLARE_ENDPOINT, [UriKind]::Absolute, [ref]$endpoint) -or
        $endpoint.Scheme -ne "https" -or $endpoint.AbsolutePath -ne "/") {
        throw "MEDREMINDER_CLOUDFLARE_ENDPOINT must be https://<account id>.r2.cloudflarestorage.com, without the bucket name."
    }
    if (-not (Get-Command aws -ErrorAction SilentlyContinue)) {
        throw "AWS CLI (aws) not found; it uploads the Store MSI to Cloudflare R2. Install it, or pass -SkipR2."
    }
}

function Assert-Prerequisites {
    Write-Step "Checking prerequisites"

    if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
        throw "GitHub CLI (gh) not found. Install it and run: gh auth login"
    }
    gh auth status *> $null
    if ($LASTEXITCODE -ne 0) { throw "GitHub CLI is not logged in. Run: gh auth login" }

    # Fail before tagging, not after: signing is the step most likely to fail.
    if ([string]::IsNullOrWhiteSpace($script:CertificateThumbprint)) {
        throw "No certificate thumbprint. Pass -CertificateThumbprint or set CERTUM_CERT_THUMBPRINT."
    }
    $script:CertificateThumbprint = ($script:CertificateThumbprint -replace '\s', '').ToUpperInvariant()
    if (-not (Test-Path "Cert:\CurrentUser\My\$($script:CertificateThumbprint)")) {
        throw "Certificate $($script:CertificateThumbprint) not found in Cert:\CurrentUser\My. Start SimplySign Desktop and connect."
    }

    $script:SignTool = Find-SignTool

    Assert-EnvVars $GoogleEnvVars "the signed build would have no Google Drive"
    if (-not $SkipR2) { Assert-R2Prerequisites }

    $dirty = git status --porcelain
    if ($LASTEXITCODE -ne 0) { throw "git status failed." }
    if ($dirty -and -not $SkipGitRelease) {
        # release.ps1 commits everything with 'git add .'; refuse to sweep
        # unrelated local changes into the release commit.
        throw "Working tree is not clean. Commit or stash your changes first."
    }
    if ($dirty -and $SkipGitRelease) {
        throw "Working tree is not clean; checking out $TagName would fail or carry local changes."
    }

    Invoke-Native { git fetch origin --tags --quiet } "git fetch"
    git rev-parse --verify --quiet "refs/tags/$TagName" *> $null
    $tagExists = ($LASTEXITCODE -eq 0)

    if (-not $SkipGitRelease -and $tagExists) {
        throw "Tag $TagName already exists. Use a new version, or -SkipGitRelease to sign the existing release."
    }
    if ($SkipGitRelease -and -not $tagExists) {
        throw "Tag $TagName does not exist; -SkipGitRelease needs an existing tag."
    }

    Write-Host "OK: gh logged in, certificate found, signtool at $($script:SignTool), environment variables set" -ForegroundColor Green
}

function Wait-ReleaseWorkflow {
    Write-Step "Waiting for the CI workflow on $TagName"

    $deadline = (Get-Date).AddSeconds($WorkflowStartTimeoutSeconds)
    $runId = $null

    $tagSha = git rev-list -n 1 $TagName
    if ($LASTEXITCODE -ne 0) { throw "Cannot resolve $TagName." }

    while (-not $runId) {
        $json = gh run list --workflow $WorkflowFile --commit $tagSha --event push --limit 1 --json databaseId
        if ($LASTEXITCODE -ne 0) { throw "gh run list failed." }

        $runs = $json | ConvertFrom-Json
        if ($runs.Count -gt 0) {
            $runId = $runs[0].databaseId
            break
        }
        if ((Get-Date) -gt $deadline) {
            throw "No $WorkflowFile run found for $TagName after $WorkflowStartTimeoutSeconds s."
        }
        Start-Sleep -Seconds 10
    }

    Write-Host "Workflow run: $runId"
    Invoke-Native { gh run watch $runId --exit-status --interval 15 } "CI workflow run $runId"

    Invoke-Native { gh release view $TagName --json tagName *> $null } "gh release view $TagName"
    Write-Host "OK: release $TagName created by CI" -ForegroundColor Green
}

function Wait-UrlStatus {
    # Polls $Url for up to 5 minutes until it answers 200 or a redirect,
    # and returns the last status ($null when unreachable). The Store
    # rejects a URL that redirects, so a 3xx is reported, not followed.
    param([string]$Url)

    $deadline = (Get-Date).AddMinutes(5)
    $status = $null
    while ((Get-Date) -lt $deadline) {
        try {
            $response = Invoke-WebRequest -Uri $Url -Method Head -MaximumRedirection 0 -UseBasicParsing -ErrorAction Stop
            $status = [int]$response.StatusCode
        }
        catch {
            $status = $null
            if ($_.Exception.Response) { $status = [int]$_.Exception.Response.StatusCode }
        }
        if ($status -eq 200 -or ($status -ge 300 -and $status -lt 400)) { break }
        Start-Sleep -Seconds 15
    }
    return $status
}

function Publish-R2 {
    # Cloudflare R2 serves the Store MSI from the bucket's public custom
    # domain (MEDREMINDER_CLOUDFLARE_DOWNLOAD), which answers without
    # redirection. The upload uses the S3-compatible API through the AWS
    # CLI; the R2 credentials reach it through AWS_* variables set for
    # this process only and restored afterwards. Old versions stay in the
    # bucket; they are removed by hand.
    $msi = Join-Path $DistDir $StoreMsiName
    if (-not (Test-Path $msi)) { throw "Missing asset: $msi" }

    $bucket = $env:MEDREMINDER_CLOUDFLARE_BUCKET
    $key    = "$Version/$StoreMsiName"
    $common = @(
        "--endpoint-url", $env:MEDREMINDER_CLOUDFLARE_ENDPOINT,
        "--region", "auto",
        "--output", "json",
        "--no-cli-pager"
    )

    # Recent AWS CLI versions add checksum headers by default; send them
    # only where the S3 API requires them.
    $awsEnv = @{
        AWS_ACCESS_KEY_ID                = $env:MEDREMINDER_CLOUDFLARE_TOKEN
        AWS_SECRET_ACCESS_KEY            = $env:MEDREMINDER_CLOUDFLARE_SECRET
        AWS_SESSION_TOKEN                = $null
        AWS_REQUEST_CHECKSUM_CALCULATION = "WHEN_REQUIRED"
        AWS_RESPONSE_CHECKSUM_VALIDATION = "WHEN_REQUIRED"
    }
    $saved = @{}
    foreach ($name in $awsEnv.Keys) {
        $saved[$name] = [Environment]::GetEnvironmentVariable($name)
        [Environment]::SetEnvironmentVariable($name, $awsEnv[$name])
    }

    try {
        # list-objects-v2 rather than head-object: a HEAD error has no body,
        # so the CLI could only report "Bad Request" without R2's reason.
        $output = aws s3api list-objects-v2 --bucket $bucket --prefix $key --max-keys 1 @common 2>&1
        $listExit = $LASTEXITCODE
        $stdout = ($output | Where-Object { $_ -isnot [System.Management.Automation.ErrorRecord] }) -join "`n"
        $stderr = ($output | Where-Object { $_ -is [System.Management.Automation.ErrorRecord] }) -join "`n"
        if ($listExit -ne 0) {
            throw "aws s3api list-objects-v2 on bucket $bucket failed (exit code $listExit): $stderr"
        }

        $existing = $null
        if (-not [string]::IsNullOrWhiteSpace($stdout)) {
            $existing = @(($stdout | ConvertFrom-Json).Contents) | Where-Object { $_.Key -eq $key }
        }

        if ($existing) {
            # A single-part upload's ETag is the MD5 of the file.
            $etag = $existing.ETag.Trim('"')
            $md5  = (Get-FileHash $msi -Algorithm MD5).Hash
            if ($etag -ne $md5) {
                throw "$key is already in R2 bucket $bucket with a different MSI. The Store requires the file behind a submitted URL not to change: release a new version, or pass -SkipR2."
            }
            Write-Host "$Version is already in R2 bucket $bucket with the same MSI." -ForegroundColor Yellow
        }
        else {
            Write-Host "Uploading $key to R2 bucket $bucket"
            Invoke-Native {
                aws s3api put-object --bucket $bucket --key $key --body $msi --content-type "application/x-msi" @common | Out-Null
            } "aws s3api put-object $key"
        }
    }
    finally {
        foreach ($name in $saved.Keys) {
            [Environment]::SetEnvironmentVariable($name, $saved[$name])
        }
    }

    $download = $env:MEDREMINDER_CLOUDFLARE_DOWNLOAD.Trim().TrimEnd('/')
    if ($download -notmatch '^https?://') { $download = "https://$download" }
    $url = "$download/$Version/$StoreMsiName"
    $status = Wait-UrlStatus $url

    if ($status -eq 200) {
        Write-Host "OK: $url answers 200. Use it as the package URL in Partner Center." -ForegroundColor Green
    }
    elseif ($status -ge 300 -and $status -lt 400) {
        Write-Host "WARNING: $url redirects ($status); the Store will reject it. Check MEDREMINDER_CLOUDFLARE_DOWNLOAD." -ForegroundColor Yellow
    }
    else {
        Write-Host "WARNING: $url not reachable (last status: $status). Check the bucket's public custom domain (MEDREMINDER_CLOUDFLARE_DOWNLOAD)." -ForegroundColor Yellow
    }
}

function Publish-StorePages {
    # The Microsoft Store downloads the MSI from a URL that must answer
    # without redirection and whose file never changes (docs/PACKAGING.md
    # §26). GitHub release assets redirect to expiring URLs, so the MSI is
    # served by GitHub Pages from the 'store' branch, which holds a single
    # parentless commit replaced at each publish: the repository does not
    # grow by one MSI per release. The work happens in a temporary
    # repository; the checkout is not touched.
    $msi = Join-Path $DistDir $StoreMsiName
    if (-not (Test-Path $msi)) { throw "Missing asset: $msi" }

    $remoteUrl = git remote get-url origin
    if ($LASTEXITCODE -ne 0) { throw "git remote get-url origin failed." }

    $work = Join-Path ([IO.Path]::GetTempPath()) "medreminder-store-$Version"
    if (Test-Path $work) { Remove-Item $work -Recurse -Force }
    New-Item -ItemType Directory -Path $work | Out-Null

    try {
        Invoke-Native { git -C $work init --quiet } "git init"

        # ls-remote exits 2 when the branch does not exist yet.
        $oldSha = ""
        $heads = git ls-remote --exit-code --heads $remoteUrl $StoreBranch
        if ($LASTEXITCODE -eq 0) {
            $oldSha = ($heads -split '\s+')[0]
            Invoke-Native { git -C $work fetch --quiet --depth 1 $remoteUrl $StoreBranch } "git fetch $StoreBranch"
            Invoke-Native { git -C $work checkout --quiet --detach FETCH_HEAD } "git checkout $StoreBranch"
        }
        elseif ($LASTEXITCODE -ne 2) {
            throw "git ls-remote failed (exit code $LASTEXITCODE)."
        }

        $target = Join-Path $work "$Version\$StoreMsiName"
        if (Test-Path $target) {
            if ((Get-FileHash $target).Hash -eq (Get-FileHash $msi).Hash) {
                Write-Host "$Version is already on $StoreBranch with the same MSI." -ForegroundColor Yellow
                return
            }
            throw "$Version is already on $StoreBranch with a different MSI. The Store requires the file behind a submitted URL not to change: release a new version, or pass -SkipStorePages."
        }

        $versions = @(Get-ChildItem -Path $work -Directory |
            Where-Object { $_.Name -match '^\d+\.\d+\.\d+$' } |
            Sort-Object { [version]$_.Name })
        $versions | Select-Object -SkipLast ($StoreKeep - 1) | ForEach-Object {
            Write-Host "  removing $($_.Name)"
            Remove-Item $_.FullName -Recurse -Force
        }

        New-Item -ItemType Directory -Path (Split-Path $target) | Out-Null
        Copy-Item $msi $target
        # No Jekyll build: the branch holds only binaries.
        Set-Content -Path (Join-Path $work ".nojekyll") -Value "" -Encoding ascii

        Invoke-Native { git -C $work checkout --quiet --orphan publish } "git checkout --orphan"
        Invoke-Native { git -C $work add -A } "git add"
        Invoke-Native { git -C $work commit --quiet -m "Publish the Microsoft Store MSI of v$Version" } "git commit"
        # The lease refuses the push when the branch moved since the fetch
        # (with an empty value: when it was created meanwhile).
        Invoke-Native { git -C $work push --quiet "--force-with-lease=refs/heads/${StoreBranch}:$oldSha" $remoteUrl "HEAD:refs/heads/$StoreBranch" } "git push $StoreBranch"
    }
    finally {
        Remove-Item $work -Recurse -Force -ErrorAction SilentlyContinue
    }

    # GitHub Pages deploys the branch in a minute or two.
    $url = "$StorePagesUrl/$Version/$StoreMsiName"
    $status = Wait-UrlStatus $url

    if ($status -eq 200) {
        Write-Host "OK: $url answers 200. Use it as the package URL in Partner Center." -ForegroundColor Green
    }
    elseif ($status -ge 300 -and $status -lt 400) {
        Write-Host "WARNING: $url redirects ($status); the Store will reject it. Check -StorePagesUrl (custom domain?)." -ForegroundColor Yellow
    }
    else {
        Write-Host "WARNING: $url not reachable yet (last status: $status). Check Settings > Pages (source: branch $StoreBranch, root)." -ForegroundColor Yellow
    }
}

if ($TestR2) {
    try {
        if ($SkipR2) { throw "-TestR2 and -SkipR2 cannot be combined." }
        Write-Step "Cloudflare R2 upload test of $Version"
        $msi = Join-Path $DistDir $StoreMsiName
        if (-not (Test-Path $msi)) {
            throw "$msi not found. Build $Version locally first (.\release.ps1 $Version -LocalBuild)."
        }
        Assert-R2Prerequisites
        Publish-R2
        exit 0
    }
    catch {
        Write-Host ""
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        exit 1
    }
}

$tagPushed  = $false
$leftBranch = $false
$exitCode   = 0

try {
    Assert-Prerequisites

    if (-not $SkipGitRelease) {
        Write-Step "[1/6] Git release $TagName"
        $leftBranch = $true
        Invoke-Native { git checkout main } "git checkout main"
        Invoke-Native { git pull } "git pull"
        & $ReleaseScript $Version
        if ($LASTEXITCODE -ne 0) { throw "release.ps1 $Version failed (exit code $LASTEXITCODE)." }
        $tagPushed = $true

        Write-Step "[2/6] Waiting for CI"
        Wait-ReleaseWorkflow
    }
    else {
        Write-Step "[1-2/6] Git release and CI wait skipped (-SkipGitRelease)"
        Invoke-Native { gh release view $TagName --json tagName *> $null } "gh release view $TagName"
    }

    Write-Step "Checking out $TagName"
    Invoke-Native { git fetch origin --tags --quiet } "git fetch --tags"
    $leftBranch = $true
    Invoke-Native { git checkout --quiet $TagName } "git checkout $TagName"

    Write-Step "[3/6] Signed local build"
    & $ReleaseScript $Version -LocalBuild `
        -CertificateThumbprint $script:CertificateThumbprint `
        -TimestampUrl $TimestampUrl
    if ($LASTEXITCODE -ne 0) { throw "release.ps1 $Version -LocalBuild failed (exit code $LASTEXITCODE)." }

    foreach ($name in "MedReminder-win-x64.msi", "MedReminder-win-x64-net10.msi") {
        $msi = Join-Path $DistDir $name
        Invoke-Native { & $script:SignTool verify /pa /v $msi } "signtool verify $msi"
    }

    Write-Step "[4/6] Uploading signed assets to $TagName"
    $files = $Assets | ForEach-Object {
        $path = Join-Path $DistDir $_
        if (-not (Test-Path $path)) { throw "Missing asset: $path" }
        $path
    }
    Invoke-Native { gh release upload $TagName @files --clobber } "gh release upload"

    Write-Host ""
    Write-Host "Signed release $TagName published." -ForegroundColor Green
    gh release view $TagName --json url --jq .url

    if (-not $SkipR2) {
        Write-Step "[5/6] Uploading the Store MSI to Cloudflare R2"
        Publish-R2
    }

    if (-not $SkipStorePages) {
        Write-Step "[6/6] Publishing the Store MSI on $StoreBranch (GitHub Pages)"
        Publish-StorePages
    }
}
catch {
    Write-Host ""
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    if ($tagPushed -or $SkipGitRelease) {
        Write-Host "Fix the cause, then resume with: .\publish-signed-release.ps1 $Version -SkipGitRelease" -ForegroundColor Yellow
    }
    $exitCode = 1
}
finally {
    if ($leftBranch) {
        Write-Step "Returning to main"
        git checkout --quiet main
        if ($LASTEXITCODE -ne 0) {
            Write-Host "WARNING: git checkout main failed; check the working tree." -ForegroundColor Yellow
        }
    }
}

exit $exitCode
