using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sudoku.Api.Data;
using Sudoku.Api.Services;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS - permissive for local development (adjust for production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DbContext - SQLite
var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=sudoku.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(conn));

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var secret = jwtSection.GetValue<string>("Key") ?? "please-change-this-secret-to-a-long-random-value";
var issuer = jwtSection.GetValue<string>("Issuer") ?? "sudoku-api";
var audience = jwtSection.GetValue<string>("Audience") ?? "sudoku-api-users";
var key = Encoding.UTF8.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// Apply EF Core migrations / ensure DB schema is up to date
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Try applying migrations; if model/migration mismatch exists, fall back to EnsureCreated
    try
    {
        db.Database.Migrate();
    }
    catch (InvalidOperationException ex)
    {
        // This can happen if there are pending model changes vs migrations (development). Fall back to EnsureCreated.
        Console.WriteLine("Warning: Migrate failed, falling back to EnsureCreated(): " + ex.Message);
        db.Database.EnsureCreated();
    }
}

// Serve frontend static files from front-end folder
var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "front-end");
if (Directory.Exists(staticFilesPath))
{
    var provider = new PhysicalFileProvider(Path.GetFullPath(staticFilesPath));
    var defaultFilesOptions = new DefaultFilesOptions { FileProvider = provider };
    defaultFilesOptions.DefaultFileNames.Clear();
    defaultFilesOptions.DefaultFileNames.Add("index.html");
    app.UseDefaultFiles(defaultFilesOptions);
    app.UseStaticFiles(new StaticFileOptions { FileProvider = provider });
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
