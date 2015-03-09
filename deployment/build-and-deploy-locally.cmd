msbuild ..\src\AgentZorge.sln /p:Configuration=Release

nuget pack AgentZorge.nuspec -Version %1
nuget push AgentZorge.%1.nupkg -ApiKey XXX -Source C:\Users\Andrey\Documents\Projects\LocalNuGetRepo