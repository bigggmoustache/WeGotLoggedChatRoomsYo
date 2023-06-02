using BlazorServerSignalApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using BlazorServerSignalApp.Hubs;
using BlazorServerSignalApp.IService;
using BlazorServerSignalApp.Service;
using MongoDB.Driver;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ChatLogDatabaseSettings>(
    builder.Configuration.GetSection("ChatLogDatabase"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IChatLogService, ChatLogService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IServerService, ServerService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthentication().AddGoogle(options =>
{
    var clientId = builder.Configuration["GoogleSignIn:ClientId"];
    options.ClientId = builder.Configuration["GoogleSignIn:ClientId"];
    options.ClientSecret = builder.Configuration["GoogleSignIn:ClientSecret"];
    options.ClaimActions.MapJsonKey("urn:google:profile", "link");
    options.ClaimActions.MapJsonKey("urn:google:image", "picture");
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpContextAccessor>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthentication();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
    // other settings go here
    endpoints.MapBlazorHub(options => {
        options.WebSockets.CloseTimeout = new TimeSpan(1, 1, 1);
        options.LongPolling.PollTimeout = new TimeSpan(1, 0, 0);
    })
);
app.MapBlazorHub();
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToPage("/_Host");

app.Run();
