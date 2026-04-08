using ExplorerApp.WebMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("ExplorerApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ExplorerApiBaseUrl"] ?? "http://localhost:5081/");
});
builder.Services.AddScoped<IExplorerApiClientService, ExplorerApiClientService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Countries}/{action=Index}/{id?}");
app.Run();
