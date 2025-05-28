namespace MvcApp.Components
{
    public class MainMenu : ViewComponent
    {
        /// <summary>
        /// Invokes the component and returns a view
        /// <para>Example call:</para>
        /// <para><c>@await Component.InvokeAsync("MainMenu") </c></para>
        /// </summary>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;

            MenuModel Model = DataStore.GetMenu("MainMenu");            
            return View(Model);
        }
    }
}
