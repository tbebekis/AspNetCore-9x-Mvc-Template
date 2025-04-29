namespace MvcApp.Library
{
    public interface IAppStartup
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureMiddlewares(IApplicationBuilder application);
        int ExcutionOrder { get; }
    }
}
