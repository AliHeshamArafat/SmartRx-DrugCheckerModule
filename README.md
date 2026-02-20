# SmartRX Drug Interaction Checker Module

A standalone Blazor Server application module focused exclusively on drug interaction checking functionality.

## Project Location

This module is located in: `F:\Freelance\SmartRX\Modules\SmartRx-DrugChecker\`

It is completely separate from the main SmartRX project and can be run independently.

## Project Structure

This module contains:
- **Layout & Navigation**: MainLayout, NavBar, and FooterSection components (same design as main project)
- **Drug Interaction Checker**: Complete drug interaction checking functionality
  - `DrugInteractionChecker.razor` - **Home page** (`@page "/"`) - Main checker interface
  - `DrugDoseCalculatorModal.razor` - Drug dose calculator modal component
  - `DrugInteractionResults.razor` - Results display page

## Features

- Drug interaction checking
- Patient information form
- Drug search and selection
- Safety checks (pregnancy, lactation)
- Condition and allergy tracking
- Detailed interaction results

## Running the Application

1. Navigate to the module directory:
   ```bash
   cd F:\Freelance\SmartRX\Modules\SmartRx-DrugChecker
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to the URL shown in the console (typically `https://localhost:5001` or `http://localhost:5000`)

## Module Separation

This module has been completely separated from the main SmartRX project (`F:\Freelance\SmartRX\SmartRx\`) to function as an independent module. The main project contains the home page, blogs, and services sections, while this module focuses solely on drug interaction checking functionality.

The DrugInteractionChecker page serves as the home page (`@page "/"`) of this module.
