nuget restore ..\src\AgentZorge.9.2.sln
msbuild ..\src\AgentZorge.9.2.sln /p:Configuration=Release /p:DefineConstants=RESHARPER9

nuget pack AgentZorge.R9_2.nuspec -Version %1
nuget push AgentZorge.R9_2.%1.nupkg -ApiKey XXX -Source C:\Users\Andrey\Documents\Projects\LocalNuGetRepo