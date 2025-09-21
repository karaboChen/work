using Microsoft.OpenApi.Models;
using work.Repository;
using work.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Work API",
        Version = "v1",
        Description = "ÃÄ§½&«È¤á"
    });
});


#region DIµù¥U
builder.Services.AddScoped<PharmaciesService>();
builder.Services.AddScoped<OpeningHoursRepository>();
builder.Services.AddScoped<PharmacyMaskRepository>();
builder.Services.AddScoped<PurchaseHistoriesRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PharmacyRepository>();
#endregion


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

app.Run();
