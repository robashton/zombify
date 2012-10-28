@ECHO OFF

SET ZOMBIFY_PATH=%~dp0
SET NODE_PATH=%ZOMBIFY_PATH%node_modules

ECHO Set NODE_PATH to %NODE_PATH%

IF %1.==. (SET WORKING_DIRECTORY="%CD%") ELSE (SET WORKING_DIRECTORY="%1")

ECHO Set working directoy to %WORKING_DIRECTORY%
ECHO Running tests with mocha in that directory

PUSHD "%WORKING_DIRECTORY%" & "%ZOMBIFY_PATH%node_modules\.bin\mocha.cmd" & POPD

ECHO Finished running tests
