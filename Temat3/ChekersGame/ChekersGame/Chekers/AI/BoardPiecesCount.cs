using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChekersGame.Chekers.AI;

internal class BoardPiecesCount : IBoardEvaluator
{
    public const int PAWN_SCORE = 1;
    public const int KING_SCORE = 3;
    public double Evaluate(Board board, Player player)
    {
        int playerScore = 0;
        int totalScore = 0;

        var allPieces = board.GetPieces();

        foreach (var piece in allPieces)
        {
            if (piece.PieceColor == player.PlayerPieceColor)
                playerScore += piece.IsKing ? KING_SCORE : PAWN_SCORE;

            totalScore += piece.IsKing ? KING_SCORE : PAWN_SCORE;
        }

        return (double)playerScore / totalScore;
    }
}

