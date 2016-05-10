using System;
using System.Linq;

public partial class Pentago_Rules : IGameRules<Pentago_GameBoard, Pentago_Move>
{
    public const bool IA_PIECES_WHITES = Pentago_GameBoard.whites_turn;
    public const bool IA_PIECES_BLACKS = Pentago_GameBoard.blacks_turn;
    bool IA_PIECES;

    const float MAX_HEURISTIC_VALUE = 1000000;
    const float MIN_HEURISTIC_VALUE = -1000000;

    public enum EvaluationFunction { controlHeuristic, heuristicA, heuristicAstar, heuristic1, heuristic1dot2, heuristicAplusDiagonalHack };
    EvaluationFunction ef;

    public bool remove_repeated_states_on_nextStates = false;
    public enum NextStatesFunction { all_states, check_symmetries, removeSym_A_B, removeSym_A_B_C, someotherxpto };
    NextStatesFunction nsf;

    public static Pentago_Move[] all_possible_place_piece_moves = null;
    public static Pentago_Move[] all_possible_rotate_squares_moves = null;

    float draw_value;

    public Pentago_Rules(EvaluationFunction ef = EvaluationFunction.controlHeuristic, NextStatesFunction nsf = NextStatesFunction.all_states, bool iapieces = IA_PIECES_WHITES, bool remove_repeated_states_on_nextStates = false, float draw_value = 0)
    {
        this.ef = ef;
        this.nsf = nsf;
        this.remove_repeated_states_on_nextStates = remove_repeated_states_on_nextStates;
        this.draw_value = draw_value;

        IA_PIECES = iapieces;
        if (ef == EvaluationFunction.heuristicAplusDiagonalHack)
            setUpDiagonalHack();


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
            return sucessor(gb).Where(move => move.is_move_possible(gb)).ToArray();
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

    public float? game_over(Pentago_GameBoard gb, int depth)
    {
        bool? player;
        if (gb.game_ended(out player))
        {
            if (player == null) return draw_value;
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
            case EvaluationFunction.heuristicA:
                return heuristicA(gb);
            case EvaluationFunction.heuristicAstar:
                return heuristicAstar(gb);
            case EvaluationFunction.heuristic1:
                return heuristic1(gb.board);
            case EvaluationFunction.heuristic1dot2:
                return heuristic1dot2(gb.board);
            case EvaluationFunction.heuristicAplusDiagonalHack:
                return heuristicA(gb) * 2.0f + heuristic1dot2(gb.board);
            default:
                return 0;
        }
    }

    public Pentago_Move[] sucessor(Pentago_GameBoard gb)
    {
        switch (nsf)
        {
            case NextStatesFunction.all_states:
                return all_possible_place_piece_moves;
            case NextStatesFunction.check_symmetries:
                return check_symmetries(gb.board);
            default:
                return null;
        }
    }

    public bool selectMINMAX(Pentago_GameBoard thisnode_gb, bool currentIterationNode)
    {
        return thisnode_gb.get_turn_state() == Pentago_GameBoard.turn_state_rotate ? !currentIterationNode : currentIterationNode;
    }

    public int smart_depth(Pentago_GameBoard gb)
    {
        int emptyholes = gb.board.Count(o => o == Pentago_GameBoard.hole_state.is_empty);
        if (emptyholes <= 5) return 5;
        if (emptyholes <= 8) return 4;
        if (emptyholes <= 25) return 3;
        return 2;
    }


    public string toDisplayString()
    {
        string result;
        result = "RULES> ";
        result += "eval " + ef.ToString();
        switch (ef)
        {
            case EvaluationFunction.controlHeuristic:
                break;
            case EvaluationFunction.heuristicA:
                result += " bias: "
                + " mon=" + monica_strength
                + " mid=" + middle_strength
                + " stra=" + straight_strength
                + " tri=" + triple_strength;
                break;
            case EvaluationFunction.heuristicAstar:
                result += " bias: "
                + " mon=" + monica_strength
                + " mid=" + middle_strength
                + " stra=" + straight_strength
                + " tri=" + triple_strength
                + " rots=" + (study_rotations_on_rotate?"Y":"N");
                break;
            case EvaluationFunction.heuristic1:
                result += " - bias = " + heuristic1_bias;
                break;
            case EvaluationFunction.heuristic1dot2:
                result += " - rel:" + (HEUR12RELAXED ? "Y" : "N") + " | D_HACK:" + (diagonal_hack ? "Y" : "N");
                result += "\n bias: "
                    + " bow=" + heuristic1dot2_own_possibilities_weigth
                    + " bop=" + heuristic1dot2_oponent_possibilities_weigth
                    + " bowS=" + heuristic1dot2_own_strongChances_weigth
                    + " bopS=" + heuristic1dot2_oponent_strongChances_weigth;
                break;
            case EvaluationFunction.heuristicAplusDiagonalHack:
                result += " - rel:" + (HEUR12RELAXED ? "Y" : "N") + " | D_HACK:" + (diagonal_hack ? "Y" : "N");
                result += "\n bias: "
                    + " bow=" + heuristic1dot2_own_possibilities_weigth
                    + " bop=" + heuristic1dot2_oponent_possibilities_weigth
                    + " bowS=" + heuristic1dot2_own_strongChances_weigth
                    + " bopS=" + heuristic1dot2_oponent_strongChances_weigth
                    + " mon=" + monica_strength
                    + " mid=" + middle_strength
                    + " stra=" + straight_strength
                     +  "tri=" + triple_strength
                    ;
                break;
            default:
                break;
        }
        result += "\nsym " + (nsf == NextStatesFunction.all_states ? "N" : "Y");
        result += " | rdt " + (remove_repeated_states_on_nextStates ? "Y" : "N");//remove duplicate rotations
        result += " |draw :" + draw_value.ToString();
        return result;
    }

    #endregion

}
