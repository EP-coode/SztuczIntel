using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChekersGame.MiniMax;

namespace ChekersGame.Chekers.AI;

public class BoardZonesEvaluatorcs : IEvaluator<Board, Move, PieceColor>
{
    public const int ZONE_1_SCORE = 1;
    public const int ZONE_2_SCORE = 2;
    public const int ZONE_3_SCORE = 3;

    public double EvaluateFromPlayerPErspective(IMiniMaxGame<Board, Move, PieceColor> game, PieceColor player)
    {
        var playerPieces = game.GetState().GetPieces(player);
        int totalScore = 0;
        int maxScore = ZONE_3_SCORE * playerPieces.Count;

        foreach (var piece in playerPieces)
        {
            totalScore += GetPieceScore(piece.Row, piece.Col);
        }

        return 1d * totalScore / maxScore;
    }

    private int GetPieceScore(int row, int col)
    {
        if (row == 0 || col == 0)
            return ZONE_1_SCORE;

        if (row == Board.BOARD_SIZE - 1 || col == Board.BOARD_SIZE - 1)
            return ZONE_1_SCORE;

        if (row == 1 || col == 1)
            return ZONE_2_SCORE;

        if (row == Board.BOARD_SIZE - 2 || col == Board.BOARD_SIZE - 2)
            return ZONE_2_SCORE;

        return ZONE_3_SCORE;
    }
}

