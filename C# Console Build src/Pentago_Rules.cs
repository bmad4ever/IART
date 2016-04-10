using System;

public class Pentago_Rules : GameRules<Pentago_GameBoard,Pentago_Move> {

    enum EvaluationFunction { blabla1 , blabla2 , blabla3};
    EvaluationFunction ef;

    Pentago_Rules(EvaluationFunction ef)
    {
        this.ef = ef;
    }

    #region  Auxiliar Methods

    /// <summary>
    /// copies a square from one board to a square in another board
    /// </summary>
    void copy_square2(Pentago_GameBoard.hole_state[] scr, Pentago_GameBoard.hole_state[] dst, int src_square, int dst_square)
    {
        for (int i = 0; i < 3; i++)
            for (int a = 0; a < 3; a++)
             dst[Pentago_GameBoard.board_position_to_index(i,a,dst_square)] =
                    scr[Pentago_GameBoard.board_position_to_index(i, a, src_square)];
    }

    Pentago_GameBoard.hole_state[] get_rotated_board_90deg_anticlockwise(Pentago_GameBoard.hole_state[] board)
    {
        Pentago_GameBoard.hole_state[] newboard = new Pentago_GameBoard.hole_state[36];

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

    bool square0_has_maindiagonal_symmetry(Pentago_GameBoard.hole_state[] board)
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
    void board_has_symmetry(Pentago_GameBoard.hole_state[] board, out bool typeA, out bool typeB, out bool[] typeC)
    {
        typeA = true; typeB = true;
        typeC = new bool[] { false, false, false, false };

        Pentago_GameBoard.hole_state[] aux = (Pentago_GameBoard.hole_state[]) board.Clone();
        typeC[0] = square0_has_maindiagonal_symmetry(aux);

        aux = get_rotated_board_90deg_anticlockwise(aux);
        if (aux != board) typeA=false;
        typeC[1] = square0_has_maindiagonal_symmetry(aux);

        aux = get_rotated_board_90deg_anticlockwise(aux);
        if (aux != board) { typeA = false; typeB = false;
            return;//might be removed later to allways compute typeC[3]
        }
        typeC[2] = square0_has_maindiagonal_symmetry(aux);

        aux = get_rotated_board_90deg_anticlockwise(aux);
        if (aux != board) typeA = false;
        typeC[3] = square0_has_maindiagonal_symmetry(aux);
    }



    #endregion

    //Interface Related --------------------------------------------------------------------

    public Pentago_Move[] possible_plays(Pentago_GameBoard gb)
	{
        throw new NotImplementedException();
    }

    public Pentago_GameBoard board_after_play(Pentago_GameBoard gmd)
	{
        throw new NotImplementedException();
    }

    public Pentago_GameBoard[] next_states(Pentago_GameBoard gb)
    {
        throw new NotImplementedException();
    }

    public bool game_over(Pentago_GameBoard gb)
	{
        throw new NotImplementedException();
    }

    public float evaluate(Pentago_GameBoard gb)
	{
        throw new NotImplementedException();
        switch (ef)
        {
            case EvaluationFunction.blabla1:
                break;
            case EvaluationFunction.blabla2:
                break;
            case EvaluationFunction.blabla3:
                break;
            default:
                break;
        }
    }

    public Pentago_GameBoard board_after_play(Pentago_Move gmd)
    {
        throw new NotImplementedException();
    }

}
