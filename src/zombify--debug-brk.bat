@ECHO OFF

SET ZOMBIFY_PATH=%~dp0..\bin\
SET NODE_PATH=%ZOMBIFY_PATH%node_modules

ECHO Set NODE_PATH to %NODE_PATH%

IF %1.==. (SET WORKING_DIRECTORY="%CD%") ELSE (SET WORKING_DIRECTORY="%1")

ECHO Set working directoy to %WORKING_DIRECTORY%
ECHO Running tests with mocha in that directory

PUSHD "%WORKING_DIRECTORY%" 
START CMD /C "%ZOMBIFY_PATH%node_modules\.bin\mocha.cmd" --debug-brk 
START CMD /C "%ZOMBIFY_PATH%node_modules\.bin\node-inspector.cmd"
START http://127.0.0.1:8080/debug?port=5858
POPD

ECHO Finished running tests
