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
        alpha_beta_test.debugBoard = (o) => { o.print_board(); };
        Pentago_Move[] moves = alpha_beta_test.run(boardAlphaBeta);
        foreach (Pentago_Move move in moves)
        {
            move.apply_move2board(boardAlphaBeta);
            boardAlphaBeta.print_board();
        }
        Console.WriteLine("Place a piece: square,x,y     square E[0,3]      x,y E[0,2]");
        int[] input = Console.ReadLine().Split(',').Select<string, int>(o => Convert.ToInt32(o)).ToArray();
        Pentago_Move pm = new Pentago_Move(input[0], input[1], input[2]);
        pm.apply_move2board(boardAlphaBeta);
        Console.WriteLine("Rotate a square: square,dir     square E[0,3]      dir E[0-anti,1-clock]");
        input = Console.ReadLine().Split(',').Select<string, int>(o => Convert.ToInt32(o)).ToArray();
        pm = new Pentago_Move(input[0], input[1] == 0 ? Pentago_Move.rotate_anticlockwise : Pentago_Move.rotate_clockwise);
        pm.apply_move2board(boardAlphaBeta);
        boardAlphaBeta.print_board();
        moves = alpha_beta_test.run(boardAlphaBeta);
        foreach (Pentago_Move move in moves)
        {
            move.apply_move2board(boardAlphaBeta);
            boardAlphaBeta.print_board();
        }

    }
}
