using StockFishBlazorChess.Data;
using StockFishBlazorChess.Pieces;
using StockFishBlazorChess.Rules;
using MudBlazor;
using MudBlazor.Extensions;
using Color = StockFishBlazorChess.Pieces.Color;

namespace StockFishBlazorChess.Game
{
    public class Chessboard
    {
        // Define the chessboard table
        public Piece[,] board = new Piece[8, 8];

        public Chessboard()
        {
            // Initialize the chessboard with the starting position
            initialize();
        }

        // Initialize the chessboard with the starting position
        private void initialize()
        {
            // Place the white pieces
            board[7, 0] = new Rook(Color.White, PieceConstants.whiteRookValue, "Images/wR.svg", "70");
            board[7, 1] = new Knight(Color.White, PieceConstants.whiteKnightValue, "Images/wN.svg", "71");
            board[7, 2] = new Bishop(Color.White, PieceConstants.whiteBishopValue, "Images/wB.svg", "72");
            board[7, 3] = new Queen(Color.White, PieceConstants.whiteQueenValue, "Images/wQ.svg", "73");
            board[7, 4] = new King(Color.White, PieceConstants.whiteKingValue, "Images/wK.svg", "74");
            board[7, 5] = new Bishop(Color.White, PieceConstants.whiteBishopValue, "Images/wB.svg", "75");
            board[7, 6] = new Knight(Color.White, PieceConstants.whiteKnightValue, "Images/wN.svg", "76");
            board[7, 7] = new Rook(Color.White, PieceConstants.whiteRookValue, "Images/wR.svg", "77");
            for (int i = 0; i < 8; i++)
            {
                board[6, i] = new Pawn(Color.White, PieceConstants.whitePawnValue, "Images/wP.svg", "6" + i.ToString());
            }

            // Place the black pieces
            board[0, 0] = new Rook(Color.Black, PieceConstants.blackRookValue, "Images/bR.svg", "00");
            board[0, 1] = new Knight(Color.Black, PieceConstants.blackKnightValue, "Images/bN.svg", "01");
            board[0, 2] = new Bishop(Color.Black, PieceConstants.blackBishopValue, "Images/bB.svg", "02");
            board[0, 3] = new Queen(Color.Black, PieceConstants.blackQueenValue, "Images/bQ.svg", "03");
            board[0, 4] = new King(Color.Black, PieceConstants.blackKingValue, "Images/bK.svg", "04");
            board[0, 5] = new Bishop(Color.Black, PieceConstants.blackBishopValue, "Images/bB.svg", "05");
            board[0, 6] = new Knight(Color.Black, PieceConstants.blackKnightValue, "Images/bN.svg", "06");
            board[0, 7] = new Rook(Color.Black, PieceConstants.blackRookValue, "Images/bR.svg", "07");
            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn(Color.Black, PieceConstants.blackPawnValue, "Images/bP.svg", "1" + i.ToString());
            }

            // Fill the remaining cells with empty pieces
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = new EmptyPiece();
                }
            }
        }

        public void setPiece(int row, int col, Piece piece, IEnumerable<Piece> list, bool isCastling, bool isEnPassant)
        {
            if (isCastling)
            {
                performCastling(row, col);
            }
            else if (isEnPassant)
            {
                EnPassant.performEnPassant(board, piece, row, col, list);
            }

            int oldRow = int.Parse(piece.Position![..1]);
            int oldCol = int.Parse(piece.Position!.Substring(1, 1));

            piece.setPosition($"{row}{col}");

            board[oldRow, oldCol] = new EmptyPiece();
            board[row, col] = piece;
        }

        private void performCastling(int row, int col)
        {
            Castling.castling(this, $"{row}{col}");
        }
    }
}
