using DbUp;
using EduTrack.Api.Data;
using EduTrack.Api.Validators;
using EduTrack.Api.Middlewares;
using FluentValidation;
using EduTrack.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<ISqlConnectionFactory>(new SqlConnectionFactory(connectionString!));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentRequestValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run DbUp at startup, enabling SQL scripts
EnsureDatabase.For.SqlDatabase(connectionString);
var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsFromFileSystem("Scripts")
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();
if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EduTrack API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
