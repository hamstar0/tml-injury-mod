rmdir /S /Q "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Injury\"
xcopy . "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Injury\" /S /EXCLUDE:.gitignore
del "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Injury\*.csproj"
del "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Injury\*.user"
del "C:\Users\Spinach\Documents\My Games\Terraria\ModLoader\Mod Sources\Injury\*.sln"