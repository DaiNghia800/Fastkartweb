using Fastkart.Helpers;
using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Fastkart.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Fastkart.Services.IServices;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"ENVIRONMENT: {builder.Environment.EnvironmentName}");
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.IdleTimeout = TimeSpan.FromHours(3);
    options.Cookie.Name = "fastkart.session";
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(3);
        options.LoginPath = "/login";
    })
    .AddCookie("External", options =>
    {
        options.Cookie.IsEssential = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.SignInScheme = "External";
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        googleOptions.CallbackPath = "/signin-google";
        googleOptions.Scope.Clear();
        googleOptions.Scope.Add("openid");
        googleOptions.Scope.Add("profile");
        googleOptions.Scope.Add("email");
        googleOptions.SaveTokens = true;
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.SignInScheme = "External";
        facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

        facebookOptions.CallbackPath = "/signin-facebook";
        facebookOptions.Scope.Clear();
        facebookOptions.Scope.Add("email");
        facebookOptions.Scope.Add("public_profile");

        facebookOptions.SaveTokens = true;

        facebookOptions.Fields.Clear();
        facebookOptions.Fields.Add("name");
        facebookOptions.Fields.Add("email");
        facebookOptions.Fields.Add("picture");
        facebookOptions.Fields.Add("first_name");
        facebookOptions.Fields.Add("last_name");
    });

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("NoCustomer", policy =>

    {
        // 2. Yêu cầu người dùng phải đăng nhập
        policy.RequireAuthenticatedUser();
        // 3. Yêu cầu người dùng KHÔNG CÓ vai trò "Customer"
        policy.RequireAssertion(context =>
            !context.User.IsInRole(WebConstants.ROLE_CUSTOMER));
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

builder.Services.AddScoped<MoMoService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IBlogService, BlogService>();


// (KHỐI AddSession BỊ TRÙNG LẶP Ở ĐÂY ĐÃ ĐƯỢC XÓA)


// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStatusCodePagesWithReExecute("/Home/NotFound404");
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAll");
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();
app.UseRouting();
app.UseHttpMethodOverride();

app.UseAuthentication();
app.UseAuthorization();

// Kích hoạt Session (đã được đăng ký ở trên)
app.UseSession();
// Routing
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


app.MapControllers();
// Route cho khu vực (Area) Admin (Vẫn cần nếu bạn dùng Area)
app.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Route mặc định (Luôn để ở cuối cùng)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "Account",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();