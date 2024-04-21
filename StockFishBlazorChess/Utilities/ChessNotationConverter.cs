using StockFishBlazorChess.Data;
using StockFishBlazorChess.Pieces;
using StockFishBlazorChess.Rules;
using System.Text;

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

        public static string convertMoveToFEN(PieceChange pieceChange)
        {
            if (pieceChange.isCastling)
            {
                return getCastlingFEN(pieceChange);
            }

            string isCheckString = pieceChange.isCheck && !pieceChange.isCheckmate ? "+" : "";

            string isCheckmateString = pieceChange.isCheckmate ? "#" : "";

            int rowIndex = 7 - pieceChange.toMove.row;
            int colIndex = pieceChange.toMove.col;

            bool isTake = pieceChange.hitPiece != 0 || pieceChange.isEnpassant;
            string isTakeString = isTake ? "x" : "";

            string piece = PieceMap[pieceChange.movedPieceValue];
            piece = isTake && string.IsNullOrEmpty(piece) ? FileMap[pieceChange.fromMove.col] : piece;

            string row = RankMap[rowIndex];
            string col = FileMap[colIndex];

            return piece + isTakeString + col + row + isCheckString + isCheckmateString;
        }

        public static string convertCoordinateToFEN(string coordinates)
        {
			int row = coordinates[0] - '0';
			int col = coordinates[1] - '0';

			char rowChar = (char)('8' - row+1);
			char colChar = (char)('a' + col);

			return $"{colChar}{rowChar}";
		}

        private static string getCastlingFEN(PieceChange pieceChange)
        {
            if (pieceChange.fromMove.col > pieceChange.toMove.col) // moved left (long castle)
            {
                return "O-O-O";
            }
            return "O-O";
        }


        public static string convertBoardToFEN(Piece[,] board, bool isWhiteTurn)
        {
            StringBuilder sb = new StringBuilder();

            for (int rank = 0; rank < 8; rank++)
            {
                int emptySquareCount = 0;

                for (int file = 0; file < 8; file++)
                {
                    Piece piece = board[rank, file];

                    if (piece is EmptyPiece)
                    {
                        emptySquareCount++;
                    }
                    else
                    {
                        if (emptySquareCount > 0)
                        {
                            sb.Append(emptySquareCount);
                            emptySquareCount = 0;
                        }

                        sb.Append(piece.getFENRepresentation());
                    }
                }

                if (emptySquareCount > 0)
                {
                    sb.Append(emptySquareCount);
                }

                if (rank < 7)
                {
                    sb.Append('/');
                }
            }

            // Add castling availability
            sb.Append(' ');
            sb.Append(isWhiteTurn == true ? 'w' : 'b');

            sb.Append(' ');
            sb.Append(Castling.getCastlingAvailability(board));

			sb.Append(' ');
            sb.Append(EnPassant.getEnPassantAvailability(board));

			return sb.ToString();
        }

        public static Piece[,] convertFENToboard(string boardString)
        {
            Piece[,] board = new Piece[8, 8];

            string[] fenParts = boardString.Split(' ');

            string[] ranks = boardString.Split(new char[] { '/', ' ' },
                                 StringSplitOptions.RemoveEmptyEntries);

            for (int rank = 7; rank >= 0; rank--)
            {
                string rowFEN = expandChessNotation(ranks[rank]);

                for (int file = 7; file >= 0; file--)
                {
                    char c = rowFEN[file];
                    Piece piece = createPieceFromFEN(c, rank, file);
                    if (piece != null)
                    {
                        board[rank, file] = piece;
                    }
                }
            }

            Castling.setCastlingAvailability(board, fenParts[2]);

            return board;
        }

        private static string expandChessNotation(string input)
        {
            // this is not the best way to do
            string result = "";
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    int count = int.Parse(c.ToString());  // Convert the character to an integer
                    result += new string('0', count);     // Append 'count' number of zeros to the result
                }
                else
                {
                    result += c;  // Append non-digit characters directly
                }
            }
            return result;
        }

        private static Piece createPieceFromFEN(char fen, int row, int col)
        {
            Pieces.Color color = char.IsUpper(fen) ? Pieces.Color.White : Pieces.Color.Black;
            bool isWhite = color == Pieces.Color.White;

            switch (char.ToLower(fen))
            {
                case 'r':
                    return new Rook(color, isWhite ? PieceConstants.whiteRookValue : PieceConstants.blackRookValue, isWhite ? "Images/wR.svg" : "Images/bR.svg", $"{row}{col}", ableToCastling: false);
                case 'n':
                    return new Knight(color, isWhite ? PieceConstants.whiteKnightValue : PieceConstants.blackKnightValue, isWhite ? "Images/wN.svg" : "Images/bN.svg", $"{row}{col}");
                case 'b':
                    return new Bishop(color, isWhite ? PieceConstants.whiteBishopValue : PieceConstants.blackBishopValue, isWhite ? "Images/wB.svg" : "Images/bB.svg", $"{row}{col}");
                case 'q':
                    return new Queen(color, isWhite ? PieceConstants.whiteQueenValue : PieceConstants.blackQueenValue, isWhite ? "Images/wQ.svg" : "Images/bQ.svg", $"{row}{col}");
                case 'k':
                    return new King(color, isWhite ? PieceConstants.whiteKingValue : PieceConstants.blackKingValue, isWhite ? "Images/wK.svg" : "Images/bK.svg", $"{row}{col}", ableToCastling: false);
                case 'p':
                    return new Pawn(color, isWhite ? PieceConstants.whitePawnValue : PieceConstants.blackPawnValue, isWhite ? "Images/wP.svg" : "Images/bP.svg", $"{row}{col}");
                default:
                    return new EmptyPiece();
            }
        }

        public static string ConvertFenTo2DStringIndex(string fen)
        {
            int startCol = fen[0] - 'a';  // Converts 'e' to 4, for example
            int startRow = 8 - (fen[1] - '0');  // Converts '2' to 6 (since row index is reverse in chess arrays)
            int endCol = fen[2] - 'a';
            int endRow = 8 - (fen[3] - '0');

            string promotionValue = string.Empty;
            if (fen.Length > 4) 
            {
                promotionValue = fen[4].ToString();
            }

            return $"{startRow}{startCol},{endRow}{endCol},{promotionValue}";
        }
    }

}
