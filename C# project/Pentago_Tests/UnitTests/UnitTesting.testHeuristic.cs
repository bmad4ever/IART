﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;

static partial class UnitTesting
{
    public const bool testFirst = true;
    public const bool testSecond = false;
    static public void testHeuristic(Pentago_GameBoard[] boards, MINMAX M_W, MINMAX M_B, bool testHeuristic, bool print_onend_only = false, bool printTies = false, bool printLosses = false)
    {
        int numberTests = boards.Length;

        int black_wins = 0;
        int black_losses = 0;
        int ties = 0;
        int totalrounds = 0;
        for (int i = 0; i < numberTests ; ++i)
        {
            List<Pentago_Move> allMoves = new List<Pentago_Move>();
            Pentago_GameBoard board = boards[i].Clone();
            bool? player;
            while (!board.game_ended(out player))
            {
                if(board.get_player_turn()==Pentago_GameBoard.whites_turn) applyMoves(M_W.run(board), board, ref allMoves);
                else applyMoves(M_B.run(board), board, ref allMoves);
                totalrounds++;
            }
            if (player == null)
            {
                if (printTies)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, board);
                }
                ties++;
            }
            else if (player == Pentago_Rules.IA_PIECES_BLACKS)
            {
                if (testHeuristic == testFirst && printLosses)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, board);
                }
                black_wins++;
            }
            else
            {
                if (testHeuristic == testSecond && printLosses)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, board);
                }
                black_losses++;
            }

            if (!print_onend_only)
            {
                System.Console.WriteLine("RESULT");
                if (testHeuristic == testFirst) 
                    Console.WriteLine("it: " + (i + 1) + " - wins: " + black_losses + ", losses: " + black_wins + ", ties: " + ties);
                else
                    Console.WriteLine("it: " + (i + 1) + " - wins: " + black_wins + ", losses: " + black_losses + ", ties: " + ties);
            }
        }

        if (print_onend_only)
        {
            if (testHeuristic == testFirst)
                Console.WriteLine(numberTests + " - wins: " + black_losses + ", losses: " + black_wins + ", ties: " + ties + ", avg rounds: " + (totalrounds / numberTests));
            else
                Console.WriteLine(numberTests + " - wins: " + black_wins + ", losses: " + black_losses + ", ties: " + ties + ", avg rounds: " + (totalrounds / numberTests));
        }

        System.Console.WriteLine();
    }

    static public void testHeuristic(int numberTests , MINMAX M_W, MINMAX M_B, bool testHeuristic, bool print_onend_only = false, bool printTies = false, bool printLosses = false)
    {

        int black_wins = 0;
        int black_losses = 0;
        int ties = 0;
        int totalrounds = 0;
        initialize_test_gameboards();
        for (int i = 0; i < numberTests; ++i)
        {
            List<Pentago_Move> allMoves = new List<Pentago_Move>();
            Pentago_GameBoard board = emptyBoard.Clone();
            bool? player;
            while (!board.game_ended(out player))
            {
                if (board.get_player_turn() == Pentago_GameBoard.whites_turn) applyMoves(M_W.run(board), board, ref allMoves);
                else applyMoves(M_B.run(board), board, ref allMoves);
                totalrounds++;
            }
            if (player == null)
            {
                if (printTies)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, board);
                }
                ties++;
            }
            else if (player == Pentago_Rules.IA_PIECES_BLACKS)
            {
                if (testHeuristic == testFirst && printLosses)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, board);
                }
                black_wins++;
            }
            else
            {
                if (testHeuristic == testSecond && printLosses)
                {
                    initialize_test_gameboards();
                    printAllMoves(allMoves, board);
                }
                black_losses++;
            }

            if (!print_onend_only)
            {
                System.Console.WriteLine("RESULT");
                if (testHeuristic == testFirst)
                    Console.WriteLine("it: " + (i + 1) + " - wins: " + black_losses + ", losses: " + black_wins + ", ties: " + ties);
                else
                    Console.WriteLine("it: " + (i + 1) + " - wins: " + black_wins + ", losses: " + black_losses + ", ties: " + ties);
            }
        }

        if (print_onend_only)
        {
            if (testHeuristic == testFirst)
                Console.WriteLine(numberTests + " - wins: " + black_losses + ", losses: " + black_wins + ", ties: " + ties + "avg rounds: " + (totalrounds / numberTests));
            else
                Console.WriteLine(numberTests + " - wins: " + black_wins + ", losses: " + black_losses + ", ties: " + ties + "avg rounds: " + (totalrounds / numberTests));
        }

        System.Console.WriteLine();
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