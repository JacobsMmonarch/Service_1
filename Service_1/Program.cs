using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Service_1;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������ Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "���������� �����", Version = "v1" });
    c.OperationFilter<AddIncludeLoopbackParameter>(); // ��������� ������ ��� Swagger
});

builder.Services.AddControllers(); // ���������� ������ ������������

builder.Services.AddHostedService<NetworkMonitoringService>(); // ������������ ������� ������

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "����������"));

app.MapControllers();

app.Run();

public class AddIncludeLoopbackParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.Name == "GetNetworkInfo") // ��������� ��� ������ �����������
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "includeLoopback",
                In = ParameterLocation.Query,
                Description = "Include loopback interfaces",
                Required = false,
                Schema = new OpenApiSchema { Type = "boolean" }
            });
        }
    }
}
