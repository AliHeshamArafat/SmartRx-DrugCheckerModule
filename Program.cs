// Configure ContentRoot and WebRoot
var contentRoot = Directory.GetCurrentDirectory();
var projectRoot = contentRoot;

// If running from bin directory (Debug/Release), find the project root
if (contentRoot.Contains(Path.Combine("bin", "Debug")) || 
    contentRoot.Contains(Path.Combine("bin", "Release")) ||
    contentRoot.Contains("bin/Debug") || 
    contentRoot.Contains("bin/Release"))
{
    var currentDir = new DirectoryInfo(contentRoot);
    while (currentDir != null && !currentDir.GetFiles("*.csproj").Any())
    {
        currentDir = currentDir.Parent;
    }
    if (currentDir != null)
    {
        projectRoot = currentDir.FullName;
    }
}

// Check for wwwroot in project root first, then in current directory
var wwwrootPath = Path.Combine(projectRoot, "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    // Fallback: check in current directory (for published apps)
    wwwrootPath = Path.Combine(contentRoot, "wwwroot");
}

// Create builder with WebApplicationOptions
var options = new WebApplicationOptions
{
    ContentRootPath = projectRoot,
    WebRootPath = Directory.Exists(wwwrootPath) ? wwwrootPath : null
};

var builder = WebApplication.CreateBuilder(options);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<SmartRx_DrugChecker.Services.MarketStateService>();
builder.Services.AddSingleton<SmartRx_DrugChecker.Services.ModalStateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Only use HTTPS redirection if HTTPS is available
if (app.Configuration["ASPNETCORE_URLS"]?.Contains("https") == true || 
    Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Contains("https") == true)
{
    app.UseHttpsRedirection();
}

// Configure static files with custom MIME types
var staticFileOptions = new StaticFileOptions
{
    ContentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider()
};
// Ensure AVIF files are served with correct MIME type
if (staticFileOptions.ContentTypeProvider is Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider provider)
{
    provider.Mappings[".avif"] = "image/avif";
}

app.UseStaticFiles(staticFileOptions);
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Configure port for Render (uses PORT environment variable)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Clear();
    app.Urls.Add($"http://0.0.0.0:{port}");
    Console.WriteLine($"Application listening on port {port}");
}
else
{
    Console.WriteLine("PORT environment variable not set, using default port");
}

Console.WriteLine($"Content Root: {app.Environment.ContentRootPath}");
Console.WriteLine($"Web Root: {app.Environment.WebRootPath}");

app.Run();
