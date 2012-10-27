ECHO Set NODE_PATH to %NODE_PATH%

ECHO Running tests with mocha
PUSHD "C:\Users\Rob Ashton\Documents\GitHub\zombify2\Hotelier\Hotelier.Test" & "%ZOMBIFY_PATH%\node_modules\.bin\mocha.cmd" & POPD

ECHO Finished running tests
