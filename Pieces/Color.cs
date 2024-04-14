namespace StockFishBlazorChess.Pieces
{
    public enum Color
    {
        White,
        Black,
        None
    }
    public static class ColorExtension
    {
        public static Color Toggle(this Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }
    }
}
