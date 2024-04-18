using StockFishBlazorChess.Game;
using StockFishBlazorChess.Pieces;
using StockFishBlazorChess.Services;

namespace StockFishBlazorChess.Rules
{
    public class Promotion
    {
        public static bool isPromotion(Piece piece, int newRow)
        {
            return piece is Pawn && (newRow is 7 or 0);
        }

        public static void performPromotion(ChessGameService chessGameService, Piece piece, char pieceChar)
        {
            Piece promotedPawn = chessGameService.piecesOnBoard.First(x => x == piece);
            (int row, int col) = piece.getPositionTuple();
            switch (pieceChar)
            {
                case 'q':
                    chessGameService.chessBoard.board[row, col] = new Queen(piece.Color, piece.Color == Color.White ? PieceConstants.whiteQueenValue : PieceConstants.blackQueenValue, $"Images/{getColorChar(piece.Color)}Q.svg", new string(piece.Position));
                    break;
                case 'r':
                    chessGameService.chessBoard.board[row, col] = new Rook(piece.Color, piece.Color == Color.White ? PieceConstants.whiteRookValue : PieceConstants.blackRookValue, $"Images/{getColorChar(piece.Color)}R.svg", new string(piece.Position));
                    break;
                case 'b':
                    chessGameService.chessBoard.board[row, col] = new Bishop(piece.Color, piece.Color == Color.White ? PieceConstants.whiteBishopValue : PieceConstants.blackBishopValue, $"Images/{getColorChar(piece.Color)}B.svg", new string(piece.Position));
                    break;
                case 'n':
                    chessGameService.chessBoard.board[row, col] = new Knight(piece.Color, piece.Color == Color.White ? PieceConstants.whiteKnightValue : PieceConstants.blackKnightValue, $"Images/{getColorChar(piece.Color)}N.svg", new string(piece.Position));
                    break;
            }
            //promotedPawn.Icon = chessGameService.chessBoard.board[row, col].Icon;
            promotedPawn = chessGameService.chessBoard.board[row, col];
            //chessGameService.piecesOnBoard = chessGameService.chessBoard.board.Cast<Piece>().ToList();
        }

        private static char getColorChar(Color color)
        {
            return color == Color.White ? 'w' : 'b';
        }
    }
}
