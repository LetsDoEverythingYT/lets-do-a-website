using lets_do_a_website.Data;
using lets_do_a_website.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ITrackerData, InMemoryTrackerData>();

builder.Services.AddSignalR();

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

     .AddCookie(options => {
         options.LoginPath = "/Login";
         options.LogoutPath = "/Logout";
     })
     .AddCookie("External")
    .AddTwitch(o =>
    {
        o.ClientId = builder.Configuration["Twitch:ClientId"];
        o.ClientSecret = builder.Configuration["Twitch:ClientSecret"];
        o.Scope.Clear();
        o.ForceVerify = true;
    });
    
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<TrackerHub>("/trackerhub");
app.Run();

