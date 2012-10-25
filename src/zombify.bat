SET ZOMBIFY_PATH=%CD%\packages\Zombify
SET NODE_PATH=%ZOMBIFY_PATH%\node_modules
SET OLDPATH = %CD%

ECHO Set node path to %NODE_PATH%

CD "C:\Users\Rob Ashton\Documents\GitHub\zombify\Hotelier\Hotelier.Test"

ECHO Running tests
"%ZOMBIFY_PATH%\node_modules\.bin\mocha.cmd"

CD OLDPATH