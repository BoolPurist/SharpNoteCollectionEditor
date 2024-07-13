#!/usr/bin/env pwsh
#Requires -PSEdition Core
param([string]$os)

$dist_folder="./dist/$os"
Remove-Item -Path $dist_folder -Recurse
$project_path="./NoteCollectionEditor/NoteCollectionEditor.csproj"
dotnet publish --runtime $os --self-contained --configuration Release --output $dist_folder $project_path
