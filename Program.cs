using AssemblyService;
using AssemblyService.Attributes;
using AssemblyService.Attributes.ValidationAttributes;
using AssemblyService.Services;
using AssemblyService.Utilities.CommunicationClientUtility;
using AssemblyService.Utilities.EmailAttachmentUtility;
using AssemblyService.Utilities.EmailServiceUtility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var server = Environment.GetEnvironmentVariable("DB_SERVER");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var user = Environment.GetEnvironmentVariable("DB_USER");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
//var connectionString = $"Server={server};Port={port};Database={database};User={user};Password={password};";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddHttpClient<CommunicationClientUtility>(client =>
{
});


GlobalAttributes.mySQLConfig.connectionString = connectionString;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
builder.Services.AddScoped<IAssembleService, AssembleService>();
builder.Services.AddScoped<IEmailAttachmentServiceUtility, EmailAttachmentServiceUtility>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAssembleRequestValidationService, AssembleRequestValidationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    dbContext.Database.Migrate();
}

app.Run();
