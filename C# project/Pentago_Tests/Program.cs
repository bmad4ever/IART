class Program
{
    static void Main(string[] args)
    {
        //UnitTesting.testAlphaBeta();
        //UnitTesting.testHeuristicA();
        UnitTesting.testHeuristicAGood(100, Pentago_Rules.EvaluationFunction.heuristicA, 4, Pentago_Rules.EvaluationFunction.controlHeuristic, 6, UnitTesting.testFirst);
        //Pentago1P.play();

        //UnitTesting.testMinMax();
        //UnitTesting.test_auxiliar_methods();

        //PentagoPandora.BUILD_PANDORA();
        //Console.WriteLine("---ENDED---")
    }
}