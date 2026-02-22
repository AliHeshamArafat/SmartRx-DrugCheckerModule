namespace SmartRx_DrugChecker.Services;

public class UserService
{
    private string _userRole = "Professional"; // Default to Professional, can be "Organization" or "Professional"
    private string _userName = "Ahmed";
    private string _userEmail = "ahmed123@gmail.com";
    private string _currentPlan = "Professional";
    private decimal _creditSpent = 210700m; // 210.7K
    private decimal _creditLimit = 216000m; // 216.0K

    public string UserRole
    {
        get => _userRole;
        set => _userRole = value;
    }

    public string UserName
    {
        get => _userName;
        set => _userName = value;
    }

    public string UserEmail
    {
        get => _userEmail;
        set => _userEmail = value;
    }

    public string CurrentPlan
    {
        get => _currentPlan;
        set => _currentPlan = value;
    }

    public decimal CreditSpent
    {
        get => _creditSpent;
        set => _creditSpent = value;
    }

    public decimal CreditLimit
    {
        get => _creditLimit;
        set => _creditLimit = value;
    }

    public bool IsOrganization => UserRole.Equals("Organization", StringComparison.OrdinalIgnoreCase);
    public bool IsProfessional => UserRole.Equals("Professional", StringComparison.OrdinalIgnoreCase);

    public decimal GetCreditUsagePercentage()
    {
        if (_creditLimit == 0) return 0;
        return (_creditSpent / _creditLimit) * 100;
    }

    public string GetControlPanelRoute()
    {
        return IsOrganization ? "/dashboard/organization" : "/dashboard/profiles";
    }

    // TODO: Replace with actual authentication/authorization logic
    // This should be populated from authentication state or API call
    public string GetUserRole()
    {
        return UserRole;
    }
}
