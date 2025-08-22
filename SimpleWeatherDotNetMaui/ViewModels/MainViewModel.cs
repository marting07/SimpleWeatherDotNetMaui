using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SimpleWeatherDotNetMaui.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IConfiguration _configuration;
    private string ApiKey => _configuration?["WeatherApi:ApiKey"];
    private string city = "Paris";
    private string temperature = string.Empty;
    private bool isLoading = false;
    
    public MainViewModel(IConfiguration configuration)
    {
        _configuration = configuration;
        _ = LoadWeatherAsync();
    }

    public string City
    {
        get => city;
        set
        {
            if (city != value)
            {
                city = value;
                OnPropertyChanged();
                _ = LoadWeatherAsync();
            }
        }
    }

    public string Temperature
    {
        get => temperature;
        set
        {
            temperature = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private async Task LoadWeatherAsync()
    {
        if (string.IsNullOrWhiteSpace(City))
            return;
        
        var apiKey = ApiKey;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Temperature = "Missing API Key";
            return;
        }

        try
        {
            IsLoading = true;
            using var http = new HttpClient();
            var url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={City}";
            var response = await http.GetStringAsync(url);
            var json = JsonDocument.Parse(response);
            var tempC = json.RootElement.GetProperty("current").GetProperty("temp_c").GetDouble();
            Temperature = $"{tempC}°C";
        }
        catch
        {
            Temperature = "Error";
        }
        finally
        {
            IsLoading = false;
        }
    }
}