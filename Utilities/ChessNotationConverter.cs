using StockFishBlazorChess.Data;

namespace StockFishBlazorChess.Utilities
{
    public static class ChessNotationConverter
    {
        private static readonly Dictionary<int, string> PieceMap = new Dictionary<int, string>
    {
        { 1, "" }, { 2, "r" }, { 3, "n" }, { 4, "b" },
        { 5, "q" }, { 6, "k" }, { 11, "" }, { 12, "r" },
        { 13, "n" }, { 14, "b" }, { 15, "q" }, { 16, "k" }
    };

        private static readonly Dictionary<int, string> FileMap = new Dictionary<int, string>
    {
        { 0, "a" }, { 1, "b" }, { 2, "c" }, { 3, "d" },
        { 4, "e" }, { 5, "f" }, { 6, "g" }, { 7, "h" }
    };

        private static readonly Dictionary<int, string> RankMap = new Dictionary<int, string>
    {
        { 0, "1" }, { 1, "2" }, { 2, "3" }, { 3, "4" },
        { 4, "5" }, { 5, "6" }, { 6, "7" }, { 7, "8" }
    };

        public static string convertMoveToString(PieceChange pieceChange)
        {
            int rowIndex = 7 - pieceChange.toMove.row;
            int colIndex = pieceChange.toMove.col;

            string isHit = pieceChange.hitPiece == 0 ? "" : "x";
            string piece = PieceMap[pieceChange.movedPieceValue];
            piece = pieceChange.hitPiece != 0 && string.IsNullOrEmpty(piece) ? FileMap[pieceChange.fromMove.col] : piece;
            string row = RankMap[rowIndex];
            string col = FileMap[colIndex];

            return piece + isHit + col + row;
        }
    }

}
