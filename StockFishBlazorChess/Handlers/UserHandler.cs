using StockFishBlazorChess.Pieces;
using MudBlazor;
using MudBlazor.Extensions;
using System.Text;
using StockFishBlazorChess.Data;

namespace StockFishBlazorChess.Handlers
{
    public class UserHandler : IUserHandler
    {
        private Dictionary<string, List<string>> connectedPlayers = new Dictionary<string, List<string>>();
        private Dictionary<string, MatchInfo> matchInfos = new Dictionary<string, MatchInfo>();

        public List<string> getConnectedPlayerKeys()
        {
            return connectedPlayers.Where(cp => cp.Value.Count == 1).Select(cp => cp.Key).ToList();
        }

        public void setMatchInfoMoves(string gameName, List<PieceChange> pieceChanges, bool isWhiteTurn)
        {
            matchInfos[gameName].pieceChanges = new List<PieceChange>(pieceChanges);
            matchInfos[gameName].isWhiteTurn = isWhiteTurn;
        }

        public void setMatchInfoBoard(string gameName, Piece[,] board)
        {
            string boardString = convertBoardToString(board);
            matchInfos[gameName].boardInfo = boardString;
        }

        public Piece[,] getMatchInfoBoard(string gameName)
        {
            return convertStringToBoard(matchInfos[gameName].boardInfo);
        }

        public List<PieceChange> getMatchInfoMoves(string gameName)
        {
            return matchInfos[gameName].pieceChanges;
        }

        private string convertBoardToString(Piece[,] board)
        {
            StringBuilder sb = new StringBuilder();

            for (int rank = 0; rank < 8; rank++)
            {
                int emptySquareCount = 0;

                for (int file = 0; file < 8; file++)
                {
                    Piece piece = board[rank, file];

                    if (piece == null)
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

                if (rank < 8)
                {
                    sb.Append('/');
                }
            }

            // Add castling availability
            sb.Append(' ');
            sb.Append(Castling.getCastlingAvailability(board));

            return sb.ToString();
        }

        private Piece[,] convertStringToBoard(string boardString)
        {
            Piece[,] board = new Piece[8, 8];

            string[] fenParts = boardString.Split(' ');

            string[] ranks = boardString.Split('/');

            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 7; file >= 0; file--)
                {
                    char c = ranks[rank][file];

                    Piece piece = createPieceFromFEN(c, rank, file);
                    if (piece != null)
                    {
                        board[rank, file] = piece;
                    }
                }
            }

            Castling.setCastlingAvailability(board, fenParts[1]);

            return board;
        }

        private Piece createPieceFromFEN(char fen, int row, int col)
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

        public Dictionary<string, List<string>> getConnectedPlayers()
        {
            return connectedPlayers;
        }

        public Dictionary<string, MatchInfo> getMatchInfos()
        {
            return matchInfos;
        }
    }
}