# API Integration Guide

This document provides a comprehensive guide for integrating API endpoints into the SmartRX Drug Checker application.

## Table of Contents
1. [Overview](#overview)
2. [Pages Updated](#pages-updated)
3. [Data Models](#data-models)
4. [Integration Steps by Page](#integration-steps-by-page)
5. [Error Handling](#error-handling)
6. [Testing Checklist](#testing-checklist)

---

## Overview

All pages have been refactored to be API-ready. The following changes were made:

- ✅ Added `HttpClient` injection for API calls
- ✅ Converted synchronous methods to async/await patterns
- ✅ Added loading states (`isLoading`, `isSearchingDrugs`, etc.)
- ✅ Added error handling structure
- ✅ Created proper data models matching expected API responses
- ✅ Kept mock data as fallback for development

---

## Pages Updated

### 1. DrugInteractionChecker.razor (`/`)
**Purpose**: Main form for drug interaction checking

**Key Changes**:
- Drug search is now async
- Safety check submission is async
- Loading states added

**API Endpoints Needed**:
- `GET /api/drugs/search?query={searchTerm}` - Search for drugs
- `POST /api/drug-interaction/check` - Submit form and check interactions
- `GET /api/drug-calculator/{drugName}` - Get drug calculator details

---

### 2. DrugDoseCalculatorModal.razor
**Purpose**: Modal showing detailed drug dosing information

**Key Changes**:
- Content is now data-driven via `DrugCalculatorDetails` model
- Falls back to hardcoded content if no data provided

**API Endpoints Needed**:
- `GET /api/drug-calculator/{drugName}` - Get drug calculator details

---

### 3. DrugInteractionResults.razor (`/drug-interaction-results`)
**Purpose**: Display results of drug interaction check

**Key Changes**:
- Data loading is async
- Supports loading from session storage or API
- Loading and error states added

**API Endpoints Needed**:
- `GET /api/drug-interaction/results/{checkId}` - Get results by check ID
- Alternative: Read from session storage (set by form submission)

---

### 4. Organization.razor (`/dashboard/organization`)
**Purpose**: Manage organization users

**Key Changes**:
- User list loading is async
- Add member is async
- Loading and error states added

**API Endpoints Needed**:
- `GET /api/organization/users` - Get all organization users
- `POST /api/organization/users` - Add new user
- `GET /api/organization/seats` - Get total available seats (optional)

---

### 5. History.razor (`/dashboard/history`)
**Purpose**: Display check history

**Key Changes**:
- History loading is async
- Loading and error states added

**API Endpoints Needed**:
- `GET /api/drug-interaction/history` - Get check history
- Optional query parameters: `?riskLevel={level}&sortBy={field}&search={query}`

---

## Data Models

### Drug Model
```csharp
public class Drug
{
    public string Name { get; set; } = "";
    public string GenericName { get; set; } = "";
    // Optional: Add ID for API
    // public string Id { get; set; } = "";
}
```

### DrugCalculatorDetails Model
```csharp
public class DrugCalculatorDetails
{
    public string GenericName { get; set; } = "";
    public string Form { get; set; } = "";
    public string Frequency { get; set; } = "";
    public string DoseForm { get; set; } = "";
    public List<string> Indications { get; set; } = new();
    public List<DoseInfo> Doses { get; set; } = new();
    public List<PopulationAdjustment> PopulationAdjustments { get; set; } = new();
    public List<string> AdministrationInstructions { get; set; } = new();
}

public class DoseInfo
{
    public string Indication { get; set; } = "";
    public string IndicationSubtitle { get; set; } = "";
    public string Dose { get; set; } = "";
    public string Frequency { get; set; } = "";
    public string MaxDaily { get; set; } = "";
}

public class PopulationAdjustment
{
    public string Type { get; set; } = ""; // Renal, Hepatic, Hemodialysis
    public string Icon { get; set; } = "";
    public List<AdjustmentCard> Cards { get; set; } = new();
    public string? CautionTitle { get; set; }
    public string? CautionText { get; set; }
    public string? ContraindicatedText { get; set; }
}

public class AdjustmentCard
{
    public string Badge { get; set; } = "";
    public string Status { get; set; } = ""; // Allowed, Contraindicated, Use with caution
    public string StatusColor { get; set; } = ""; // green, red
    public List<string> Notes { get; set; } = new();
}
```

### Drug Interaction Results Models
```csharp
public class RiskSummary
{
    public int SameActiveIngredients { get; set; }
    public int DrugInteractions { get; set; }
    public int PregnancyRisks { get; set; }
    public int LactationRisks { get; set; }
    public int Contraindications { get; set; }
    public int AllergyRisks { get; set; }
}

public class OverallRisk
{
    public string Level { get; set; } = "";
    public int InteractionsCount { get; set; }
}

public class IngredientConflict
{
    public Drug Drug1 { get; set; } = new();
    public Drug Drug2 { get; set; } = new();
    public List<string> ConflictingIngredients { get; set; } = new();
}

public class DrugInteraction
{
    public Drug Drug1 { get; set; } = new();
    public Drug Drug2 { get; set; } = new();
    public string Severity { get; set; } = "";
    public string Description { get; set; } = "";
}

public class PregnancyRisk
{
    public Drug Drug { get; set; } = new();
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public List<TrimesterRisk> TrimesterRisks { get; set; } = new();
}

public class TrimesterRisk
{
    public string Trimester { get; set; } = "";
    public string Category { get; set; } = "";
}

public class LactationRisk
{
    public Drug Drug { get; set; } = new();
    public string SafetyStatus { get; set; } = "";
    public string Reason { get; set; } = "";
}

public class Contraindication
{
    public Drug Drug { get; set; } = new();
    public string Condition { get; set; } = "";
    public string Reason { get; set; } = "";
}

public class AllergyRisk
{
    public Drug Drug { get; set; } = new();
    public string Allergen { get; set; } = "";
    public string Note { get; set; } = "";
    public string Source { get; set; } = "";
}

public class Recommendation
{
    public int Number { get; set; }
    public string Text { get; set; } = "";
}
```

### Organization User Model
```csharp
public class OrganizationUser
{
    public string Name { get; set; } = "";
    public string Role { get; set; } = "";
    public string Email { get; set; } = "";
    public string AccessLevel { get; set; } = "";
    public string AvatarUrl { get; set; } = "";
    public string Status { get; set; } = "";
    public int AddedDaysAgo { get; set; }
}
```

### History Item Model
```csharp
public class HistoryItem
{
    public string PatientName { get; set; } = "";
    public int Age { get; set; }
    public string Gender { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public List<string> Medications { get; set; } = new();
    public string RiskLevel { get; set; } = "";
    public int CriticalCount { get; set; }
    public int ModerateCount { get; set; }
    public int MinorCount { get; set; }
    public string Timestamp { get; set; } = "";
    // Recommended: Add DateTime property
    // public DateTime CheckedAt { get; set; }
    // public string CheckId { get; set; } = "";
}
```

---

## Integration Steps by Page

### 1. DrugInteractionChecker.razor

#### Step 1: Update Drug Search
**Location**: `SearchDrugAsync()` method

**Current Code**:
```csharp
// TODO: Replace with actual API endpoint
// Example: var response = await Http.GetFromJsonAsync<List<Drug>>($"/api/drugs/search?query={Uri.EscapeDataString(drugSearchQuery)}");
```

**Implementation**:
```csharp
private async Task SearchDrugAsync()
{
    if (string.IsNullOrWhiteSpace(drugSearchQuery))
    {
        drugSearchResults.Clear();
        return;
    }

    isSearchingDrugs = true;
    StateHasChanged();

    try
    {
        var response = await Http.GetFromJsonAsync<List<Drug>>(
            $"/api/drugs/search?query={Uri.EscapeDataString(drugSearchQuery)}");
        
        drugSearchResults = response ?? new List<Drug>();
    }
    catch (HttpRequestException ex)
    {
        // Handle HTTP errors
        Console.WriteLine($"Error searching drugs: {ex.Message}");
        drugSearchResults.Clear();
        // TODO: Show user-friendly error message
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
        drugSearchResults.Clear();
    }
    finally
    {
        isSearchingDrugs = false;
        StateHasChanged();
    }
}
```

#### Step 2: Update Safety Check Submission
**Location**: `CheckDrugSafety()` method

**Implementation**:
```csharp
private async Task CheckDrugSafety()
{
    if (selectedDrugs.Count == 0)
    {
        // TODO: Show validation message to user
        return;
    }

    isCheckingSafety = true;
    StateHasChanged();

    try
    {
        var formData = new
        {
            PatientInfo = patientInfo,
            SelectedDrugs = selectedDrugs,
            SafetyOptions = safetyOptions,
            Conditions = conditions,
            Allergies = allergies
        };

        var response = await Http.PostAsJsonAsync("/api/drug-interaction/check", formData);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<DrugCheckResponse>();
            
            // Option 1: Store in session storage
            if (JS != null && result?.CheckId != null)
            {
                await JS.InvokeVoidAsync("sessionStorage.setItem", 
                    "drugCheckResults", 
                    JsonSerializer.Serialize(result));
                Navigation.NavigateTo($"/drug-interaction-results?checkId={result.CheckId}");
            }
            // Option 2: Navigate with check ID
            else if (result?.CheckId != null)
            {
                Navigation.NavigateTo($"/drug-interaction-results?checkId={result.CheckId}");
            }
            else
            {
                Navigation.NavigateTo("/drug-interaction-results");
            }
        }
        else
        {
            // Handle error response
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error: {errorContent}");
            // TODO: Show error to user
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error checking drug safety: {ex.Message}");
        // TODO: Show error to user
    }
    finally
    {
        isCheckingSafety = false;
        StateHasChanged();
    }
}
```

#### Step 3: Update Calculator Data Loading
**Location**: `OpenCalculatorAsync()` method

**Implementation**:
```csharp
private async Task OpenCalculatorAsync(Drug drug)
{
    calculatorSelectedDrug = drug;
    isCalculatorModalOpen = true;

    if (JS != null)
    {
        await JS.InvokeVoidAsync("smartRxModal.lockBodyScroll", true);
    }

    try
    {
        var details = await Http.GetFromJsonAsync<DrugCalculatorDetails>(
            $"/api/drug-calculator/{Uri.EscapeDataString(drug.Name)}");
        
        // Pass details to modal via a new parameter or update the modal to accept DrugCalculatorDetails
        // For now, you can serialize it to CalculatorData
        if (details != null)
        {
            calculatorData = JsonSerializer.Serialize(details);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading calculator data: {ex.Message}");
        calculatorData = null;
    }
}
```

---

### 2. DrugInteractionResults.razor

#### Step 1: Update Results Loading
**Location**: `LoadResultsDataAsync()` method

**Implementation**:
```csharp
private async Task LoadResultsDataAsync()
{
    isLoading = true;
    errorMessage = null;
    StateHasChanged();

    try
    {
        // Option 1: Get check ID from query string
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var checkId = queryParams.TryGetValue("checkId", out var id) ? id.ToString() : null;

        if (!string.IsNullOrWhiteSpace(checkId))
        {
            var response = await Http.GetFromJsonAsync<DrugInteractionResultsResponse>(
                $"/api/drug-interaction/results/{checkId}");
            
            if (response != null)
            {
                MapResultsToModels(response);
                isLoading = false;
                StateHasChanged();
                return;
            }
        }

        // Option 2: Get from session storage
        if (JS != null)
        {
            var resultsJson = await JS.InvokeAsync<string>(
                "sessionStorage.getItem", "drugCheckResults");
            
            if (!string.IsNullOrWhiteSpace(resultsJson))
            {
                var results = JsonSerializer.Deserialize<DrugInteractionResultsResponse>(resultsJson);
                if (results != null)
                {
                    MapResultsToModels(results);
                    isLoading = false;
                    StateHasChanged();
                    return;
                }
            }
        }

        // Fallback to mock data
        LoadMockData();
    }
    catch (Exception ex)
    {
        errorMessage = $"Error loading results: {ex.Message}";
        Console.WriteLine(errorMessage);
        LoadMockData(); // Fallback
    }
    finally
    {
        isLoading = false;
        StateHasChanged();
    }
}

private void MapResultsToModels(DrugInteractionResultsResponse response)
{
    riskSummary = response.RiskSummary ?? new RiskSummary();
    overallRisk = response.OverallRisk ?? new OverallRisk();
    sameActiveIngredients = response.SameActiveIngredients ?? new List<IngredientConflict>();
    drugInteractions = response.DrugInteractions ?? new List<DrugInteraction>();
    pregnancyRisks = response.PregnancyRisks ?? new List<PregnancyRisk>();
    lactationRisks = response.LactationRisks ?? new List<LactationRisk>();
    contraindications = response.Contraindications ?? new List<Contraindication>();
    allergyRisks = response.AllergyRisks ?? new List<AllergyRisk>();
    keyRecommendations = response.KeyRecommendations ?? new List<Recommendation>();
}
```

**Add Response Model**:
```csharp
public class DrugInteractionResultsResponse
{
    public RiskSummary? RiskSummary { get; set; }
    public OverallRisk? OverallRisk { get; set; }
    public List<IngredientConflict>? SameActiveIngredients { get; set; }
    public List<DrugInteraction>? DrugInteractions { get; set; }
    public List<PregnancyRisk>? PregnancyRisks { get; set; }
    public List<LactationRisk>? LactationRisks { get; set; }
    public List<Contraindication>? Contraindications { get; set; }
    public List<AllergyRisk>? AllergyRisks { get; set; }
    public List<Recommendation>? KeyRecommendations { get; set; }
    public string? CheckId { get; set; }
}
```

---

### 3. Organization.razor

#### Step 1: Update User List Loading
**Location**: `LoadOrganizationUsersAsync()` method

**Implementation**:
```csharp
private async Task LoadOrganizationUsersAsync()
{
    isLoading = true;
    errorMessage = null;
    StateHasChanged();

    try
    {
        var response = await Http.GetFromJsonAsync<OrganizationUsersResponse>(
            "/api/organization/users");
        
        if (response != null)
        {
            organizationUsers = response.Users ?? new List<OrganizationUser>();
            totalUsersCount = response.TotalSeats;
        }
    }
    catch (Exception ex)
    {
        errorMessage = $"Error loading users: {ex.Message}";
        Console.WriteLine(errorMessage);
        organizationUsers = new List<OrganizationUser>();
    }
    finally
    {
        isLoading = false;
        StateHasChanged();
    }
}
```

**Add Response Model**:
```csharp
public class OrganizationUsersResponse
{
    public List<OrganizationUser>? Users { get; set; }
    public int TotalSeats { get; set; }
}
```

#### Step 2: Update Add Member
**Location**: `HandleAddMember()` method

**Implementation**:
```csharp
private async Task HandleAddMember()
{
    if (string.IsNullOrWhiteSpace(newMember.FullName) || 
        string.IsNullOrWhiteSpace(newMember.Email))
    {
        // TODO: Show validation message to user
        return;
    }

    try
    {
        var request = new
        {
            FullName = newMember.FullName,
            Specialty = newMember.Specialty,
            Email = newMember.Email,
            Role = newMember.Role,
            ProfileImageUrl = newMember.ProfileImageUrl
        };

        var response = await Http.PostAsJsonAsync("/api/organization/users", request);
        
        if (response.IsSuccessStatusCode)
        {
            var newUser = await response.Content.ReadFromJsonAsync<OrganizationUser>();
            if (newUser != null)
            {
                organizationUsers.Add(newUser);
                CloseModal();
                // Optionally refresh the entire list
                await LoadOrganizationUsersAsync();
            }
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            // TODO: Show error to user
            Console.WriteLine($"Error adding member: {errorContent}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error adding member: {ex.Message}");
        // TODO: Show error to user
    }
}
```

---

### 4. History.razor

#### Step 1: Update History Loading
**Location**: `LoadHistoryAsync()` method

**Implementation**:
```csharp
private async Task LoadHistoryAsync()
{
    isLoading = true;
    errorMessage = null;
    StateHasChanged();

    try
    {
        // Build query string for filters
        var queryParams = new List<string>();
        if (selectedRiskFilter != "all")
        {
            queryParams.Add($"riskLevel={Uri.EscapeDataString(selectedRiskFilter)}");
        }
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            queryParams.Add($"search={Uri.EscapeDataString(searchQuery)}");
        }
        queryParams.Add($"sortBy={Uri.EscapeDataString(selectedSortBy)}");

        var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
        var response = await Http.GetFromJsonAsync<List<HistoryItem>>(
            $"/api/drug-interaction/history{queryString}");
        
        if (response != null)
        {
            historyItems = response;
        }
    }
    catch (Exception ex)
    {
        errorMessage = $"Error loading history: {ex.Message}";
        Console.WriteLine(errorMessage);
        historyItems = new List<HistoryItem>();
    }
    finally
    {
        isLoading = false;
        StateHasChanged();
    }
}
```

**Note**: Consider updating `HistoryItem` to include a `DateTime` property instead of string `Timestamp`:
```csharp
public class HistoryItem
{
    // ... existing properties ...
    public DateTime CheckedAt { get; set; }
    public string CheckId { get; set; } = "";
    
    // Keep Timestamp as computed property for display
    public string Timestamp => FormatTimestamp(CheckedAt);
    
    private string FormatTimestamp(DateTime dateTime)
    {
        // Format logic here
        var timeSpan = DateTime.Now - dateTime;
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hours ago • {dateTime:hh:mm tt}";
        // ... more formatting logic
    }
}
```

---

## Error Handling

### Recommended Error Handling Pattern

```csharp
try
{
    var response = await Http.GetFromJsonAsync<T>("/api/endpoint");
    // Process response
}
catch (HttpRequestException ex)
{
    // Network or HTTP errors
    errorMessage = "Unable to connect to server. Please check your connection.";
    // Log error
    Console.WriteLine($"HTTP Error: {ex.Message}");
}
catch (TaskCanceledException ex)
{
    // Timeout errors
    errorMessage = "Request timed out. Please try again.";
    Console.WriteLine($"Timeout: {ex.Message}");
}
catch (JsonException ex)
{
    // JSON parsing errors
    errorMessage = "Invalid response from server.";
    Console.WriteLine($"JSON Error: {ex.Message}");
}
catch (Exception ex)
{
    // Unexpected errors
    errorMessage = "An unexpected error occurred.";
    Console.WriteLine($"Unexpected Error: {ex.Message}");
}
```

### User Notification Service (Optional)

Consider creating a notification service for better UX:

```csharp
// Services/NotificationService.cs
public class NotificationService
{
    public event Action<string, NotificationType>? OnNotification;
    
    public void ShowError(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Error);
    }
    
    public void ShowSuccess(string message)
    {
        OnNotification?.Invoke(message, NotificationType.Success);
    }
}

public enum NotificationType
{
    Success,
    Error,
    Warning,
    Info
}
```

---

## Testing Checklist

### Before Integration
- [ ] Review all TODO comments in code
- [ ] Verify HttpClient is registered in `Program.cs` or `Startup.cs`
- [ ] Ensure API base URL is configured
- [ ] Check if authentication is required

### During Integration
- [ ] Replace mock data with API calls one page at a time
- [ ] Test each endpoint independently
- [ ] Verify loading states work correctly
- [ ] Test error scenarios (network errors, 404, 500, etc.)
- [ ] Verify data mapping is correct

### After Integration
- [ ] Test all user flows end-to-end
- [ ] Verify error messages are user-friendly
- [ ] Check loading indicators appear/disappear correctly
- [ ] Test with slow network (throttle in DevTools)
- [ ] Verify fallback behavior when API is unavailable
- [ ] Remove or comment out mock data

---

## API Endpoint Summary

| Page | Method | Endpoint | Purpose |
|------|--------|----------|---------|
| DrugInteractionChecker | GET | `/api/drugs/search?query={term}` | Search drugs |
| DrugInteractionChecker | POST | `/api/drug-interaction/check` | Submit check request |
| DrugInteractionChecker | GET | `/api/drug-calculator/{drugName}` | Get calculator details |
| DrugInteractionResults | GET | `/api/drug-interaction/results/{checkId}` | Get check results |
| Organization | GET | `/api/organization/users` | Get organization users |
| Organization | POST | `/api/organization/users` | Add new user |
| History | GET | `/api/drug-interaction/history` | Get check history |

---

## Additional Notes

### HttpClient Configuration

Ensure HttpClient is registered in your `Program.cs`:

```csharp
builder.Services.AddHttpClient();
// Or with base address:
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://api.yourserver.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

### Authentication

If your API requires authentication, add headers:

```csharp
// In your service or page
Http.DefaultRequestHeaders.Authorization = 
    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
```

### CORS

If calling external APIs, ensure CORS is configured on the server.

---

## Support

For questions or issues during integration, refer to:
- API documentation
- Backend team
- This guide

Last Updated: [Current Date]
