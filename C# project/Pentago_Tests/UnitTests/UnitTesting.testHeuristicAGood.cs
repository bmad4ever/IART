using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;

static partial class UnitTesting
{
    static public void testHeuristicAGood()
    {
        Pentago_Rules wrules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.controlHeuristic,
            Pentago_Rules.NextStatesFunction.all_states,
            Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX alpha_beta_test_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 4);
        Pentago_Rules brules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.heuristicA,
            Pentago_Rules.NextStatesFunction.all_states,
            Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX alpha_beta_test_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 4);
        int black_wins = 0;
        int ties = 0;
        for (int i = 1; i < 101; i++)
        {
            initialize_test_gameboards();
            bool? player;
            int rounds = 0;
            while (!emptyBoard.game_ended(out player))
            {
                applyMoves(alpha_beta_test_w.run(emptyBoard), emptyBoard);
                applyMoves(alpha_beta_test_b.run(emptyBoard), emptyBoard);
                rounds++;
            }
            if (player == null) ties++;
            else if (player == Pentago_Rules.IA_PIECES_BLACKS) black_wins++;
            Console.WriteLine(i + " black wins " + black_wins + " ties " + ties);
        }
    }

    static void applyMoves(Pentago_Move[] moves, Pentago_GameBoard board)
    {
        foreach (Pentago_Move move in moves)
            move.apply_move2board(board);
    }

}
