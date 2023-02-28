
using Azure;
using Azure.AI.OpenAI;
using System.Diagnostics;

namespace WhatsForDinner.Services;
public class OpenAIService
{

    private OpenAIClient _openAIClient = CreateClient();

    public async IAsyncEnumerable<string> GetCompletionsSinglePrompt(
        string prompt,
        TimeSpan minimumResponseTokenDelay = default) {
        CompletionsOptions requestOptions = new CompletionsOptions()
        {
            Prompt = { prompt },
            MaxTokens = 512,
            LogProbability = 1,
        };

        Response<StreamingCompletions> response = await _openAIClient.GetCompletionsStreamingAsync(
                s_Completions_Deployment_Id,
                requestOptions);
        using StreamingCompletions streamingCompletions = response.Value;

        Stopwatch elapsedWatch = Stopwatch.StartNew();
        TimeSpan lastTokenElapsed = elapsedWatch.Elapsed;

        await foreach (StreamingChoice choice in streamingCompletions.GetChoicesStreaming())
        {
            await foreach (string choiceToken in choice.GetTextStreaming())
            {
                if (minimumResponseTokenDelay != default)
                {
                    TimeSpan currentTokenElapsed = elapsedWatch.Elapsed;
                    TimeSpan elapsedBetweenTokens = currentTokenElapsed - lastTokenElapsed;
                    if (elapsedBetweenTokens < minimumResponseTokenDelay)
                    {
                        await Task.Delay(minimumResponseTokenDelay - elapsedBetweenTokens);
                    }
                }
                lastTokenElapsed = elapsedWatch.Elapsed;
                yield return choiceToken;
            }
        }
    }

    public async Task<Completions> GetCompletionsSimple(string prompt) {
        CompletionsOptions requestOptions = new CompletionsOptions()
        {
            Prompt = { prompt },
            MaxTokens = 512,
            LogProbability = 1,
        };

        Response<Completions> response = await _openAIClient.GetCompletionsAsync(
                s_Completions_Deployment_Id,
                requestOptions);

        return response.Value;
    }

    private static OpenAIClient CreateClient()
    {
        var uri = new Uri(s_Azure_OpenAI_Endpoint);
        var credentials = new AzureKeyCredential(s_Azure_OpenAI_Secret);
        return new OpenAIClient(uri, credentials);
    }

    private static string s_Azure_OpenAI_Endpoint = "<YOUR_VALUE>";
    private static string s_Azure_OpenAI_Secret = "<YOUR_VALUE>";
    private static string s_Completions_Deployment_Id = "text-davinci-003";
}

