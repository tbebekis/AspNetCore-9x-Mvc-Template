# Localization

 
public void ConfigureServices(IServiceCollection services)
{  
    services.AddControllersWithViews();  
    services.AddLocalization(opt => { opt.ResourcesPath = "Resources";  });
}

List<CultureInfo> supportedCultures = new List<CultureInfo>
{
    new CultureInfo("en"),    
    new CultureInfo("de"),    
    new CultureInfo("fr"),    
    new CultureInfo("es"),    
    new CultureInfo("en-GB")
};


services.Configure<RequestLocalizationOptions>(options => {
    List<CultureInfo> supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("de-DE"),
        new CultureInfo("fr-FR"),
        new CultureInfo("en-GB")
    };   
    options.DefaultRequestCulture = new RequestCulture("en-GB");   
    options.SupportedCultures = supportedCultures;    
    options.SupportedUICultures = supportedCultures; 
});
