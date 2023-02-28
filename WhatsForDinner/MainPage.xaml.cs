using WhatsForDinner.ViewModels;

namespace WhatsForDinner;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        UpdateSmoothingButtonText();
    }

    private void UpdateSmoothingButtonText(object _ = null, ValueChangedEventArgs __ = null)
    {
        MainViewModel viewModel = BindingContext as MainViewModel;
        string value = viewModel.SmoothTokenRenderingDelayMs == 0
            ? "OFF"
            : $"{viewModel.SmoothTokenRenderingDelayMs}ms";
        TokenResponseSmoothingButton.Text = $"Per-token response smoothing delay: {value}";
    }
}
