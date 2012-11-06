@ECHO ON

REM The following directory is for .NET 2.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo Installing BetEx247.Services.exe
echo ---------------------------------------------------
InstallUtil /i E:\beauty\Dropbox\Betting\newcode\BetEx247.Services\bin\Debug\BetEx247Services.exe
echo ---------------------------------------------------
echo Done.
pause