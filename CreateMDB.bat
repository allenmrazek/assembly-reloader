REM Post-Build script that generates MDB from PDB
REM
REM Usage: [targetFileName] [destinationDirectory]

echo off

if "%unitydir%"=="" (
	echo Error while creating MDB:
	echo ERROR: Unity path not set. Make sure SetEnvironment.bat has run and is configured correctly.
	exit -1
)

set fail=false
if [%1]==[] set fail=true
if [%2]==[] set fail=true

if %fail%=="true" (
	echo Usage: [outputPath] [destinationDirectory]
	goto :eof
)


call:CheckUnity %1
call:CreateMdb %1
echo copying created mdb from %~dp1 to %~2 with filter %~nx1.mdb
robocopy "%~dp1 " "%~2 " %~nx1.mdb
call:CheckRoboCopyCode
set ERRORLEVEL=0
goto:eof




REM
REM CreateMdb - Given a fully qualified path to an assembly, generate MDB with same name + appended with .mdb
REM
:CreateMdb
echo Creating MDB from "%~1"

if exist %~1 (
call "%unitydir%\Editor\Data\MonoBleedingEdge\bin\cli.bat" "%unitydir%\Editor\Data\MonoBleedingEdge\lib\mono\4.5\pdb2mdb.exe" "%~1"

IF NOT EXIST "%~1".mdb (
echo ERROR: failed to generate %~1.mdb
exit -4
)
echo Successfully created %~nx1.mdb
goto:eof
) else (
echo CreateMdb: Error - %~1 not found!
exit -5
)

:CheckRoboCopyCode
    if %ERRORLEVEL% EQU 16 echo ***FATAL ERROR*** & exit 16
    if %ERRORLEVEL% EQU 15 echo OKCOPY + FAIL + MISMATCHES + XTRA & exit 15
    if %ERRORLEVEL% EQU 14 echo FAIL + MISMATCHES + XTRA & exit 14
    if %ERRORLEVEL% EQU 13 echo OKCOPY + FAIL + MISMATCHES & exit 13
    if %ERRORLEVEL% EQU 12 echo FAIL + MISMATCHES& exit 12
    if %ERRORLEVEL% EQU 11 echo OKCOPY + FAIL + XTRA & exit 11
    if %ERRORLEVEL% EQU 10 echo FAIL + XTRA & exit 10
    if %ERRORLEVEL% EQU 9 echo OKCOPY + FAIL & exit 9
    if %ERRORLEVEL% EQU 8 echo FAIL & exit 8
    if %ERRORLEVEL% EQU 7 echo OKCOPY + MISMATCHES + XTRA & goto end
    if %ERRORLEVEL% EQU 6 echo MISMATCHES + XTRA & goto end
    if %ERRORLEVEL% EQU 5 echo OKCOPY + MISMATCHES & goto end
    if %ERRORLEVEL% EQU 4 echo MISMATCHES & goto end
    if %ERRORLEVEL% EQU 3 echo OKCOPY + XTRA & goto end
    if %ERRORLEVEL% EQU 2 echo XTRA & goto end
    if %ERRORLEVEL% EQU 1 echo OKCOPY & goto end
    if %ERRORLEVEL% EQU 0 echo No Change & goto end
    :end  
goto:eof

:CheckUnity
echo Verifying Unity directory set...
IF NOT EXIST "%unitydir%\Editor\Data\MonoBleedingEdge\lib\mono\4.5\pdb2mdb.exe" (
echo ERROR: Failed to find pdb2mdb.exe; check Unity path
exit -1
)

echo Verifying PDB at %~1 exists...
IF NOT EXIST "%~dp1%~n1.pdb" (
echo ERROR: Failed to find "%~dp1%~n1.pdb"
exit -2
)
goto:eof