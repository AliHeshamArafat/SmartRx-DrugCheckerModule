namespace SmartRx_DrugChecker.Services;

public class ModalStateService
{
    private bool _isModalOpen = false;

    public bool IsModalOpen
    {
        get => _isModalOpen;
        set
        {
            if (_isModalOpen != value)
            {
                _isModalOpen = value;
                OnModalStateChanged?.Invoke();
            }
        }
    }

    public event Action? OnModalStateChanged;
}
