# AgentZorge
AgentZorge is a plugin for Resharper simplifying work with mocking libraries (e.g. Moq). To install use standard Resharper plugins management UI.

## Compatibility

* Only C# langauage is supported
* Only Reshareper 8.2.x is supported. Shout if you want to see plugin for other versions.

## Supported features for Moq

### Suggest variable names based on mocked interface

Was:
![](https://github.com/Litee/AgentZorge/blob/master/media/variable-name-suggestion-was.png)

Now:
![](https://github.com/Litee/AgentZorge/blob/master/media/variable-name-suggestion-now.png)

* Moq: Highlight incompatibility between Setup and Callback parameters. Example: myMock.Setup(x => x.MyCall("")).Callback((int i) => {...})