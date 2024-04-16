using Microsoft.AspNetCore.Components;

namespace StockFishBlazorChess.Components.Pages
{
    public partial class Home
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = default!;

        private int elo { get; set; } = 1350;
        private bool isBlackSide { get; set; } = true;
        private string side = "white";
        private void startGame()
        {
            side = isBlackSide ? "white" : "black";
            navigationManager.NavigateTo($"game/{side}/{elo}");
        }
    }
}
