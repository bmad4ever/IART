using System;
using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;


static class PerformanceTests
{
    static public void testPerformnace(int numOfBorads)
    {
        Pentago_GameBoard[] testBoardsWhites = new Pentago_GameBoard[numOfBorads];
        Pentago_GameBoard[] testBoardsBlacks = new Pentago_GameBoard[numOfBorads];

        for (int i = 0; i < numOfBorads; i++)
        {
            int numPieces = GenerateRandomBoard.GetRandomNumber(0, 17);
            GenerateRandomBoard rndBoard = new GenerateRandomBoard(numPieces, true);
            rndBoard.generateNewBoard();
            testBoardsWhites[i] = rndBoard.Pentago_gb;
        }

        for (int i = 0; i < numOfBorads; i++)
        {
            int numPieces = GenerateRandomBoard.GetRandomNumber(0, 17);
            GenerateRandomBoard rndBoard = new GenerateRandomBoard(numPieces, false);
            rndBoard.generateNewBoard();
            testBoardsBlacks[i] = rndBoard.Pentago_gb;
        }

        Pentago_Rules wrules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.all_states,
                    Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX test_w = new MINMAX(MINMAX.VERSION.minmax, wrules, 6);
        Pentago_Rules brules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.controlHeuristic,
            Pentago_Rules.NextStatesFunction.all_states,
            Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX test_b = new MINMAX(MINMAX.VERSION.minmax, brules, 6);

        TimeSpan test1 = Performance.PerformanceTimes(test_w, testBoardsWhites);
        TimeSpan test2 = Performance.PerformanceTimes(test_b, testBoardsBlacks);

        TimeSpan ts = test1.Add(test2);

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);

        Console.WriteLine("RunTime " + elapsedTime);
    }
}