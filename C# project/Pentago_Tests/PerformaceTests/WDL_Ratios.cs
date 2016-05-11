using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MINMAX = MinMax<Pentago_GameBoard, Pentago_Move>;
using EVFC = Pentago_Rules.EvaluationFunction;

public static class WDL_Ratios
{

    const bool test_with_control_auto = true;
    const bool test_with_others_auto = false;
    const bool test_with_control_d6 = false;
    const bool test_with_others_d6 = false;

    const int number_of_games_div4_pertest = 5;
    //assuming we use prev var with 2500;
    //total of 10.000 games per test... 
    //lets supose we have 40 tests of 1 second each -> 40.000 secs = 11.11... hours
    //lets supose we have 40 tests of 10 second each -> 400.000 secs = 111.11... hours = ~5 days
    //lets assume we have 40 tests of 1 minute each (=prev*60) ->  24.000.00 sec = 666,66... hours = ~28 days
    //lets assume we have 40 tests of 10 minutes each (=prev*600) -> 24.000.000 sec = 6666,66... hours = ~278 days

    static void printLatexTableHeader()
    {
        Console.WriteLine("\\begin{table}[]");
        Console.WriteLine("\\centering");
        Console.WriteLine("\\begin{tabular}{|c|c|c|c|c|c|c|c|c|c|c|c|c|c|}");
        Console.WriteLine("\\hline");
        Console.WriteLine(" &  & \\multicolumn{4}{c|}{Tabuleio Inicial Vazio} & \\multicolumn{4}{c|}{Tabuleiro Inicial Aleatorio} & \\multicolumn{4}{c|}{Total} \\\\ \\cline{3-14}");
        Console.WriteLine("\\multirow{-2}{*}{Joga com Brancas} & \\multirow{-2}{*}{Joga com Pretas} & {\\color[HTML]{00009B} Vit} & {\\color[HTML]{9A0000} Der} & {\\color[HTML]{009901} Emp} & Tur & {\\color[HTML]{00009B} Vit} & {\\color[HTML]{9A0000} Der} & {\\color[HTML]{009901} Emp} & Tur & {\\color[HTML]{00009B} Vit} & {\\color[HTML]{9A0000} Der} & {\\color[HTML]{009901} Emp} & Tur \\\\ \\hline");
    }

    static void printLatexTableFooter()
    {
        Console.WriteLine(
"\\end{tabular}"
+" \\caption{ My caption}"
+" \\label{ my - label}"
+" \\end{table}"
            );
    }

    static void printseparator()
    {
        Console.WriteLine();
        //Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        //Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
    }

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

        printLatexTableHeader();

        //using AUTO depth  - - - - - - - - - - - 
        //minimum is 4, gets bigger when there are more pieces on the board

        if (test_with_control_auto)
        {
            //control heuristic
            test00();
            printseparator();
            System.Threading.Thread.Sleep(1000);
            //heuristic1 vs control
            test01_0();
            test01_1();
            test02_0();
            test02_1();
            test03_0();
            test03_1();

            System.Threading.Thread.Sleep(1000);
            printseparator();
            //heuristic1.2 relaxed vs control
            test1dot2vsC_0(0);
            test1dot2vsC_0(1);
            test1dot2vsC_0(2);
            test1dot2vsC_0(3);
            test1dot2vsC_0(4);
            test1dot2vsC_0(5);
            test1dot2vsC_0(0);
            test1dot2vsC_1(1);
            test1dot2vsC_1(2);
            test1dot2vsC_1(3);
            test1dot2vsC_1(4);
            test1dot2vsC_1(5);

            System.Threading.Thread.Sleep(1000);
            printseparator();
            //heuristicA vs control
            testAvsC_0(EVFC.heuristicA, 1);
            testAvsC_0(EVFC.heuristicA, 2);
            testAvsC_0(EVFC.heuristicA, 3);
            testAvsC_0(EVFC.heuristicA, 4);
            testAvsC_1(EVFC.heuristicA, 1);
            testAvsC_1(EVFC.heuristicA, 2);
            testAvsC_1(EVFC.heuristicA, 3);
            testAvsC_1(EVFC.heuristicA, 4);

            System.Threading.Thread.Sleep(1000);
            printseparator();
            //heuristicAstar vs control
            testAvsC_0(EVFC.heuristicAstar, 1);
            testAvsC_0(EVFC.heuristicAstar, 2);
            testAvsC_0(EVFC.heuristicAstar, 3);
            testAvsC_0(EVFC.heuristicAstar, 4);
            testAvsC_1(EVFC.heuristicAstar, 1);
            testAvsC_1(EVFC.heuristicAstar, 2);
            testAvsC_1(EVFC.heuristicAstar, 3);
            testAvsC_1(EVFC.heuristicAstar, 4);

            System.Threading.Thread.Sleep(1000);
            printseparator();
            //heuristicAhacked relaxed vs control
            testAvsC_0(EVFC.heuristicAplusDiagonalHack, 1);
            testAvsC_0(EVFC.heuristicAplusDiagonalHack, 2);
            testAvsC_0(EVFC.heuristicAplusDiagonalHack, 3);
            testAvsC_0(EVFC.heuristicAplusDiagonalHack, 4);
            testAvsC_1(EVFC.heuristicAplusDiagonalHack, 1);
            testAvsC_1(EVFC.heuristicAplusDiagonalHack, 2);
            testAvsC_1(EVFC.heuristicAplusDiagonalHack, 3);
            testAvsC_1(EVFC.heuristicAplusDiagonalHack, 4);
        }

        if (test_with_others_auto)
        {
            System.Threading.Thread.Sleep(1000);
            printseparator();
            //1.2 relaxed vs A

            System.Threading.Thread.Sleep(1000);
            printseparator();
            //1.2 vs 1.2

            System.Threading.Thread.Sleep(1000);
            printseparator();
            //A vs A

            System.Threading.Thread.Sleep(1000);
            printseparator();
            //A vs Ahacked

        }


        //using depth 6 - - - - - - - - - - - 
        //NOT SO SURE ABOUT USING 6

        if (test_with_control_d6)
        {
            //heuristic1 vs control

            System.Threading.Thread.Sleep(1000);
            //heuristic1.2 relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //heuristicA relaxed vs control

            System.Threading.Thread.Sleep(1000);
            //heuristicAhacked relaxed vs control

        }

        if (test_with_others_d6)
        {

            System.Threading.Thread.Sleep(1000);
            //1.2 relaxed vs A

            System.Threading.Thread.Sleep(1000);
            //1.2 vs 1.2

            System.Threading.Thread.Sleep(1000);
            //A vs A

            System.Threading.Thread.Sleep(1000);
            //A vs Ahacked
        }

        printLatexTableFooter();
    }

    static void test_aux(MINMAX M_W, MINMAX M_B, bool testFirst)
    {
        /* System.Console.WriteLine();
         System.Console.WriteLine("=======================================================");
         System.Console.WriteLine();
         M_W.printConfigs();
         System.Console.WriteLine("       --- --- ---VS --- --- ---");
         M_B.printConfigs();
         System.Console.WriteLine(" ---  --- --- --- --- --- --- ---");
         System.Console.WriteLine("from start");
         int[] test_start = UnitTesting.testHeuristic(number_of_games_div4_pertest * 2, M_W, M_B, testFirst, true, false, false);
         System.Console.WriteLine("from mid game");
         int[] test_mid = UnitTesting.testHeuristic(testBoards, M_W, M_B, testFirst, true, false, false);

         Console.WriteLine("TOTAL "
             + "W = " + (test_start[0] + test_mid[0])
             + "   L = " + (test_start[1] + test_mid[1])
             + "   D = " + (test_start[2] + test_mid[2])
             )
             ;*/
        System.Console.WriteLine();

        int[] test_start = UnitTesting.testHeuristic(number_of_games_div4_pertest * 2, M_W, M_B, testFirst, true, false, false);
        int[] test_mid = UnitTesting.testHeuristic(testBoards, M_W, M_B, testFirst, true, false, false);

        Console.WriteLine("h1 name depth xpto & h2 name depth xpta & {\\color[HTML]{00009B} } & {\\color[HTML]{9A0000} } & {\\color[HTML]{009901} } &  & {\\color[HTML]{00009B} } & {\\color[HTML]{9A0000} } & {\\color[HTML]{009901} } &  & {\\color[HTML]{00009B} } & {\\color[HTML]{9A0000} } & {\\color[HTML]{009901} } &  \\\\ \\cline{1-2}");
        Console.WriteLine("h1 properties & h2properties & \\multirow{-2}{*}{{\\color[HTML]{00009B} " + test_start[0]*100/(number_of_games_div4_pertest*2) + "}} & \\multirow{-2}{*}{{\\color[HTML]{9A0000} "+ test_start[1] * 100 / (number_of_games_div4_pertest * 2) + "}} & \\multirow{-2}{*}{{\\color[HTML]{009901} "+ test_start[2] * 100 / (number_of_games_div4_pertest * 2) + "}} & \\multirow{-2}{*}{"+ test_start[3] + "} & \\multirow{-2}{*}{{\\color[HTML]{00009B} " + test_mid[0] * 100 / (number_of_games_div4_pertest * 2) + "}} & \\multirow{-2}{*}{{\\color[HTML]{9A0000} " + test_mid[1] * 100 / (number_of_games_div4_pertest * 2) + "}} & \\multirow{-2}{*}{{\\color[HTML]{009901} " + test_mid[2] * 100 / (number_of_games_div4_pertest * 2) + "}} & \\multirow{-2}{*}{" + test_mid[3] + "} & \\multirow{-2}{*}{{\\color[HTML]{00009B} " + (test_start[0] + test_mid[0])*100/(number_of_games_div4_pertest*4) + "}} & \\multirow{-2}{*}{{\\color[HTML]{9A0000} " + (test_start[1] + test_mid[1]) * 100 / (number_of_games_div4_pertest * 4) + "}} & \\multirow{-2}{*}{{\\color[HTML]{009901} " + (test_start[2] + test_mid[2]) * 100 / (number_of_games_div4_pertest * 4) + "}} & \\multirow{-2}{*}{" + (test_start[3] + test_mid[3])/2 + "} \\\\ \\hline");

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

#region with auto depth



    #region tests heuristic 1 vs control

    static void test01_0()
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.heuristic1,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, true);
    }
    static void test01_1()
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.heuristic1,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, false);
    }

    static void test02_0()
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.heuristic1,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        wrules.setHeuristic1Bias(0.0f);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, true);
    }
    static void test02_1()
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.heuristic1,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        brules.setHeuristic1Bias(0.0f);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, false);
    }

    static void test03_0()
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.heuristic1,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        wrules.setHeuristic1Bias(1.0f);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, true);
    }
    static void test03_1()
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.heuristic1,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        brules.setHeuristic1Bias(1.0f);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, false);
    }

    #endregion

    #region tests heuristic 1.2 vs control

    static void test1dot2vsC_0(int config)
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.heuristic1dot2,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        if (config == 1) wrules.setHeur12_AD_PO();
        if (config == 2) wrules.setHeur12_PD_AO();
        if (config == 3) wrules.setHeur12_UD();
        if (config == 4) wrules.setHeur12_UO();
        if (config == 5) wrules.setHeur12_P();
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, true);
    }

    static void test1dot2vsC_1(int config)
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.heuristic1dot2,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        if (config == 1) brules.setHeur12_AD_PO();
        if (config == 2) brules.setHeur12_PD_AO();
        if (config == 3) brules.setHeur12_UD();
        if (config == 4) brules.setHeur12_UO();
        if (config == 5) brules.setHeur12_P();
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, false);
    }

    #endregion

    #region heuristic A vs control

    static void testAvsC_0(EVFC heur, int config)
    {
        Pentago_Rules wrules = new Pentago_Rules(heur,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);

        if (config == 1) wrules.setA_setup1();
        if (config == 2) wrules.setA_setup2();
        if (config == 3) wrules.setA_setup3();

        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(EVFC.controlHeuristic,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);
        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, true);
    }

    static void testAvsC_1(EVFC heur, int config)
    {
        Pentago_Rules wrules = new Pentago_Rules(EVFC.controlHeuristic ,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_WHITES, false);
        MINMAX minmax_w = new MINMAX(MINMAX.VERSION.alphabeta, wrules, 0);
        Pentago_Rules brules = new Pentago_Rules(heur ,
                Pentago_Rules.NextStatesFunction.all_states,
                Pentago_Rules.IA_PIECES_BLACKS, false);

        if (config == 1) brules.setA_setup1();
        if (config == 2) brules.setA_setup2();
        if (config == 3) brules.setA_setup3();
        if (config == 4) brules.setA_setup4();

        MINMAX minmax_b = new MINMAX(MINMAX.VERSION.alphabeta, brules, 0);

        test_aux(minmax_w, minmax_b, false);
    }

    #endregion

    #region heuristic A star vs Control

    #endregion

    #region A hacked vs control

    #endregion

    #endregion with auto depth


}

