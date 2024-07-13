#!/usr/bin/env pwsh
#Requires -PSEdition Core
param([string]$os)
./publish.ps1 $os

$dist_folder="./dist/$os"

Copy-Item -Path "LICENSE.txt" -Destination $dist_folder
Copy-Item -Path "README.md" -Destination $dist_folder
Compress-Archive -Path $dist_folder -DestinationPath "$dist_folder.zip"
