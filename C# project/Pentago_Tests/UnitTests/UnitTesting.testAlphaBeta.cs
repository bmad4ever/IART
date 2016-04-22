using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;
static partial class UnitTesting
{
    static public void testAlphaBeta()
    {

        Pentago_Rules prules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.controlHeuristic,
            Pentago_Rules.NextStatesFunction.all_states,
            Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX alpha_beta_test = new MINMAX(MINMAX.VERSION.alphabeta, prules, 6);
        initialize_test_gameboards();
        boardAlphaBeta.print_board();
        Pentago_Move[] moves = alpha_beta_test.run(boardAlphaBeta);
        foreach (Pentago_Move move in moves)
        {
            move.apply_move2board(boardAlphaBeta);
            boardAlphaBeta.print_board();
        }


    }
}
