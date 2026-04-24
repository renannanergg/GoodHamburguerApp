using GoodHamburguerApp.Api.Middleware;
using GoodHamburguerApp.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
 
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
