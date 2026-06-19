
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Patient.Data.InterfaceSql;
using Api.Patient.Data.ServiceSql;
using Api.Patient.Interfaces;
using Api.Psychologist.Data.InterfaceSql;
using Api.Psychologist.Data.ServiceSql;
using Api.Psychologist.Services;
using Data.Interface;
using Api.User.Data.ServicesSql;
using Api.User.Interfaces;
using Api.User.Services;
using tcc.Patient.Services;
using tcc.Psychologist.Interfaces;
using Api.Admin.Interfaces;
using Api.Admin.Services;
using Microsoft.OpenApi.Models;
using Api.Admin.Data.InterfaceSql;
using Api.Admin.Data.ServiceSql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TCC API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite o token JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

// services
    //User
        builder.Services.AddScoped<IUserSQL, UserServiceSql>();
        builder.Services.AddScoped<IUserInterface, UserService>();
    //Psychologist
        builder.Services.AddScoped<IPsychologistInterface, PsychologistService>();
        builder.Services.AddScoped<IPsychologistInterfaceSql, PsychologistServiceSql>();
    //Patient
        builder.Services.AddScoped<IPatientInterface, PatientService>();
        builder.Services.AddScoped<IPatientInterfaceSql, PatientServiceSql>();
    //Auth
        builder.Services.AddScoped<IAuthInterface, AuthService>();
    //Admin
        builder.Services.AddScoped<IAdmInterface, AdminService>();
        builder.Services.AddScoped<IAdminInterfaceSql, AdminServiceSql>();

//JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT Key not configured.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))  
        };
    });
builder.Services.AddAuthorization();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend"); 

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
