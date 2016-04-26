using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;

static partial class UnitTesting
{
    static public void testHeuristicA()
    {
        initialize_test_gameboards();
        Pentago_Rules wrules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.controlHeuristic,
            Pentago_Rules.NextStatesFunction.all_states,
            Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX alpha_beta_test_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 6);
        Pentago_Rules brules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.heuristicA,
            Pentago_Rules.NextStatesFunction.all_states,
            Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX alpha_beta_test_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 6);
        bool? player;
        int rounds = 0;
        while (!emptyBoard.game_ended(out player))
        {
            applyPrintMoves(alpha_beta_test_w.run(emptyBoard), emptyBoard);
            Console.WriteLine("|-|-|-|-|-|-|-|-|");
            applyPrintMoves(alpha_beta_test_b.run(emptyBoard), emptyBoard);
            Console.WriteLine("|-|-|-|-|-|-|-|-|");
            rounds++;
        }
        if (player == null)
            Console.WriteLine("Game ends in tie.");
        else if (player == Pentago_Rules.IA_PIECES_WHITES)
            Console.WriteLine("White wins in " + rounds + " rounds.");
        else
            Console.WriteLine("Black wins in " + rounds + " rounds.");
      /*        initialize_test_gameboards();
                Pentago_Rules rules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.heuristicA,
                    Pentago_Rules.NextStatesFunction.all_states,
                    Pentago_Rules.IA_PIECES_WHITES, false);

                Console.WriteLine(rules.heuristicA(boardHeuristicA));*/
    }

}
