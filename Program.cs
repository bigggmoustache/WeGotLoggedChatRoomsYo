using BlazorServerSignalApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using BlazorServerSignalApp.Hubs;
using BlazorServerSignalApp.IService;
using BlazorServerSignalApp.Service;
using MongoDB.Driver;
using Blazored.SessionStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ChatLogDatabaseSettings>(
    builder.Configuration.GetSection("ChatLogDatabase"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IChatLogService, ChatLogService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
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

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToPage("/_Host");

app.Run();
