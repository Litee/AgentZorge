nuget restore ..\src\AgentZorge.2017.2.sln
msbuild ..\src\AgentZorge.2017.2.sln /p:Configuration=Release

nuget pack AgentZorge.R2017_2.nuspec -Version %1
nuget push AgentZorge.R2017_2.%1.nupkg -ApiKey XXX -Source C:\Users\Andrey\Documents\Projects\LocalNuGetRepo