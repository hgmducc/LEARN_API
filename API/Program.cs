using API.Data;
using API.Mappings;
using API.Repositories;
using API.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    // Định nghĩa security scheme kiểu Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n" +
                      "Nhập 'Bearer' [space] và sau đó là token của bạn trong ô bên dưới.\r\n\r\n" +
                      "Ví dụ: 'Bearer 12345abcdef'\r\n\r\n",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Thêm yêu cầu xác thực toàn cục cho Swagger
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x=>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        { 
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };

    });

// DI luôn phải đặt trước builder.Build(); nếu không sẽ gặp lỗi
builder.Services.AddDbContext<NZWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZConnection")));

builder.Services.AddScoped<IRegionRepository,SQLRegionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
