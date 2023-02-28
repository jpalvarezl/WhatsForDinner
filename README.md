## What's for dinner?

### Running the app

- Clone the repository
- Visual Studio 2022
- MAUI for Visual Studio
- Open the `WhatsForDinner.sln` file
- Within the `Services/OpenAIService.cs` class replace with your own the values of:
  - `s_Azure_OpenAI_Endpoint`
  - `s_Azure_OpenAI_Secret`

### About

This is a small sample app written using MAUI and the Azure OpenAI C# SDK demonstrating the streaming capabilities of the API.

The app simply receives through a prompt constraints under which it will suggest cooking recipes. We store in a list of strings all the prompts that the user enters, then we issue a request after compiling everything into a single prompt. Check the `services/PromptService.cs` file for details.

The streaming response, currently, returns the text by larger chunks. We've added some "trickery" to parcelise the incoming text and make it appear to arrive "word by word". This can be disabled by hardcoding to `default` the value of the local variable `tokenStreamDelay` to `default` within the `ViewModels/MainViewModel.cs::Submit` method.

### References

[Best practices for prompt engineering](https://help.openai.com/en/articles/6654000-best-practices-for-prompt-engineering-with-openai-api)
