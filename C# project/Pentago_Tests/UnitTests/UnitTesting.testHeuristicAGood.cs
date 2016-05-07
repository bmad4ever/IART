using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;

static partial class UnitTesting
{
    public const bool testFirst = true;
    public const bool testSecond = false; 
    static public void testHeuristicAGood(int numberTests, Pentago_Rules.EvaluationFunction ef1, int depth1, Pentago_Rules.EvaluationFunction ef2, int depth2, bool testHeuristic, bool printTies = false, bool printLosses = false)
    {
        Pentago_Rules wrules = new Pentago_Rules(ef1,
            Pentago_Rules.NextStatesFunction.check_symmetries,
            Pentago_Rules.IA_PIECES_WHITES, true/*, -100*/);
        MINMAX alpha_beta_test_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, depth1);
        Pentago_Rules brules = new Pentago_Rules(ef2,
            Pentago_Rules.NextStatesFunction.check_symmetries,
            Pentago_Rules.IA_PIECES_BLACKS, true);
        MINMAX alpha_beta_test_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, depth2);
        int black_wins = 0;
        int black_losses = 0;
        int ties = 0;
        for (int i = 1; i < numberTests + 1; i++)
        {
            List<Pentago_Move> allMoves = new List<Pentago_Move>();
            initialize_test_gameboards();
            bool? player;
            int rounds = 0;
            while (!emptyBoard.game_ended(out player))
            {
                applyMoves(alpha_beta_test_w.run(emptyBoard), emptyBoard, ref allMoves);
                applyMoves(alpha_beta_test_b.run(emptyBoard), emptyBoard, ref allMoves);
                rounds++;
            }
            if (player == null)
            {
                if (printTies)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, emptyBoard);
                }
                ties++;
            }
            else if (player == Pentago_Rules.IA_PIECES_BLACKS)
            {
                if (testHeuristic == testFirst && printLosses)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, emptyBoard);
                }
                black_wins++;
            }
            else
            {
                if (testHeuristic == testSecond && printLosses)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, emptyBoard);
                }
                black_losses++;
            }
            if (testHeuristic == testFirst)
                Console.WriteLine(i + " - wins: " + black_losses + ", losses: " + black_wins + ", ties: " + ties);
            else
                Console.WriteLine(i + " - wins: " + black_wins + ", losses: " + black_losses + ", ties: " + ties);
        }
    }

    static void applyMoves(Pentago_Move[] moves, Pentago_GameBoard board, ref List<Pentago_Move> allMoves)
    {
        foreach (Pentago_Move move in moves)
        {
            move.apply_move2board(board);
            allMoves.Add(move);
        }
    }

    static void printAllMoves(List<Pentago_Move> allMoves, Pentago_GameBoard board)
    {
        int i = 0;
        foreach (Pentago_Move move in allMoves)
        {
            if (board.get_player_turn() == Pentago_GameBoard.whites_turn) Console.Write("White ");
            else Console.Write("Black ");
            if (board.get_turn_state() == Pentago_GameBoard.turn_state_rotate)
                Console.WriteLine("rotate");
            else Console.WriteLine("place");
            move.apply_move2board(board);
            board.print_board();
            i++;
            if (i % 2 == 0) Console.WriteLine("|-|-|-|-|-|-|-|-|");
        }
    }

}
