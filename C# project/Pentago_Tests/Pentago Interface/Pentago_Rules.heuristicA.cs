//#define DEBUG_HEURISTIC_A
//#define NO_ROTATE_WHEN_ADD
//#define FAST_A

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
        float value;
        bool? player;
#if FAST_A
        player = boardValue(gb, out value);
        if (player != null)
            return (player == IA_PIECES) ? 100 : -100;
        return value;
#else
        float result = gb.get_player_turn() == IA_PIECES ? float.NegativeInfinity : float.PositiveInfinity;
#if NO_ROTATE_WHEN_ADD
        if (gb.get_turn_state() == Pentago_GameBoard.turn_state_addpiece)
        {
            player = boardValue(gb, out value);
            if (player != null)
                return (player == IA_PIECES) ? 100 : -100;
            return value;
        }
#else
        if (gb.get_turn_state() == Pentago_GameBoard.turn_state_addpiece)
            gb = new Pentago_GameBoard(gb.board, gb.get_player_turn(), Pentago_GameBoard.turn_state_rotate);
#endif
        Pentago_Move[] nplays = possible_plays(gb);
        Pentago_GameBoard ngb;
        foreach (Pentago_Move move in nplays)
        {
            ngb = after_rotate(gb, move);
            player = boardValue(ngb, out value);
#if DEBUG_HEURISTIC_A
            ngb.print_board();
            Console.WriteLine(value + " " + player);
#endif
            if (player != null)
                return (player == IA_PIECES) ? 100 : -100;
            if ((gb.get_player_turn() == IA_PIECES && value > result) 
                || (gb.get_player_turn() != IA_PIECES && value < result))
                result = value;
        }
        return result;
#endif
    }

    bool? boardValue(Pentago_GameBoard gb, out float value)
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

        value = 0;
#if DEBUG_HEURISTIC_A
        Console.WriteLine("monica");
#endif
        int whiteCount;
        int blackCount;
        bool? player;
        int whiteAlmost = 0;
        int blackAlmost = 0;
        foreach (int[] monica in monicas)
        {
            player = countLine(gb, monica, out whiteCount, out blackCount);
            if (player != null) return player;
            player = checkAlmost(gb, whiteCount, blackCount, ref whiteAlmost, ref blackAlmost);
            if (player != null) return player;
            value += (float)(Math.Pow(1.13, whiteCount) - Math.Pow(1.13, blackCount));
        }
#if DEBUG_HEURISTIC_A
        Console.WriteLine("middle");
#endif
        foreach (int[] middle in middles)
        {
            player = countLine(gb, middle, out whiteCount, out blackCount);
            if (player != null) return player;
            player = checkAlmost(gb, whiteCount, blackCount, ref whiteAlmost, ref blackAlmost);
            if (player != null) return player;
            value += (float)(Math.Pow(1.15, whiteCount) - Math.Pow(1.15, blackCount));
        }
#if DEBUG_HEURISTIC_A
        Console.WriteLine("straight");
#endif
        foreach (int[] straight in straights)
        {
            player = countLine(gb, straight, out whiteCount, out blackCount);
            if (player != null) return player;
            player = checkAlmost(gb, whiteCount, blackCount, ref whiteAlmost, ref blackAlmost);
            if (player != null) return player;
            value += (float)(Math.Pow(1.17, whiteCount) - Math.Pow(1.17, blackCount));
        }
#if DEBUG_HEURISTIC_A
        Console.WriteLine("triple");
#endif
        foreach (int[] triple in triples)
        {
            countShortLine(gb, triple, out whiteCount, out blackCount);
            player = checkAlmost(gb, whiteCount, blackCount, ref whiteAlmost, ref blackAlmost);
            if (player != null) return player;
            value += (float)(Math.Pow(1.19, whiteCount) - Math.Pow(1.19, blackCount));
        }
        if (IA_PIECES == IA_PIECES_BLACKS) value *= -1;
        return null;
    }

    bool? checkAlmost(Pentago_GameBoard gb, int whiteCount, int blackCount, ref int whiteAlmost, ref int blackAlmost)
    {
        if (whiteCount > 4)
        {
            whiteAlmost++;
#if DEBUG_HEURISTIC_A
            Console.WriteLine("white almost");
#endif
        }
        if (blackCount > 4)
        {
            blackAlmost++;
#if DEBUG_HEURISTIC_A
            Console.WriteLine("black almost");
#endif
        }
        if (whiteAlmost >= 2 && gb.get_player_turn() == IA_PIECES_WHITES) return IA_PIECES_WHITES;
        if (blackAlmost >= 2 && gb.get_player_turn() == IA_PIECES_BLACKS) return IA_PIECES_BLACKS;
        return null;
    }

    bool? countLine(Pentago_GameBoard gb, int[] line, out int whiteCount, out int blackCount) {
        int interiorWhites = 0;
        int interiorBlacks = 0;
        int borderWhites = 0;
        int borderBlacks = 0;
        if (gb.board[line[0]] == HOLESTATE.has_white) borderWhites++;
        else if (gb.board[line[0]] == HOLESTATE.has_black) borderBlacks++;
        if (gb.board[line[line.Length - 1]] == HOLESTATE.has_white) borderWhites++;
        else if (gb.board[line[line.Length - 1]] == HOLESTATE.has_black) borderBlacks++;
        for (int i = 1; i < line.Length - 1; i++)
        {
            if (gb.board[line[i]] == HOLESTATE.has_white) interiorWhites++;
            else if (gb.board[line[i]] == HOLESTATE.has_black) interiorBlacks++;
        }
        whiteCount = 0;
        blackCount = 0;
        if (interiorBlacks == 0 && borderBlacks < 2)
            whiteCount += borderWhites + interiorWhites;
        //if (whiteCount > 0 && borderWhites == 0) whiteCount += 1 - borderBlacks;
        if (interiorWhites == 0 && borderWhites < 2)
            blackCount += borderBlacks + interiorBlacks;
        //if (blackCount > 0 && borderBlacks == 0) blackCount += 1 - borderWhites;
#if DEBUG_HEURISTIC_A
        Console.WriteLine("W " + whiteCount + "  B " + blackCount);
#endif
        if (whiteCount == 4 && borderWhites == 0 || whiteCount > 4)
        {
            if (gb.get_player_turn() == IA_PIECES_WHITES) return IA_PIECES_WHITES;
            else whiteCount = 10;
        }
        else if (whiteCount == 4 && borderWhites < 2) whiteCount++;
        if (blackCount == 4 && borderBlacks == 0 || blackCount > 4)
        {
            if (gb.get_player_turn() == IA_PIECES_BLACKS) return IA_PIECES_BLACKS;
            else blackCount = 10;
        }
        else if (blackCount == 4 && borderBlacks < 2) blackCount++;
        return null;
    }

    void countShortLine(Pentago_GameBoard gb, int[] line, out int whiteCount, out int blackCount)
    {
        int whites = 0;
        int blacks = 0;
        foreach (int i in line)
        {
            if (gb.board[i] == HOLESTATE.has_white) whites++;
            else if (gb.board[i] == HOLESTATE.has_black) blacks++;
        }
        whiteCount = 0;
        blackCount = 0;
        if (blacks == 0 && whites > 0)
            whiteCount = whites;
        else if (whites == 0 && blacks > 0)
            blackCount = blacks;
#if DEBUG_HEURISTIC_A
        Console.WriteLine("W " + whiteCount + "  B " + blackCount);
#endif
    }

    HOLESTATE[] rotate(HOLESTATE[] gb, Pentago_Move move)
    {
        HOLESTATE[] new_gb = (HOLESTATE[])gb.Clone();
        if (move.rotDir == Pentago_Move.rotate_clockwise)
            for (int x = 0; x < 3; ++x) for (int y = 0; y < 3; ++y)
                    new_gb[Pentago_GameBoard.board_position_to_index(Math.Abs(2 - y), x, move.square2rotate)]
                        = gb[Pentago_GameBoard.board_position_to_index(x, y, move.square2rotate)];
        else
            for (int x = 0; x < 3; ++x) for (int y = 0; y < 3; ++y)
                    new_gb[Pentago_GameBoard.board_position_to_index(y, Math.Abs(2 - x), move.square2rotate)]
                        = gb[Pentago_GameBoard.board_position_to_index(x, y, move.square2rotate)];
        return new_gb;
    }

    Pentago_GameBoard after_rotate(Pentago_GameBoard gb, Pentago_Move move)
    {
        return new Pentago_GameBoard(rotate(gb.board, move), gb.get_player_turn(), gb.get_turn_state());
    }

}
