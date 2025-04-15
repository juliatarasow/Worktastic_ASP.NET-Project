using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Worktastic.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//wenn "options.SignIn.RequireConfirmedAccount = true" dann muss bei Registrierung mit Email best�tigt werden
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//baut Services auf
var app = builder.Build();

//using->während der Laufzeit
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    await CreateRole(roleManager, "Admin");
    await CreateDefaultUser(userManager, "Admin", "admin@worktastic.com", "Test123.");
}

async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
{ 
    //Schaut ob es keinen Admin gibt
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        //legt neue IdentityRole "Admin" an
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}

async Task CreateDefaultUser(UserManager<IdentityUser> userManager, string roleName, string userName, string password)
{
    var user = await userManager.FindByNameAsync(userName);
    if (user == null)
    {
        var newUser = new IdentityUser()
        {
            UserName = userName,
            Email = userName
        };

        var result = await userManager.CreateAsync(newUser, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, roleName);
        }
    }
}

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
