using Microsoft.AspNetCore.Components;

namespace StockFishBlazorChess.Components.Pages
{
    public partial class Home
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = default!;

        private void klikk()
        {
            navigationManager.NavigateTo("game/asd");
        }
    }
}
