using System;
using System.Linq;

public partial class Pentago_Rules : IGameRules<Pentago_GameBoard, Pentago_Move>
{
    public const bool IA_PIECES_WHITES = Pentago_GameBoard.whites_turn;
    public const bool IA_PIECES_BLACKS = Pentago_GameBoard.blacks_turn;
    bool IA_PIECES;

    const float MAX_HEURISTIC_VALUE = 1000000;
    const float MIN_HEURISTIC_VALUE = -1000000;

    public enum EvaluationFunction { controlHeuristic, heuristicA, blabla2, blabla3 };
    EvaluationFunction ef;

    public bool remove_repeated_states_on_nextStates = false;
    public enum NextStatesFunction { all_states, removeSym_A_B, removeSym_A_B_C, someotherxpto };
    NextStatesFunction nsf;

    public static Pentago_Move[] all_possible_place_piece_moves = null;
    public static Pentago_Move[] all_possible_rotate_squares_moves = null;

    public Pentago_Rules(EvaluationFunction ef = EvaluationFunction.controlHeuristic, NextStatesFunction nsf = NextStatesFunction.all_states , bool iapieces = IA_PIECES_WHITES, bool remove_repeated_states_on_nextStates = false)
    {
        this.ef = ef;
        this.nsf = nsf;
        this.remove_repeated_states_on_nextStates = remove_repeated_states_on_nextStates;

        IA_PIECES = iapieces;

        //create all possible plays.
        //since we are using a class, there is no need to initialize them again
        //when we are getting possible moves in a board
        //just filter the impossible ones

        if (all_possible_place_piece_moves == null)
        {
            all_possible_place_piece_moves = new Pentago_Move[36];
            int i = 0;
            for (int s2p = 0; s2p < 4; s2p++)
                for (int x = 0; x < 3; x++)
                    for (int y = 0; y < 3; y++)
                    {
                        all_possible_place_piece_moves[i] = new Pentago_Move(s2p, x, y);
                        i++;
                    }
        }
        if (all_possible_rotate_squares_moves == null)
        {
            all_possible_rotate_squares_moves = new Pentago_Move[8];
            int i = 0;
            for (int s2p = 0; s2p < 4; s2p++)
            {
                all_possible_rotate_squares_moves[i] = new Pentago_Move(s2p, false);
                i++;
                all_possible_rotate_squares_moves[i] = new Pentago_Move(s2p, true);
                i++;
            }
        }
    }

    #region Interface Related XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

    public Pentago_Move[] possible_plays(Pentago_GameBoard gb)
    {
        if (gb.get_turn_state() == Pentago_GameBoard.turn_state_addpiece)
        {
            return all_possible_place_piece_moves.Where(move => move.is_move_possible(gb)).ToArray();
        }
        else
        {
            //it is allways possible to rotate! no need to check
            return all_possible_rotate_squares_moves;//result = all_possible_rotate_squares_moves.Where(move => move.is_move_possible(gb)).ToArray();
        }

       // return result;
    }

    /// <summary>
    /// not really needed with current implementation, but use instead of "state_after_move()" on minimax (if needed) to keep the generic pattern
    /// </summary>
    /// <returns></returns>
    public Pentago_GameBoard board_after_play(Pentago_GameBoard gb, Pentago_Move gmd)
    {
        return gmd.state_after_move(gb);
    }

    public Pentago_GameBoard[] next_states(Pentago_GameBoard gb)
    {
        Pentago_Move[] moves = possible_plays(gb);

        if (remove_repeated_states_on_nextStates && gb.get_turn_state() == Pentago_GameBoard.turn_state_rotate)
            return removeDuplicates(moves.Select(m => m.state_after_move(gb)).ToArray());//.Distinct().ToArray();
        else return moves.Select(m => m.state_after_move(gb)).ToArray();
    }

    public float? game_over(Pentago_GameBoard gb,int depth)
    {
        bool? player;
        if (gb.game_ended(out player))
        {
            if (player == null) return 0;
            if (player == IA_PIECES)
                return MAX_HEURISTIC_VALUE + 36 * 2 - depth;
            else return MIN_HEURISTIC_VALUE - 36 * 2 + depth;
        }
        return null;
    }

    public float evaluate(Pentago_GameBoard gb)
    {
        switch (ef)
        {
            case EvaluationFunction.controlHeuristic:
                return ControlHeuristic();
            case EvaluationFunction.blabla2:
                break;
            case EvaluationFunction.blabla3:
                break;
            default:
                break;
        }
        return 0;
    }

    public bool selectMINMAX(Pentago_GameBoard thisnode_gb, bool currentIterationNode)
    {
        return thisnode_gb.get_turn_state() == Pentago_GameBoard.turn_state_rotate ? !currentIterationNode : currentIterationNode;
    }

    #endregion

}
