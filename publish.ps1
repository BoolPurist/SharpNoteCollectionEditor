#!/usr/bin/env pwsh
#Requires -PSEdition Core
param([string]$os)
$dist_folder="./dist/$os"

Remove-Item -Path $dist_folder -Recurse
dotnet publish --runtime $os --output $dist_folder ./NoteCollectionEditor/NoteCollectionEditor.csproj
