using Microsoft.AspNetCore.Components;

namespace WhoIsFaster.BlazorApp.Shared {
    public class NavMenuBase : ComponentBase {
        [CascadingParameter] public bool Collapsed { get; set; }
        [CascadingParameter] public string NavTitle { get; set; }
        public bool collapseNavMenu = true;

        public string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        public void ToggleNavMenu () {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}