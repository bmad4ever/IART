using System.Collections.Generic;
using System.Linq;
using HOLESTATE = Pentago_GameBoard.hole_state;

public partial class Pentago_Rules
{

    bool check_rotation_90(HOLESTATE[] board)
    {
        if (board[0] != board[30]) return false;
        if (board[0] != board[35]) return false;
        if (board[0] != board[5]) return false;
        if (board[1] != board[24]) return false;
        if (board[1] != board[34]) return false;
        if (board[1] != board[11]) return false;
        if (board[2] != board[18]) return false;
        if (board[2] != board[33]) return false;
        if (board[2] != board[17]) return false;
        if (board[6] != board[31]) return false;
        if (board[6] != board[29]) return false;
        if (board[6] != board[4]) return false;
        if (board[7] != board[25]) return false;
        if (board[7] != board[28]) return false;
        if (board[7] != board[10]) return false;
        if (board[8] != board[19]) return false;
        if (board[8] != board[27]) return false;
        if (board[8] != board[16]) return false;
        if (board[12] != board[32]) return false;
        if (board[12] != board[23]) return false;
        if (board[12] != board[3]) return false;
        if (board[13] != board[26]) return false;
        if (board[13] != board[22]) return false;
        if (board[13] != board[9]) return false;
        if (board[14] != board[20]) return false;
        if (board[14] != board[21]) return false;
        if (board[14] != board[15]) return false;
        return true;
    }

    bool check_rotation_180(HOLESTATE[] board)
    {
        if (board[0] != board[35]) return false;
        if (board[1] != board[34]) return false;
        if (board[2] != board[33]) return false;
        if (board[3] != board[32]) return false;
        if (board[4] != board[31]) return false;
        if (board[5] != board[30]) return false;
        if (board[6] != board[29]) return false;
        if (board[7] != board[28]) return false;
        if (board[8] != board[27]) return false;
        if (board[9] != board[26]) return false;
        if (board[10] != board[25]) return false;
        if (board[11] != board[24]) return false;
        if (board[12] != board[23]) return false;
        if (board[13] != board[22]) return false;
        if (board[14] != board[21]) return false;
        if (board[15] != board[20]) return false;
        if (board[16] != board[19]) return false;
        if (board[17] != board[18]) return false;
        return true;
    }

    bool check_reflection_ver(HOLESTATE[] board)
    {
        if (board[0] != board[5]) return false;
        if (board[1] != board[4]) return false;
        if (board[2] != board[3]) return false;
        if (board[6] != board[11]) return false;
        if (board[7] != board[10]) return false;
        if (board[8] != board[9]) return false;
        if (board[12] != board[17]) return false;
        if (board[13] != board[16]) return false;
        if (board[14] != board[15]) return false;
        if (board[18] != board[23]) return false;
        if (board[19] != board[22]) return false;
        if (board[20] != board[21]) return false;
        if (board[24] != board[29]) return false;
        if (board[25] != board[28]) return false;
        if (board[26] != board[27]) return false;
        if (board[30] != board[35]) return false;
        if (board[31] != board[34]) return false;
        if (board[32] != board[33]) return false;
        return true;
    }

    bool check_reflection_hor(HOLESTATE[] board)
    {
        if (board[0] != board[30]) return false;
        if (board[1] != board[31]) return false;
        if (board[2] != board[32]) return false;
        if (board[3] != board[33]) return false;
        if (board[4] != board[34]) return false;
        if (board[5] != board[35]) return false;
        if (board[6] != board[24]) return false;
        if (board[7] != board[25]) return false;
        if (board[8] != board[26]) return false;
        if (board[9] != board[27]) return false;
        if (board[10] != board[28]) return false;
        if (board[11] != board[29]) return false;
        if (board[12] != board[18]) return false;
        if (board[13] != board[19]) return false;
        if (board[14] != board[20]) return false;
        if (board[15] != board[21]) return false;
        if (board[16] != board[22]) return false;
        if (board[17] != board[23]) return false;
        return true;
    }

    bool check_reflection_main(HOLESTATE[] board)
    {
        if (board[1] != board[6]) return false;
        if (board[2] != board[12]) return false;
        if (board[3] != board[18]) return false;
        if (board[4] != board[24]) return false;
        if (board[5] != board[30]) return false;
        if (board[8] != board[13]) return false;
        if (board[9] != board[19]) return false;
        if (board[10] != board[25]) return false;
        if (board[11] != board[31]) return false;
        if (board[15] != board[20]) return false;
        if (board[16] != board[26]) return false;
        if (board[17] != board[32]) return false;
        if (board[22] != board[27]) return false;
        if (board[23] != board[33]) return false;
        if (board[29] != board[34]) return false;
        return true;
    }

    bool check_reflection_anti(HOLESTATE[] board)
    {
        if (board[0] != board[35]) return false;
        if (board[1] != board[29]) return false;
        if (board[2] != board[23]) return false;
        if (board[3] != board[17]) return false;
        if (board[4] != board[11]) return false;
        if (board[6] != board[34]) return false;
        if (board[7] != board[28]) return false;
        if (board[8] != board[22]) return false;
        if (board[9] != board[16]) return false;
        if (board[12] != board[33]) return false;
        if (board[13] != board[27]) return false;
        if (board[14] != board[21]) return false;
        if (board[18] != board[32]) return false;
        if (board[19] != board[26]) return false;
        if (board[24] != board[31]) return false;
        return true;
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

