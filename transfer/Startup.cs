using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using transfer.Core.Account;
using transfer.Core.Account.Interface;
using transfer.Core.Messaging;
using transfer.Core.Services;
using transfer.Core.Services.Interface;
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
            services.AddHostedService<TransferenceProducer>();

            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMqConfig"));
            services.Configure<AccountServiceConfiguration>(Configuration.GetSection("AccountServiceConfig"));

            //Repository
            services.AddSingleton<ITransferRepository, TransferRepository>();
            services.AddSingleton<ITransferLogRepository, TransferLogRepository>();
            services.AddSingleton<ITransferStatusRepository, TransferStatusRepository>();

            //Transfer
            services.AddSingleton<ITransfer, Transfer>();

            //Account
            services.AddSingleton<IAccount, Account>();

            //Services
            services.AddHttpClient<IAccountService, AccountService>();
            services.AddSingleton<IAccountService, AccountService>();
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
