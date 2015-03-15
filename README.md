# AgentZorge
AgentZorge is a plugin for Resharper simplifying work with mocking libraries (currently focused on Moq). To install the plugin use standard Resharper plugins management UI.

## Compatibility

* Only C# langauage is supported
* Only Reshareper 8.2.x is supported. Shout if you want to see plugin for other versions.

## Supported features

#### Moq: Suggest variable names based on the name of mocked interface

![](https://github.com/Litee/AgentZorge/blob/master/media/variable-name-suggestion.png)

#### Moq: Suggest It.IsAny() when setting up mocked method, including full set of arguments for accepting any parameters. Recommendation: don't use It.IsAny() where specific value can be applied.

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-mocked-method-parameters.png)

#### Moq: Generate callbacks with correct parameter types/names

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-callback-argument.png)

#### Moq: Suggest mocks where they are applicable

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-mocked-objects.png)

#### Moq: Highlight callbacks with invalid number of arguments or incompatible argument types

![](https://github.com/Litee/AgentZorge/blob/master/media/highlight-incompatible-callbacks.png)
