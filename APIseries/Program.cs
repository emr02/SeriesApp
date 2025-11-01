using APIseries.Mapping;
using APIseries.Models.DataManager;
using APIseries.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Newtonsoft.Json;

namespace APIseries
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Lecture de la chaîne de connexion
            var connectionString = builder.Configuration.GetConnectionString("SeriesDbContext");

            // 2. Enregistrement du DbContext
            builder.Services.AddDbContext<SeriesDbContext>(options =>
            {
                options
                    .UseNpgsql(connectionString);

                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();   // Affiche les valeurs dans les logs (uniquement dev)
                }
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // JSON.NET avec configuration pour gérer les références circulaires
            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    // Ignore les références circulaires (recommandé pour les APIs)
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                    // Alternative : préserver les références (génère des $id et $ref dans le JSON)
                    // options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    // options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

                    // Optionnel : formater le JSON pour le rendre plus lisible en dev
                    if (builder.Environment.IsDevelopment())
                    {
                        options.SerializerSettings.Formatting = Formatting.Indented;
                    }
                });

            // Enregistre AutoMapper (méthode moderne selon la doc officielle)
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            // Enregistrement des managers comme implémentations de IDataRepository
            builder.Services.AddScoped<IDataRepository<Utilisateur>, UtilisateurManager>();
            builder.Services.AddScoped<IDataRepository<Serie>, SerieManager>();

            // --- Ajout de la configuration CORS ---
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:7046") // ton front-end
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                // Active la page spéciale pour exceptions/migrations EF
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                // (Pour une API pure tu peux te passer de HSTS, à garder si exposée en prod)
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowLocalhost");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}