using Eskul;
using Eskul.APIClient;
using Eskul.Controllers;
using Eskul.Custom;
using Eskul.Hubs;
using Eskul.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SmartPaperEdms.Web.App_Code;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ILoggerErr, LoggerErr>();
builder.Services.AddSingleton<MyUtilities>();
builder.Services.AddSingleton(provider => "C:\\ESKUL\\LOGS\\");
builder.Services.AddSignalR();
var app = builder.Build();
SessionHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
var provider=builder.Services.BuildServiceProvider();
var congig = provider.GetRequiredService<IConfiguration>();
var APIUrl = congig.GetValue<string>("Base_Url");


//app.Use(async (context, next) =>
//{
//    RequestHandler request = new RequestHandler(congig);
//     var Url = "Settings/SystemConfig/" + "SCHOOLLOGO";
//    var b =  await request.Get<SysConfigVm>(Url);
//    string logo="";

//    if(b.Count > 0)
//    {
//        logo = b.Where(x => x.SysconfigName == "SCHOOLLOGO").FirstOrDefault().SysconfigValue;
//    }
//    context.Items["AppLogo"] = logo;
//    try
//    {
//        await next.Invoke();
//    }
//    catch (Exception ex)
//    {

//        throw;
//    }
   

//});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default2",
    pattern: "{controller}/{action=Index}/{id1?}/{id2?}/{id3?}/{id4?}/{id5?}");
app.MapControllerRoute(
    name: "default3",
    pattern: "{controller=Login}/{action=Index}/{id?}/{classs?}/{streamcode?}/{subjectcode?}");
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    
    endpoints.MapHub<StudentCountHub>("/StudentCountHub");
});
app.Run();
