REM This will create ITK\current-build which links to SimpleITK-2.3.1-CSharp-win64-x64
REM A dir junction is used rather than a symbolic link because it does not require admin
echo "**********************linking dirs...***************************"
rmdir /s /q %CD%\ITK\current-build 2>nul
mklink /J %CD%\ITK\current-build %CD%\ITK\SimpleITK-2.3.1-CSharp-win64-x64