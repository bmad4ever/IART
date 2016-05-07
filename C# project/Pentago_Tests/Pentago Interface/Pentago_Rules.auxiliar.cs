﻿using System.Collections.Generic;
using System.Linq;
using HOLESTATE = Pentago_GameBoard.hole_state;

public partial class Pentago_Rules
{

    Pentago_Move[] check_symmetries(HOLESTATE[] board)
    {
        if (check_rotation_180(board))
        {
            if (check_rotation_90_after_180_ok(board))
            {
                if (square0_has_maindiagonal_symmetry(board)) return getMovesSquare0Triang(); // check main diag and above in square 0
                else return getMoves(new int[1]{ 0 }); // check square 0
            }
            else if (check_reflection_ver_lef(board))
            {
                if (square0_has_maindiagonal_symmetry(board)) return getMovesSquare0Triang(); // check main diag and above in square 0
                else return getMoves(new int[1] { 0 }); // check square 0
            }
            /* there is no point in checking horizontal reflection, because 180 + horizontal => vertical */
            else if (check_reflection_main(board)) return getMovesTopTriang(); // check top triangle
            /* there is no point in checking antidiagonal reflection, because 180 + antidiagnoal => maindiagonal */
            else return getMoves(new int[2] { 0, 1 }); // check squares 0 and 1
        }
        if (check_reflection_ver(board))
        {
            /* there is no point in checking horizontal reflection, because vertical + horizontal => 180 */
            /* there is no point in checking maindiagonal reflection, because vertical + maindiagonal => 90 */
            return getMoves(new int[2] { 0, 1 }); // check squares 0 and 1
        }
        else if (check_reflection_hor(board))
        {
            /* there is no point in checking maindiagonal reflection, because horizontal + maindiagonal => 90 */
            return getMoves(new int[2] { 0, 2 }); // check squares 0 and 2
        }
        else if (check_reflection_main(board))
        {
            /* there is no point in checking antidiagonal reflection, because maindiagonal + antidiagonal => 180 */
            return getMovesMainDiagAbove(); // check maindiagonal and above
        }
        else if (check_reflection_anti(board)) return getMovesAntiDiagAbove(); // check antidiagonal and above
        return getMoves(new int[4] { 0, 1, 2, 3 }); // check all squares
    }

    Pentago_Move[] getMoves(int[] squares)
    {
        Pentago_Move[] moves = new Pentago_Move[squares.Length*9];
        int i = 0;
        foreach (int s in squares)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    moves[i] = new Pentago_Move(s, x, y);
                    i++;
                }
            }
        }
        return moves;
    }

    Pentago_Move[] getMovesSquare0Triang()
    {
        return new Pentago_Move[6] {
            new Pentago_Move(0, 0, 0),
            new Pentago_Move(0, 1, 0),
            new Pentago_Move(0, 2, 0),
            new Pentago_Move(0, 1, 1),
            new Pentago_Move(0, 2, 1),
            new Pentago_Move(0, 2, 2) };
    }

    Pentago_Move[] getMovesTopTriang()
    {
        return new Pentago_Move[12] {
            new Pentago_Move(0, 0, 0),
            new Pentago_Move(0, 1, 0),
            new Pentago_Move(0, 2, 0),
            new Pentago_Move(0, 1, 1),
            new Pentago_Move(0, 2, 1),
            new Pentago_Move(0, 2, 2),
            new Pentago_Move(1, 0, 0),
            new Pentago_Move(1, 1, 0),
            new Pentago_Move(1, 2, 0),
            new Pentago_Move(1, 0, 1),
            new Pentago_Move(1, 1, 1),
            new Pentago_Move(1, 0, 2) };
    }

    Pentago_Move[] getMovesMainDiagAbove()
    {
        Pentago_Move[] moves = new Pentago_Move[21];
        moves[0] = new Pentago_Move(0, 0, 0);
        moves[1] = new Pentago_Move(0, 1, 0);
        moves[2] = new Pentago_Move(0, 2, 0);
        moves[3] = new Pentago_Move(0, 1, 1);
        moves[4] = new Pentago_Move(0, 2, 1);
        moves[5] = new Pentago_Move(0, 2, 2);
        for (int i = 6; i < 15; i++)
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    moves[i] = new Pentago_Move(1, x, y);
        moves[15] = new Pentago_Move(3, 0, 0);
        moves[16] = new Pentago_Move(3, 1, 0);
        moves[17] = new Pentago_Move(3, 2, 0);
        moves[18] = new Pentago_Move(3, 1, 1);
        moves[19] = new Pentago_Move(3, 2, 1);
        moves[20] = new Pentago_Move(3, 2, 2);
        return moves;
    }

    Pentago_Move[] getMovesAntiDiagAbove()
    {
        Pentago_Move[] moves = new Pentago_Move[21];
        for (int i = 0; i < 9; i++)
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    moves[i] = new Pentago_Move(0, x, y);
        moves[9] = new Pentago_Move(1, 0, 0);
        moves[10] = new Pentago_Move(1, 1, 0);
        moves[11] = new Pentago_Move(1, 2, 0);
        moves[12] = new Pentago_Move(1, 1, 1);
        moves[13] = new Pentago_Move(1, 2, 1);
        moves[14] = new Pentago_Move(1, 2, 2);
        moves[15] = new Pentago_Move(2, 0, 0);
        moves[16] = new Pentago_Move(2, 1, 0);
        moves[17] = new Pentago_Move(2, 2, 0);
        moves[18] = new Pentago_Move(2, 1, 1);
        moves[19] = new Pentago_Move(2, 2, 1);
        moves[20] = new Pentago_Move(2, 2, 2);
        return moves;
    }

    bool check_rotation_180(HOLESTATE[] board)
    {
        return board[0] == board[35]
            && board[1] == board[34]
            && board[2] == board[33]
            && board[3] == board[32]
            && board[4] == board[31]
            && board[5] == board[30]
            && board[6] == board[29]
            && board[7] == board[28]
            && board[8] == board[27]
            && board[9] == board[26]
            && board[10] == board[25]
            && board[11] == board[24]
            && board[12] == board[23]
            && board[13] == board[22]
            && board[14] == board[21]
            && board[15] == board[20]
            && board[16] == board[19]
            && board[17] == board[18];
    }

    bool check_rotation_90_after_180_ok(HOLESTATE[] board)
    {
        return board[0] == board[30]
            && board[1] == board[24]
            && board[2] == board[18]
            && board[6] == board[31]
            && board[7] == board[25]
            && board[8] == board[19]
            && board[12] == board[32]
            && board[13] == board[26]
            && board[14] == board[20];
    }

    bool check_reflection_hor(HOLESTATE[] board)
    {
        return check_reflection_hor_top(board) && check_reflection_hor_bot(board);
    }

    bool check_reflection_hor_top(HOLESTATE[] board)
    {
        return board[0] == board[5]
            && board[1] == board[4]
            && board[2] == board[3]
            && board[6] == board[11]
            && board[7] == board[10]
            && board[8] == board[9]
            && board[12] == board[17]
            && board[13] == board[16]
            && board[14] == board[15];
    }

    bool check_reflection_hor_bot(HOLESTATE[] board)
    {
        return board[18] == board[23]
            && board[19] == board[22]
            && board[20] == board[21]
            && board[24] == board[29]
            && board[25] == board[28]
            && board[26] == board[27]
            && board[30] == board[35]
            && board[31] == board[34]
            && board[32] == board[33];
    }

    bool check_reflection_ver(HOLESTATE[] board)
    {
        return check_reflection_ver_lef(board) && check_reflection_ver_rig(board);
    }
    bool check_reflection_ver_lef(HOLESTATE[] board)
    {
        return board[0] == board[30]
            && board[1] == board[31]
            && board[2] == board[32]
            && board[6] == board[24]
            && board[7] == board[25]
            && board[8] == board[26]
            && board[12] == board[18]
            && board[13] == board[19]
            && board[14] == board[20];
    }
    bool check_reflection_ver_rig(HOLESTATE[] board)
    {
        return board[3] == board[33]
            && board[4] == board[34]
            && board[5] == board[35]
            && board[9] == board[27]
            && board[10] == board[28]
            && board[11] == board[29]
            && board[15] == board[21]
            && board[16] == board[22]
            && board[17] == board[23];
    }

    bool check_reflection_main(HOLESTATE[] board)
    {
        return board[1] == board[6]
            && board[2] == board[12]
            && board[3] == board[18]
            && board[4] == board[24]
            && board[5] == board[30]
            && board[8] == board[13]
            && board[9] == board[19]
            && board[10] == board[25]
            && board[11] == board[31]
            && board[15] == board[20]
            && board[16] == board[26]
            && board[17] == board[32]
            && board[22] == board[27]
            && board[23] == board[33]
            && board[29] == board[34];
    }

    bool check_reflection_anti(HOLESTATE[] board)
    {
        return board[0] == board[35]
            && board[1] == board[29]
            && board[2] == board[23]
            && board[3] == board[17]
            && board[4] == board[11]
            && board[6] == board[34]
            && board[7] == board[28]
            && board[8] == board[22]
            && board[9] == board[16]
            && board[12] == board[33]
            && board[13] == board[27]
            && board[14] == board[21]
            && board[18] == board[32]
            && board[19] == board[26]
            && board[24] == board[31];
    }

    /// <summary>
    /// copies a square from one board to a square in another board
    /// </summary>
    void copy_square2(HOLESTATE[] scr, HOLESTATE[] dst, int src_square, int dst_square)
    {
        for (int i = 0; i < 3; i++)
            for (int a = 0; a < 3; a++)
                dst[Pentago_GameBoard.board_position_to_index(i, a, dst_square)] =
                       scr[Pentago_GameBoard.board_position_to_index(i, a, src_square)];
    }

    HOLESTATE[] get_rotated_board_90deg_anticlockwise(HOLESTATE[] board)
    {
        HOLESTATE[] newboard = new HOLESTATE[36];

        copy_square2(board, newboard, 0, 2);
        copy_square2(board, newboard, 1, 0);
        copy_square2(board, newboard, 2, 3);
        copy_square2(board, newboard, 3, 1);

        Pentago_Move rotsquares = new Pentago_Move(0, Pentago_Move.rotate_anticlockwise);
        rotsquares.move(newboard, true, Pentago_GameBoard.turn_state_rotate);
        rotsquares.square2rotate = 1; rotsquares.move(newboard, true, Pentago_GameBoard.turn_state_rotate);
        rotsquares.square2rotate = 2; rotsquares.move(newboard, true, Pentago_GameBoard.turn_state_rotate);
        rotsquares.square2rotate = 3; rotsquares.move(newboard, true, Pentago_GameBoard.turn_state_rotate);

        return newboard;
    }

    bool square0_has_maindiagonal_symmetry(HOLESTATE[] board)
    {
        return board[1] == board[6] && board[12] == board[2] && board[13] == board[8];
    }
    bool square1_has_antidiagonal_symmetry(HOLESTATE[] board)
    {
        return board[3] == board[17] && board[4] == board[11] && board[9] == board[16];
    }

    /// <summary>
    /// find board simmetries
    /// </summary>
    /// <param name="board"></param>
    /// <param name="typeA">true if the board remains always the same after been rotated (only need to check 1 square)</param>
    /// <param name="typeB">true if remains the same after rotaing 180 degrees (only need to check 2 adjacent squares)</param>
    /// <param name="typeC">if true, only need to process half square in some instances ( typeA ;  typeB ; 1square between 2 diagonal squares mirrored) </param> 
    void board_has_symmetry(HOLESTATE[] board, out bool typeA, out bool typeB, out bool[] typeC)
    {
        typeA = true; typeB = true;
        typeC = new bool[] { false, false, false, false };

        HOLESTATE[] aux = (HOLESTATE[])board.Clone();
        typeC[0] = square0_has_maindiagonal_symmetry(aux);

        aux = get_rotated_board_90deg_anticlockwise(aux);
        if (aux != board) typeA = false;
        typeC[1] = square0_has_maindiagonal_symmetry(aux);

        aux = get_rotated_board_90deg_anticlockwise(aux);
        if (aux != board)
        {
            typeA = false; typeB = false;
            return;//might be removed later to allways compute typeC[3]
        }
        typeC[2] = square0_has_maindiagonal_symmetry(aux);

        aux = get_rotated_board_90deg_anticlockwise(aux);
        if (aux != board) typeA = false;
        typeC[3] = square0_has_maindiagonal_symmetry(aux);
    }



    /// <summary>
    /// removes duplicates. could also remove using Distinc + override GetHashCode , would be faster but could (very rarely) make a wrong avaliation
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static Pentago_GameBoard[] removeDuplicates(Pentago_GameBoard[] list)
    {
        List<Pentago_GameBoard> newlist = new List<Pentago_GameBoard>();
        for (int i = list.Length - 1; i >= 0; i--)
        {
            bool save = true;
            foreach (Pentago_GameBoard elem in newlist)
            {
                if (elem == list[i])
                {
                    save = false;
                    break;
                }
            }
            if (save) newlist.Add(list[i]);
        }

        return newlist.ToArray();
    }

    /// <summary>
    /// select the minimum value of an arbitrary number of integers variables
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    static public int select_min(params int[] values)
    {
        return values.Min();
    }

    static public int select_max(params int[] values)
    {
        return values.Max();
    }

}

