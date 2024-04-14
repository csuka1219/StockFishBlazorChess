using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace StockFishBlazorChess.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        ILocalStorageService localStorage { get; set; } = default!;

        private bool isDrawerOpen = true;

        private MudTheme myCustomTheme = new MudTheme()
        {
            Palette = new PaletteLight()
            {
                Primary = Colors.Red.Default,
                Secondary = Colors.Green.Accent4,
                AppbarBackground = Colors.Red.Default,
            },
            PaletteDark = new PaletteDark()
            {
                Primary = "#00e676ff",
                Secondary = "#FFFFFF",
                Tertiary = "#00C853",
                Background = "#37474fff",
                Dark = "#263238ff",
                DrawerBackground = "#263238ff",
                AppbarBackground = "#263238ff",
                DrawerText = "#FFFFFF",
                DrawerIcon = "#FFFFFF",
                Surface = "#263238ff",
                TableStriped = "#37474fff",
                TableLines = "#424242ff",
            },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            string uniqueGuid = await localStorage.GetItemAsync<string>("uniqueGuid");

            if (string.IsNullOrEmpty(uniqueGuid))
            {
                // Save the GUID to localStorage
                await localStorage.SetItemAsync("uniqueGuid", Guid.NewGuid().ToString());
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void toggleDrawer()
        {
            isDrawerOpen = !isDrawerOpen;
        }

        private string getAvatarSize()
        {
            return isDrawerOpen ? "width:56px;height:56px;" : "width:40px;height:40px;";
        }
    }
}
