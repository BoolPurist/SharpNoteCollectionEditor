#!/usr/bin/env pwsh
#Requires -PSEdition Core
param([string]$os)

./publish.ps1 $os
Invoke-Expression "./dist/$os/NoteCollectionEditor"
