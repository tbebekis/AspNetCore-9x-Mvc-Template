namespace MvcApp.Library
{
    public interface IMvcAppStartup
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureMiddlewares(IApplicationBuilder application);
        int ExcutionOrder { get; }
    }
}
