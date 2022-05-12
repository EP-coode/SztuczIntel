using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChekersGame.MiniMax;
using ChekersGame.Chekers;

namespace ChekersGame.Chekers.AI;

public class BoardPiecesCount : IEvaluator<Board, Move, PieceColor>
{
    public const int PAWN_SCORE = 1;
    public const int KING_SCORE = 3;
    public double EvaluateFromPlayerPErspective(IMiniMaxGame<Board, Move, PieceColor> game, PieceColor movingPlayer)
    {
        int playerScore = 0;
        int totalScore = 0;

        var allPieces = game.GetState().GetPieces();

        foreach (var piece in allPieces)
        {
            if (piece.PieceColor == movingPlayer)
                playerScore += piece.IsKing ? KING_SCORE : PAWN_SCORE;

            totalScore += piece.IsKing ? KING_SCORE : PAWN_SCORE;
        }

        return 1.0d * playerScore / totalScore;
    }
}

