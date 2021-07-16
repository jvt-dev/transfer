using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using transfer.Core.Messaging;
using transfer.Core.Messaging.Interface;
using transfer.Core.Transfer;
using transfer.Core.Transfer.Interface;
using transfer.Infrastructure.Data;
using transfer.Infrastructure.Options;
using transfer.Infrastructure.Repository;
using transfer.Infrastructure.Repository.Interface;

namespace transfer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "transfer", Version = "v1" });
            });

            services.AddDbContext<TransferContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DbContext")));

            services.AddHostedService<TransferenceConsumer>();

            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMqConfig"));

            //Repository
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<ITransferLogRepository, TransferLogRepository>();
            services.AddScoped<ITransferStatusRepository, TransferStatusRepository>();

            //Transfer
            services.AddScoped<ITransfer, Transfer>();

            //Messaging
            services.AddScoped<IFactory, Factory>();
            services.AddScoped<ITransferenceProducer, TransferenceProducer>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "transfer v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
