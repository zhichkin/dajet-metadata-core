using DaJet.Http.DataMappers;
using DaJet.Http.Model;
using DaJet.Metadata;

namespace DaJet.Http.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationOptions options = new()
            {
                Args = args,
                ContentRootPath = AppContext.BaseDirectory
            };

            var builder = WebApplication.CreateBuilder(options);

            builder.Host.UseSystemd();
            builder.Host.UseWindowsService();

            ConfigureServices(builder.Services);

            // Add services to the container.
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

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            
            app.MapControllers();

            app.UseStaticFiles();
            app.UseBlazorFrameworkFiles();

            app.Run();
        }
        private static void ConfigureServices(IServiceCollection services)
        {
            MetadataService metadataService = new();

            InfoBaseDataMapper mapper = new();
            List<InfoBaseModel> list = mapper.Select();
            foreach (InfoBaseModel entity in list)
            {
                if (!Enum.TryParse(entity.DatabaseProvider, out DatabaseProvider provider))
                {
                    provider = DatabaseProvider.SqlServer;
                }

                metadataService.Add(new InfoBaseOptions()
                {
                    Key = entity.Name,
                    DatabaseProvider = provider,
                    ConnectionString = entity.ConnectionString
                });
            }

            services.AddSingleton<IMetadataService>(metadataService);
        }
    }
}