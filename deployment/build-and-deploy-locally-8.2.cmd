nuget restore ..\src\AgentZorge.8.2.sln
msbuild ..\src\AgentZorge.8.2.sln /p:Configuration=Release /p:DefineConstants=RESHARPER8

nuget pack AgentZorge.nuspec -Version %1
nuget push AgentZorge.%1.nupkg -ApiKey XXX -Source C:\Users\Andrey\Documents\Projects\LocalNuGetRepo