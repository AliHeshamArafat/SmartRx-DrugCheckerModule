namespace SmartRx_DrugChecker.Services;

public class MarketStateService
{
    private string _selectedMarket = "egypt";

    public string SelectedMarket
    {
        get => _selectedMarket;
        set
        {
            if (_selectedMarket != value)
            {
                _selectedMarket = value;
                OnMarketChanged?.Invoke();
            }
        }
    }

    public event Action? OnMarketChanged;
}
