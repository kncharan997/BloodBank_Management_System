
using BloodBankManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        // Add session services
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout duration
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        builder.Services.AddDbContext<BloodBankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddSession();
        var app = builder.Build();
        app.UseSession();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BloodBankContext>();
            var expiredBlood = dbContext.BloodInventories.Where(b => b.ExpiryDate < DateTime.Today).ToList();
            if (expiredBlood.Any())
            {
                dbContext.BloodInventories.RemoveRange(expiredBlood);
                dbContext.SaveChanges();
            }
        }


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
