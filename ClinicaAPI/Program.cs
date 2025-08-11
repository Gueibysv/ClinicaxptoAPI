
using ClinicaAPI.DAL;
using ClinicaAPI.DAL.Repository;
using ClinicaAPI.DAL.Repository.Implementations;
using ClinicaAPI.Model;
using ClinicaAPI.Services;
using ClinicaAPI.Services.Implementations;
using ClinicaAPI.Shared.Repository;
using ClinicaAPI.Shared.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using ClinicaAPI.DAL.Helper;
using Autofac.Core;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at
https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title =
    "Clinica API",
        Version = "v1"
    });
    option.AddSecurityDefinition("Bearer", new
    Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new
    Microsoft.OpenApi.Models.OpenApiSecurityRequirement
{
{
new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
Reference = new Microsoft.OpenApi.Models.OpenApiReference
{
Type=Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
Id="Bearer"
}
},
new string[]{}
}
});
});
// Configure Entity Framework with Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // O nome do assembly onde as migrações serão criadas
// Configure ASP.NET Core Identity
builder.Services.AddIdentity<Utilizador, IdentityRole>(options =>
{
// Configurações de password (ajuste conforme sua política de segurança)
options.Password.RequireDigit = true;
options.Password.RequireLowercase = true;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequireUppercase = true;
options.Password.RequiredLength = 6;
options.Password.RequiredUniqueChars = 1;
// Configurações de Lockout
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
options.Lockout.MaxFailedAccessAttempts = 5;
options.Lockout.AllowedForNewUsers = true;
// Configurações de Utilizador
options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Integra o Identity com o seu DataContext
.AddDefaultTokenProviders(); // Adiciona provedores de token padrão (para reset de senha, etc.)
// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false; // Apenas para desenvolvimento, emprodução use HTTPS
options.TokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtSettings["Issuer"],
    ValidAudience = jwtSettings["Audience"],
    IssuerSigningKey = new
SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
};
});
// Configure CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
    policy =>
    {
        policy.AllowAnyOrigin() // Em produção, restrinja a origens específicas
    .AllowAnyMethod()
    .AllowAnyHeader();
    });
});
// Add AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Register your custom services and repositories
// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<ISubsistemaSaudeRepository,
SubsistemaSaudeRepository>();
builder.Services.AddScoped<IAtoClinicoRepository, AtoClinicoRepository>();
// Services
builder.Services.AddScoped(typeof(IService<,,,>), typeof(Service<,,,>));
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IProfissionalService, ProfissionalService>();
builder.Services.AddScoped<ISubsistemaSaudeService, SubsistemaSaudeService>();
builder.Services.AddScoped<IAtoClinicoService, AtoClinicoService>();
builder.Services.AddScoped<IUtilizadorService, UtilizadorService>();
builder.Services.AddScoped<IEmailService, EmailService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // ?? isso mostra o erro exato no navegador
    app.UseSwagger();
    app.UseSwaggerUI();
}

static async Task CreateRoles(IServiceProvider serviceProvider)
{
    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var UserManager = serviceProvider.GetRequiredService<UserManager<Utilizador>>(); // corrigido aqui

    string[] roleNames = { "Anonimo", "registado", "Administrativo", "Administrador" };

    foreach (var roleName in roleNames)
    {
        if (!await RoleManager.RoleExistsAsync(roleName))
        {
            await RoleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        await CreateRoles(serviceProvider); // Chama o método estático
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar roles: {ex.Message}");
        // (Opcional) logar o stacktrace
        Console.WriteLine(ex);
    }
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Use a política CORS definida
app.UseAuthentication(); // Deve vir antes de UseAuthorization
app.UseAuthorization();
app.MapControllers();

app.Run();


