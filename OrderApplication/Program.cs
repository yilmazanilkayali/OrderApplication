using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderApplication.Data;
using OrderApplication.Data.Repository;
using OrderApplication.Data.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

//DI container tan�mlanarak soyut nesneler somut nesnelere d�n��t�r�ld�.

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//add defaulttokenproviders telefon no mail gibideğişiklerde doğrulama belirtesi oluşturmak için kullanıldı.

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
