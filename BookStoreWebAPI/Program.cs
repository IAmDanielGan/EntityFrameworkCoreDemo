using BookStoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc(opt => opt.EnableEndpointRouting = false)
    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddDbContext<BookStoresDbContext>(opt => opt.UseSqlServer("name=BookStoresDB"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region Version Control Step 1
//NuGet Microsoft.AspNetCore.Mvc.Versioning
//NuGet Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer

builder.Services.AddApiVersioning(option =>
{
    option.RegisterMiddleware = true;
    option.DefaultApiVersion = new ApiVersion(1, 0);
    option.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddVersionedApiExplorer(option =>
{
    option.GroupNameFormat = "'v'VVVV";
    option.AssumeDefaultVersionWhenUnspecified = true;
});

#endregion

builder.Services.AddSwaggerGen(option => 
{
    #region Version Control Step 2

    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        option.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Daniel Gan",
                Email = "ganyuansong@gmail.com"
            },
            Description = ".Net6 WebAPI Demo Document",
            Title = ".Net6 WebAPI Demo Document",
            Version = description.ApiVersion.ToString()
        }
        ); ;
    }

    option.DocInclusionPredicate((version, apiDescription) =>
    {
        if (!version.Equals(apiDescription.GroupName))
            return false;
        IEnumerable<string>? values = apiDescription!.RelativePath.Split('/').Select(v => v.Replace("V{version}", apiDescription.GroupName));
        apiDescription.RelativePath = string.Join("/", values);
        return true;
    });

    option.DescribeAllParametersInCamelCase();

    #endregion 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    #region Version Control Step 3
    app.UseSwaggerUI(option =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Version:{description.GroupName.ToUpperInvariant()}");
        }
    });
    #endregion
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
