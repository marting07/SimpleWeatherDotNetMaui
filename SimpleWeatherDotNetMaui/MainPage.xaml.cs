using SimpleWeatherDotNetMaui.ViewModels;

namespace SimpleWeatherDotNetMaui;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}