namespace MvcApp.Library
{
    static public partial class DataStore
    {

        public static MenuModel GetMenu(string MenuName)
        {
            // ● menu bar
            MenuModel MenuBar = new MenuModel();

            MenuModel mnuHome = MenuBar.Add("Home", "/");
            MenuModel mnuDemos = MenuBar.Add("Demos");
            MenuModel mnuProducts = MenuBar.Add("Products");

            // ● demos
            MenuModel mnuPluginTest = mnuDemos.Add("Plugin Test", "/plugin-test");
            MenuModel mnuAjaxDemos = mnuDemos.Add("Ajax Demos", "/ajax-demos");

            // ● products
            MenuModel mnuProductList = mnuProducts.Add("Product List", "/product/list");
            MenuModel mnuProductListPaging = mnuProducts.Add("Product Lis with Paging", "/product/paging");

            return MenuBar;
        }
    }
}
