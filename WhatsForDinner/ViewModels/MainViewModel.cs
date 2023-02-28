using Azure;
using Azure.AI.OpenAI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using WhatsForDinner.Services;

namespace WhatsForDinner.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private OpenAIService _openAIService;
    private PromptService _promptService;

    public MainViewModel(OpenAIService openAIService, PromptService promptService)   {
        _openAIService = openAIService;
        _promptService = promptService;
    }

    [ObservableProperty]
    string completionText;

    [ObservableProperty]
    string promptText;

    [ObservableProperty]
    int smoothTokenRenderingDelayMs = 0;

    [RelayCommand]
    async Task Submit()
    {
        CompletionText = $"{Environment.NewLine}{Environment.NewLine}** {PromptText} **{Environment.NewLine}";
        CompletionText += "  >> Connecting ... ";

        if (!string.IsNullOrWhiteSpace(PromptText))
        {
            _promptService.addNewUserConstraint(PromptText);
        }
        var fullPrompt = _promptService.compilePrompt();

        PromptText = "";

        /*TimeSpan tokenStreamDelay = SmoothTokenRenderingDelayMs > 0
            ? TimeSpan.FromMilliseconds(SmoothTokenRenderingDelayMs)
            : default;*/

        TimeSpan tokenStreamDelay = TimeSpan.FromMilliseconds(40);
        IAsyncEnumerable<string> choiceChunks = _openAIService.GetCompletionsSinglePrompt(fullPrompt, tokenStreamDelay);
        var stopwatch = Stopwatch.StartNew();

        await foreach (string choiceChunk in choiceChunks)
        {
            CompletionText += choiceChunk;
        }

        CompletionText += $"{Environment.NewLine}{Environment.NewLine}  >> Total streaming response time: {stopwatch.Elapsed.TotalSeconds:0.0}s";
    }

    [RelayCommand]
    public void Clear() {
        _promptService.clearUserConstraints();
        CompletionText = "";
    }

    [RelayCommand]
    public void PrintPromptHistory() {
        CompletionText = $"Your prompt history:{Environment.NewLine}{Environment.NewLine}";
        CompletionText += _promptService.joinUserConstraints();
    }
}

