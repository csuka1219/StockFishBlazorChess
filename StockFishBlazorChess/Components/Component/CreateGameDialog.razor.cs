using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace StockFishBlazorChess.Components.Component
{
    public partial class CreateGameDialog
    {
        [Inject]
        private NavigationManager navigationManager { get; set; } = default!;

        [CascadingParameter]
        private MudDialogInstance? mudDialog { get; set; }

        private string gameName = "";

        private void cancel()
        {
            mudDialog!.Cancel();
        }

        private void create()
        {
            navigationManager!.NavigateTo("game/" + gameName);
            mudDialog!.Close();
        }
    }
}
