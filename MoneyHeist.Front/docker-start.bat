@ECHO OFF
ECHO "Running envsubst on C:/app/config-template/publicConfig.js to C:/app/ClientApp/build/config/publicConfig.js"
C:\\envsubst\\envsubst.exe < "C:/app/config-template/publicConfig.js" > "C:/app/ClientApp/build/config/publicConfig.js"
dotnet ReactOnKestrel.dll