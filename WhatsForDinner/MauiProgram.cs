﻿using Microsoft.Extensions.Logging;
using WhatsForDinner.ViewModels;
using WhatsForDinner.Services;

namespace WhatsForDinner;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<OpenAIService>();
        builder.Services.AddSingleton<PromptService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

