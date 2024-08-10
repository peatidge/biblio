
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tela.Data.Core;
using Tela.Data.Organisation;
using Tela.Models;
//TODO:b 175. Add Tela.Services using statement to Program.cs
using Tela.Services;
//TODO:a 88. Jiggle'n'Wiggle
Console.WriteLine("Quick~Break: Time to Jiggle'n'Wiggle");
var builder = WebApplication.CreateBuilder(args);
//TODO: 1. Change the connection string to point to your local SQL Server instance
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//TODO:a 16. Add Identity Services with LibraryUser default identity 
builder.Services.AddDefaultIdentity<LibraryUser>(o => 
{
    //TODO:a 17. Configure Identity Options (for simplicity)
    o.SignIn.RequireConfirmedAccount = false;
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 1;
})
//TODO:a 18. Add Role Manager to Services
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();
//TODO:a 32. Register Ludus with DI
builder.Services.AddScoped<Ludus>();
//TODO:b 178. Register Mapster with DI
builder.Services.AddMapster();
//TODO:b 188. Add Library Services to DI
builder.Services.AddLibraryServices();
//TODO:b 316. Add view option to disable client side validation
//We can turn this on/off when needed to test server side validation
builder.Services.AddControllersWithViews()
    .AddViewOptions(o => o.HtmlHelperOptions.ClientValidationEnabled = true);


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
    options.OnAppendCookie = context =>
    {
        if (context.CookieOptions.Secure && context.CookieOptions.SameSite == SameSiteMode.None)
        {
            context.CookieOptions.Extensions.Add("Partitioned");
        }
    };
});
var app = builder.Build();
app.UseCookiePolicy();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
//TODO:b 217. Add Admninistration Area and required route pattern for all areas
app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
//TODO:b 327. Invoke Q.F prior to app run to generate a new fractal image 
//Q.F();
//TODO:b 425. Invoke EnsureExistDance prior to app run to ensure the existence of SQL view(s) and/or records in the database
EntityMapper.EnsureExistDance(app.Configuration);
app.Run();
//TODO:b 490. In this iteration some general work was also done to improve the overall quality (I hope) use a keen eye to spot the changes 
//TODO:b 491. to 500. You know what to do :)
//TODO:b 501. ...................................................................................................................*....*.....