nuget restore ..\src\AgentZorge.2016.1.sln
msbuild ..\src\AgentZorge.2016.1.sln /p:Configuration=Release /p:DefineConstants=RESHARPER9

nuget pack AgentZorge.R2016_1.nuspec -Version %1
nuget push AgentZorge.R2016_1.%1.nupkg -ApiKey XXX -Source C:\Users\Andrey\Documents\Projects\LocalNuGetRepo