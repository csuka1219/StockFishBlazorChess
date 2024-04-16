namespace StockFishBlazorChess.Components.Pages
{
    // Game.UI.cs - Part of the partial Game class focusing on UI
    public partial class Game
    {
        private string getCellCss(int index)
        {
            string backgroundColor = index % 2 == 0 ? "#eeeed2" : "#769656";
            return "height:10vw; width:10vh; max-height: 64px; max-width: 64px; background-color:" + backgroundColor;
        }

        private string getPlayerTableView()
        {
            return isWhiteSide ? string.Empty : "transform: rotate(180deg);";
        }

        private string getPlayerPieceView()
        {
            isWhiteSide = string.Equals(side, "white");
            return isWhiteSide ? string.Empty : "transform: rotate(180deg);";
        }
    }

}
