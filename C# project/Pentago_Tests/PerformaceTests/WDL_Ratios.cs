using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;
using EVFC = Pentago_Rules.EvaluationFunction;

    public static class WDL_Ratios
    {
        const int number_of_games_div4_pertest = 50;
    //assuming we use prev var with 2500;
    //total of 10.000 games per test... 
    //lets supose we have 40 tests of 1 second each -> 40.000 secs = 11.11... hours
    //lets supose we have 40 tests of 10 second each -> 400.000 secs = 111.11... hours = ~5 days
    //lets assume we have 40 tests of 1 minute each (=prev*60) ->  24.000.00 sec = 666,66... hours = ~28 days
    //lets assume we have 40 tests of 10 minutes each (=prev*600) -> 24.000.000 sec = 6666,66... hours = ~278 days
    static Pentago_GameBoard[] testBoards;
        public static void RUN()
        {
            testBoards = new Pentago_GameBoard[number_of_games_div4_pertest * 2];

            int i = 0;
            for (; i < number_of_games_div4_pertest; ++i)
            {
                int numPieces = GenerateRandomBoard.GetRandomNumber(1, 12);
                GenerateRandomBoard rndBoard = new GenerateRandomBoard(numPieces, true);
                rndBoard.generateNewBoard();
                testBoards[i] = rndBoard.Pentago_gb;
            }

            int aux = number_of_games_div4_pertest * 2;
            for (; i < aux; ++i)
            {
                int numPieces = GenerateRandomBoard.GetRandomNumber(1, 12);
                GenerateRandomBoard rndBoard = new GenerateRandomBoard(numPieces, false);
                rndBoard.generateNewBoard();
                testBoards[i] = rndBoard.Pentago_gb;
            }



        //using AUTO depth  - - - - - - - - - - - 
        //minimum is 4, gets bigger when there are more pieces on the board

        //control heuristic
        test00();

            System.Threading.Thread.Sleep(1000);
            //heuristic1 vs control
            test01_0();
            test01_1();
            test02_0();
            test02_1();
            test03_0();
            test03_1();

            System.Threading.Thread.Sleep(1000);
            //heuristic1.2 relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //heuristicA relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //heuristicAhacked relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //1.2 relaxed vs A

            System.Threading.Thread.Sleep(1000);
            //1.2 vs 1.2

            System.Threading.Thread.Sleep(1000);
            //A vs A

            System.Threading.Thread.Sleep(1000);
            //A vs Ahacked



            //using depth 6 - - - - - - - - - - - 
            //NOT SO SURE ABOUT USING 6

            //heuristic1 vs control

            System.Threading.Thread.Sleep(1000);
            //heuristic1.2 relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //heuristicA relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //heuristicAhacked relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //1.2 relaxed vs A

            System.Threading.Thread.Sleep(1000);
            //1.2 vs 1.2

            System.Threading.Thread.Sleep(1000);
            //A vs A

            System.Threading.Thread.Sleep(1000);
            //A vs Ahacked


        }

        static void test_aux(MINMAX M_W, MINMAX M_B,bool testFirst)
        {
            System.Console.WriteLine("===========================");
            M_W.printConfigs();
            System.Console.WriteLine(" --- --- ---VS --- --- ---");
            M_B.printConfigs();

            System.Console.WriteLine("--> from start");
        UnitTesting.testHeuristic(number_of_games_div4_pertest * 2, M_W, M_B, testFirst, true, false, false);
            System.Console.WriteLine("--> from mid game");
        UnitTesting.testHeuristic(testBoards, M_W, M_B, testFirst, true, false, false);

    }

        static void test00()
        {
            Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_WHITES, false);
            MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
            Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_BLACKS, false);
            MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

            test_aux(minmax_w, minmax_b, true);
        }

        #region tests heuristic 1

        static void test01_0()
        {
            Pentago_Rules wrules = new Pentago_Rules(EVFC.heuristic1,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_WHITES, false);
            MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
            Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_BLACKS, false);
            MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

            test_aux(minmax_w, minmax_b,true);
        }
        static void test01_1()
        {
            Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_WHITES, false);
            MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
            Pentago_Rules brules = new Pentago_Rules(EVFC.heuristic1,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_BLACKS, false);
            MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

            test_aux(minmax_w, minmax_b, false);
        }

        static void test02_0()
        {
            Pentago_Rules wrules = new Pentago_Rules(EVFC.heuristic1,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_WHITES, false);
            wrules.setHeuristic1Bias(0.0f);
            MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
            Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_BLACKS, false);
            MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

            test_aux(minmax_w, minmax_b, true);
        }
        static void test02_1()
        {
            Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_WHITES, false);
            MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
            Pentago_Rules brules = new Pentago_Rules(EVFC.heuristic1,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_BLACKS, false);
            brules.setHeuristic1Bias(0.0f);
            MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

            test_aux(minmax_w, minmax_b, false);
        }

        static void test03_0()
        {
            Pentago_Rules wrules = new Pentago_Rules(EVFC.heuristic1,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_WHITES, false);
            MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
            Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_BLACKS, false);
            wrules.setHeuristic1Bias(1.0f);
            MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

            test_aux(minmax_w, minmax_b, true);
        }
        static void test03_1()
        {
            Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_WHITES, false);
            MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
            Pentago_Rules brules = new Pentago_Rules(EVFC.heuristic1,
                    Pentago_Rules.NextStatesFunction.check_symmetries,
                    Pentago_Rules.IA_PIECES_BLACKS, false);
            brules.setHeuristic1Bias(1.0f);
            MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

            test_aux(minmax_w, minmax_b, false);
        }

        #endregion



    }

