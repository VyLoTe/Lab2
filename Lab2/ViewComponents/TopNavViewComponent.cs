using Lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.ViewComponents
{
    public class TopNavViewComponent : ViewComponent
    {
        private List<NavItem> NavItems;

        public TopNavViewComponent()
        {
            NavItems = new List<NavItem>() {
            new NavItem { Title = "Home", Url = "/", Order = 2 },
            new NavItem { Title = "About", Url = "/about", Order = 3 },
            new NavItem { Title = "Dashboard", Url = "/dashboard", Order = 1 }
            };
        }
        public IViewComponentResult Invoke(List<NavItem> navItems, string sort = "asc")
        {
            return View("RenderTopNav", navItems);
        }
    }
}
