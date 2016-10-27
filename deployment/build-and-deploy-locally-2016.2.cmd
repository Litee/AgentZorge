nuget restore ..\src\AgentZorge.2016.2.sln
msbuild ..\src\AgentZorge.2016.2.sln /p:Configuration=Release

nuget pack AgentZorge.R2016_2.nuspec -Version %1
nuget push AgentZorge.R2016_2.%1.nupkg -ApiKey XXX -Source C:\Users\Andrey\Documents\Projects\LocalNuGetRepo