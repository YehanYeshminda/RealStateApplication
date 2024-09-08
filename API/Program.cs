using System.Net;
using System.Security.Cryptography.X509Certificates;
using API;
using API.Models;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Jobs;
using API.Repos.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


// var serverCertificate = new X509Certificate2("./ssl/your_cert.pfx", "yeshanyesh");
// var serverCertificate = new X509Certificate2("/var/www/api.cparamount.com/ssl/your_cert.pfx", "yeshanyesh");


// builder.Services.Configure<ForwardedHeadersOptions>(options =>
// {
//     options.KnownProxies.Add(IPAddress.Parse("http://localhost:6000/"));
// });

builder.Services.AddControllers();

builder.Services.AddDbContext<CRMContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(provider => {
    IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
    return new DAL(configuration);
});

// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.Listen(IPAddress.Any, 5010, listenOptions =>
//     {
//         listenOptions.UseHttps(serverCertificate);
//     });
// });


builder.Services.AddSignalR();

builder.Services.AddSingleton<GlobalDataService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<GlobalDataService>();

builder.Services.AddOptions();
builder.Services.AddScoped<IScheduler>(_ => StdSchedulerFactory.GetDefaultScheduler().Result);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    
    // var jobKey = new JobKey("LeadDeleteJob");
    // q.AddJob<LeadJobs>(opts => opts.WithIdentity(jobKey));
    //
    // var triggerKey = new TriggerKey("EasyDrawTrigger", "MyTriggerGroup");
    // q.AddTrigger(opts => opts
    //     .ForJob(jobKey)
    //     .WithIdentity(triggerKey)
    //     .WithCronSchedule("0 0 22 ? * *", x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"))));
    
    var jobKey2 = new JobKey("CallReassignJob");
    q.AddJob<CallStatusResetJob>(opts => opts.WithIdentity(jobKey2));
    
    var triggerKey2 = new TriggerKey("CallsReassign", "MyTriggerGroup");
    q.AddTrigger(opts => opts
        .ForJob(jobKey2)
        .WithIdentity(triggerKey2)
        .WithCronSchedule("0 0 22 ? * *", x => x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"))));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseForwardedHeaders();

app.UseCors(builder =>
{
    builder.WithOrigins(
            "http://localhost:4200", 
            "https://paramountcrm.thinkview.click", 
            "http://192.168.70.222", 
            "http://127.0.0.1:5500", 
            "http://crm.cparamount.com", 
            "https://crm.cparamount.com")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials();
});

// app.Urls.Add("http://127.0.0.1:5000");
// app.Urls.Add("http://167.86.86.17:5000");

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.Urls.Add("https://0.0.0.0:5010");
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("/myhub");

app.Run();
