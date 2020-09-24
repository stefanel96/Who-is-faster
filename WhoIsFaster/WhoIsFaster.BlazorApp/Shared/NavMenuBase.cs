using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace WhoIsFaster.BlazorApp.Shared {
    public class NavMenuBase : ComponentBase {
        [CascadingParameter] 
        public bool Collapsed { get; set; }
        [CascadingParameter] 
        public string NavTitle { get; set; }
        public bool collapseNavMenu = true;

        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        
        public void ToggleNavMenu () {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}