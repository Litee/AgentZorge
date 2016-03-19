nuget restore ..\src\AgentZorge.10.0.sln
msbuild ..\src\AgentZorge.10.0.sln /p:Configuration=Release /p:DefineConstants=RESHARPER9

nuget pack AgentZorge.R10_0.nuspec -Version %1
nuget push AgentZorge.R10_0.%1.nupkg -ApiKey XXX -Source C:\Users\Andrey\Documents\Projects\LocalNuGetRepo