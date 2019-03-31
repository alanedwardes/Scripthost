Remove-Item publish -Recurse

function Publish-Workshop { dotnet publish src/Scripthost/Scripthost.csproj -r $args -c Release -o $PSScriptRoot/publish/$args }

Publish-Workshop linux-x64
Publish-Workshop osx-x64
Publish-Workshop win-x64