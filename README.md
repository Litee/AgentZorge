# AgentZorge
AgentZorge is a plugin for Resharper simplifying work with mocking libraries (e.g. Moq). To install use standard Resharper plugins management UI.

## Compatibility

* Only C# langauage is supported
* Only Reshareper 8.2.x is supported. Shout if you want to see plugin for other versions.

## Supported features for Moq

### Suggest variable names based on mocked interface

Was | Now
----|-----
![](https://github.com/Litee/AgentZorge/blob/master/media/variable-name-suggestion-was.png) | ![](https://github.com/Litee/AgentZorge/blob/master/media/variable-name-suggestion-now.png)

### Suggest It.IsAny<T> when setting up mock

Was | Now
----|-----
![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-mocked-method-parameters-was.png) | ![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-mocked-method-parameters-now.png)

### Generate callbacks

### Highlight callbacks with invalid number of arguments or incompatible argument types

### Suggest declared mocks when they are applicable