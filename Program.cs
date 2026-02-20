// Configure ContentRoot and WebRoot to point to project directory (not bin directory)
var contentRoot = Directory.GetCurrentDirectory();
var projectRoot = contentRoot;

// If running from bin directory, find the project root by looking for .csproj file
if (contentRoot.Contains(@"\bin\Debug") || contentRoot.Contains(@"\bin\Release"))
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

var wwwrootPath = Path.Combine(projectRoot, "wwwroot");

// Create builder with WebApplicationOptions to set content root and web root
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

app.Run();
