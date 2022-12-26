using lets_do_a_website.Data;
using lets_do_a_website.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ITrackerData, InMemoryTrackerData>();

builder.Services.AddSignalR();


var clientId = Environment.GetEnvironmentVariable("TWITCHCLIENTID");
if (clientId == null)
{
    clientId = builder.Configuration["TWITCHCLIENTID"];
}
var clientSecret = Environment.GetEnvironmentVariable("TWITCHSECRET");
if (clientSecret == null)
{
    clientSecret = builder.Configuration["TWITCHSECRET"];
}

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})

     .AddCookie(options => {
         options.LoginPath = "/Login";
         options.LogoutPath = "/Logout";
         options.ExpireTimeSpan = TimeSpan.FromDays(35);
         options.SlidingExpiration = true;
     })
     .AddCookie("External")
    .AddTwitch(o =>
    {
        
        o.ClientId = clientId;
        o.ClientSecret = clientSecret;
        o.Scope.Clear();
        o.ForceVerify = true;
    });
    
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<TrackerHub>("/trackerhub");

var port = Environment.GetEnvironmentVariable("PORT") ?? "49154";

if (port.Equals("49154")) {
    app.Run();
} else
{
    app.Run("http://0.0.0.0:" + port);

}
