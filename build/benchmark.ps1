#! /usr/bin/env pwsh
#Requires -Version 7.0
#Requires -PSEdition Core

param(
    [switch] $PublishResults,
    [Parameter(Mandatory = $false)][string] $Framework = "net10.0"
)

$ErrorActionPreference = "Stop"

$additionalArgs = @()

if ($PublishResults) {
    $additionalArgs += "--exporters", "json"
}

$benchmarkProject = Join-Path $PSScriptRoot ".." "benchmarks" "HwoodiwissApplication.Benchmarks" "HwoodiwissApplication.Benchmarks.csproj"

dotnet run --project $benchmarkProject --configuration Release --framework $Framework -- $additionalArgs --% --filter *

exit $LASTEXITCODE
