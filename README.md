# AgentZorge
AgentZorge is a plugin for Resharper simplifying work with mocking libraries (e.g. Moq). To install use standard Resharper plugins management UI.

## Supported features

* Moq: Suggest variable names based on mocked interface
* Moq: Highlight incompatibility between Setup and Callback parameters. Example: myMock.Setup(x => x.MyCall("")).Callback((int i) => {...})

## Compatibility

* Only C# langauage is supported
* Only Reshareper 8.2.x is supported