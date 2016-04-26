//#define DEBUG_HEURISTIC_A

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HOLESTATE = Pentago_GameBoard.hole_state;

public partial class Pentago_Rules
{
    public float heuristicA(Pentago_GameBoard gb)
    {
        if (gb.get_turn_state() == Pentago_GameBoard.turn_state_addpiece) return boardValue(gb.board);
        float result = gb.get_player_turn() == IA_PIECES ? float.NegativeInfinity : float.PositiveInfinity;
        Pentago_Move[] nplays = possible_plays(gb);
        float value;
        Pentago_GameBoard ngb;
        foreach (Pentago_Move move in nplays)
        {
            ngb = move.state_after_move(gb);
            value = boardValue(ngb.board);
            if ((gb.get_player_turn() == IA_PIECES && value > result) 
                || (gb.get_player_turn() != IA_PIECES && value < result))
                result = value;
        }
        return result;
    }

    public float boardValue(HOLESTATE[] gb)
    {
        int[] monica1 = { 5, 10, 15, 20, 25, 30 };
        int[] monica2 = { 0, 7, 14, 21, 28, 35 };
        int[][] monicas = { monica1, monica2 };                                                                 // diagonal score 3

        int[] middle1 = { 6, 7, 8, 9, 10, 11 };
        int[] middle2 = { 24, 25, 26, 27, 28, 29 };
        int[] middle3 = { 1, 7, 13, 19, 25, 31 };
        int[] middle4 = { 4, 10, 16, 22, 28, 34 };
        int[][] middles = { middle1, middle2, middle3, middle4 };                                               // middle line score 5

        int[] straight1 = { 0, 1, 2, 3, 4, 5 };
        int[] straight2 = { 12, 13, 14, 15, 16, 17 };
        int[] straight3 = { 18, 19, 20, 21, 22, 23 };
        int[] straight4 = { 30, 31, 32, 33, 34, 35 };
        int[] straight5 = { 0, 6, 12, 18, 24, 30 };
        int[] straight6 = { 2, 8, 14, 20, 26, 32 };
        int[] straight7 = { 3, 9, 15, 21, 27, 33 };
        int[] straight8 = { 5, 11, 17, 23, 29, 35 };
        int[][] straights = { straight1, straight2, straight3, straight4, straight5, straight6, straight7, straight8 };    // border line score 7

        int[] triple1 = { 1, 8, 15, 22, 29 };
        int[] triple2 = { 6, 13, 20, 27, 34 };
        int[] triple3 = { 4, 9, 14, 19, 24 };
        int[] triple4 = { 11, 16, 21, 26, 31 };

        int[][] triples = { triple1, triple2, triple3, triple4 };                                               // short diagonal score 9

        float result = 0;
#if DEBUG_HEURISTIC_A
        Console.WriteLine("monica");
#endif
        int whiteCount;
        int blackCount;
        foreach (int[] monica in monicas)
        {
            countLine(gb, monica, out whiteCount, out blackCount);
            result += (float)(Math.Pow(1.3, whiteCount) - Math.Pow(1.3, blackCount));
        }
#if DEBUG_HEURISTIC_A
        Console.WriteLine("middle");
#endif
        foreach (int[] middle in middles)
        {
            countLine(gb, middle, out whiteCount, out blackCount);
            result += (float)(Math.Pow(1.5, whiteCount) - Math.Pow(1.5, blackCount));
        }
#if DEBUG_HEURISTIC_A
        Console.WriteLine("straight");
#endif
        foreach (int[] straight in straights)
        {
            countLine(gb, straight, out whiteCount, out blackCount);
            result += (float)(Math.Pow(1.7, whiteCount) - Math.Pow(1.7, blackCount));
        }
#if DEBUG_HEURISTIC_A
        Console.WriteLine("triple");
#endif
        foreach (int[] triple in triples)
        {
            countLine(gb, triple, out whiteCount, out blackCount);
            result += (float)(Math.Pow(1.9, whiteCount) - Math.Pow(1.9, blackCount));
        }
        if (IA_PIECES == IA_PIECES_BLACKS) result *= -1;
        return result;
    }

    void countLine(HOLESTATE[] gb, int[] line, out int whiteCount, out int blackCount) {
        int interiorWhites = 0;
        int interiorBlacks = 0;
        int borderWhites = 0;
        int borderBlacks = 0;
        if (gb[line[0]] == HOLESTATE.has_white) borderWhites++;
        else if (gb[line[0]] == HOLESTATE.has_black) borderBlacks++;
        if (gb[line[line.Length - 1]] == HOLESTATE.has_white) borderWhites++;
        else if (gb[line[line.Length - 1]] == HOLESTATE.has_black) borderBlacks++;
        for (int i = 1; i < line.Length - 1; i++)
        {
            if (gb[line[i]] == HOLESTATE.has_white) interiorWhites++;
            else if (gb[line[i]] == HOLESTATE.has_black) interiorBlacks++;
        }
        whiteCount = 0;
        blackCount = 0;
        if (interiorBlacks == 0 && borderBlacks < 2)
            whiteCount += borderWhites + interiorWhites;
        if (whiteCount > 0 && borderWhites == 0) whiteCount += 1 - borderBlacks;
        if (whiteCount == 5) whiteCount = 10;
        if (interiorWhites == 0 && borderWhites < 2)
            blackCount += borderBlacks + interiorBlacks;
        if (blackCount > 0 && borderBlacks == 0) blackCount += 1 - borderWhites;
        if (blackCount == 5) blackCount = 10;
#if DEBUG_HEURISTIC_A
        Console.WriteLine("W " + whiteCount + "  B " + blackCount);
#endif
    }

    int countShortLine(HOLESTATE[] gb, int[] line)
    {
        int whites = 0;
        int blacks = 0;
        foreach (int i in line)
        {
            if (gb[i] == HOLESTATE.has_white) whites++;
            else if (gb[i] == HOLESTATE.has_black) blacks++;
        }
        int whiteCount = 0;
        int blackCount = 0;
        if (blacks == 0 && whites > 0)
            whiteCount = whites;
        else if (whites == 0 && blacks > 0)
            blackCount = blacks;
#if DEBUG_HEURISTIC_A
        Console.WriteLine("W " + whiteCount + "  B " + blackCount);
#endif
        return whiteCount - blackCount;
    }

}
