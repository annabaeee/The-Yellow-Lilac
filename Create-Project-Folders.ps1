# PowerShell Script to Generate a Unity HDRP Project Folder Structure
#
# INSTRUCTIONS:
# 1. Save this file as "Create-Project-Folders.ps1" in your Unity project's root folder.
# 2. Open a PowerShell terminal in that folder.
# 3. Run the script by typing: .\Create-Project-Folders.ps1

Write-Host "Starting Unity HDRP folder generation..." -ForegroundColor Green

$basePath = ".\Assets"
$userName = $env:USERNAME # Automatically gets your Windows username

# Define all the directories to be created
$folders = @(
    "_Project",
    "_Project/Animations",
    "_Project/Animations/Clips",
    "_Project/Animations/Controllers",
    "_Project/Audio",
    "_Project/Audio/Music",
    "_Project/Audio/SFX",
    "_Project/Core",
    "_Project/Core/Materials",
    "_Project/Core/Prefabs",
    "_Project/Core/Scripts",
    "_Project/Fonts",
    "_Project/Materials",
    "_Project/Materials/Decals",
    "_Project/Materials/Surfaces",
    "_Project/Models",
    "_Project/Models/Characters",
    "_Project/Models/Environment",
    "_Project/Prefabs",
    "_Project/Prefabs/Characters",
    "_Project/Prefabs/Environment",
    "_Project/Rendering",
    "_Project/Rendering/CustomPasses",
    "_Project/Rendering/RenderTextures",
    "_Project/Scenes",
    "_Project/Scenes/Levels",
    "_Project/Scenes/System",
    "_Project/Scripts",
    "_Project/Scripts/Editor",
    "_Project/Scripts/Runtime",
    "_Project/Settings",
    "_Project/Settings/PostProcessing",
    "_Project/Settings/RenderProfiles",
    "_Project/Shaders",
    "_Project/Shaders/Graphs",
    "_Project/Shaders/Includes",
    "_Project/Textures",
    "_Project/Textures/Characters",
    "_Project/Textures/Environment",
    "_Project/Textures/UI",
    "_Sandbox",
    "_Sandbox/$userName",
    "_ThirdParty"
)

# Loop through the list and create each directory if it doesn't exist
foreach ($folder in $folders) {
    $fullPath = Join-Path -Path $basePath -ChildPath $folder
    if (-not (Test-Path $fullPath)) {
        try {
            New-Item -ItemType Directory -Path $fullPath -ErrorAction Stop | Out-Null
            Write-Host "  Created: $fullPath"
        }
        catch {
            Write-Host "  Error creating folder: $fullPath" -ForegroundColor Red
        }
    } else {
        Write-Host "  Exists:  $fullPath" -ForegroundColor Yellow
    }
}

Write-Host "Folder structure generation complete! ✨" -ForegroundColor Green