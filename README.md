# AgentZorge

AgentZorge is a plugin for Resharper simplifying work with mocking libraries - currently focused on Moq and NSubstitute. To install the plugin use standard Resharper plugins management UI.

## Compatibility

* Only C# language is supported
* I develop new features for the latest stable version of the R# (2016.1 at the moment) and backport them upon requests

## Supported features

#### Moq: Suggest variable names based on the name of mocked interface

![](https://github.com/Litee/AgentZorge/blob/master/media/variable-name-suggestion.png)

#### Moq: Suggest It.IsAny() when setting up mocked method, including full set of arguments for accepting any parameters. Recommendation: don't use It.IsAny() where specific value can be applied.

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-isany-argument.png)

#### Moq: Generate callbacks with correct parameter types/names

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-callback-argument.png)

#### Moq: Suggest existing mocks if they are available, otherwise suggest *new Mock().Object*

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-existing-mocks.png)

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-new-mock.png)

#### Moq: Highlight callbacks with invalid number of arguments or incompatible argument types

![](https://github.com/Litee/AgentZorge/blob/master/media/highlight-incompatible-callbacks.png)

#### NSubstitute: Suggest Arg.Any() when setting up mocked methods, including full set of arguments for accepting any parameters

![](https://github.com/Litee/AgentZorge/blob/master/media/suggest-argany-argument.png)