using PortalRepository.Repository;
using PortalRepository.Repository.DBRepository;
using PortalRepository.Repository.FileRepository;
using PortalService.Services;

namespace Portal
{
    public static class ServiceRegistration
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, FileRepository>();
            // can use the appSettings to switch between the two ways.
            //services.AddScoped<IRepository, DBRepository>();

            services.AddScoped<IDataFileService, DataFileService>();
        }
    }
}
