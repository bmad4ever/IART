//#define DEBUG_ALPHA_BETA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public partial class MinMax <GAME_BOARD, GAME_MOVE_DESCRIPTION>
{


    float alpha_beta_minmaxMT(float alpha, float beta, GAME_BOARD gb, int depth, bool node)
    {
        //Random random = new Random();//probably faster than locking the same

        float? gover = rules.game_over(gb, depth);
        if (gover != null)
        {
            return gover.Value;
        }
        if (depth >= max_depth) return rules.evaluate(gb);
        GAME_BOARD[] nstates = rules.next_states(gb);
        bool nminmax = rules.selectMINMAX(gb, node);
        float next_value;
        foreach (int i in Enumerable.Range(0, nstates.Length).OrderBy(x => random.Next()))
        {
            GAME_BOARD ngb = nstates[i];
            next_value = alpha_beta_minmaxMT(alpha, beta, ngb, depth + 1, nminmax);
            if (node == MIN_NODE && beta > next_value) beta = next_value;
            else if (node == MAX_NODE && alpha < next_value) alpha = next_value;
            if (alpha >= beta) break;
        }
        return node == MIN_NODE ? beta : alpha;
    }


    public int num_of_thread = 6;
    GAME_MOVE_DESCRIPTION[] alpha_beta_minmax_initMT( GAME_BOARD gb)
    {
        Random rand= new Random();
        GAME_MOVE_DESCRIPTION[] nplays = rules.possible_plays(gb);
        nplays = nplays.OrderBy(x => rand.Next()).ToArray();

        if (nplays.Length < 4) return alpha_beta_minmax_init(gb);

        bool nminmax = rules.selectMINMAX(gb, MAX_NODE);


        float[] alpha = new float[num_of_thread];
        for (int a = alpha.Length - 1; a >= 0; --a) alpha[a] = float.NegativeInfinity;
      
        GAME_MOVE_DESCRIPTION[][] moves = new GAME_MOVE_DESCRIPTION[num_of_thread][];

        Parallel.For(0, num_of_thread, (n) =>
        {
            GAME_MOVE_DESCRIPTION[] temp_moves = new GAME_MOVE_DESCRIPTION[0];
            float next_value;
            int offset = nplays.Length / num_of_thread * n;
            int i = nplays.Length/ num_of_thread;
            if (n == num_of_thread-1) i += nplays.Length % num_of_thread;
            --i;
            for (; i >= 0; --i)
            {
                GAME_MOVE_DESCRIPTION nplay = nplays[offset+i];
                GAME_BOARD ngb = rules.board_after_play(gb, nplay);

                if (nminmax == MIN_NODE) next_value = alpha_beta_minmaxMT(alpha[n], float.NegativeInfinity, ngb, 1, MIN_NODE);
                else next_value = alpha_beta_minmax_init_auxMT(float.NegativeInfinity, float.PositiveInfinity, ngb, 1, out temp_moves);

                if (alpha[n] < next_value)
                {
                    alpha[n] = next_value;
                    moves[n] = new GAME_MOVE_DESCRIPTION[temp_moves.Length + 1];
                    moves[n][0] = nplay;
                    Array.Copy(temp_moves, 0, moves[n], 1, temp_moves.Length);
                }

            }

        });

        int best = Array.IndexOf(alpha, alpha.Max());

        return moves[best];
    }

    float alpha_beta_minmax_init_auxMT(float alpha, float beta, GAME_BOARD gb, int depth, out GAME_MOVE_DESCRIPTION[] moves)
    {
        //Random random = new Random();//probably faster than locking the same

        moves = new GAME_MOVE_DESCRIPTION[0];
        float? gover = rules.game_over(gb, depth);
        if (gover != null) return gover.Value;
        if (depth >= max_depth) return rules.evaluate(gb);

        GAME_MOVE_DESCRIPTION[] nplays = rules.possible_plays(gb);
        bool nminmax = rules.selectMINMAX(gb, MAX_NODE);
        GAME_MOVE_DESCRIPTION[] temp_moves = new GAME_MOVE_DESCRIPTION[0];
        float next_value;
        foreach (int i in Enumerable.Range(0, nplays.Length).OrderBy(x => random.Next()))
        {
            GAME_MOVE_DESCRIPTION nplay = nplays[i];
            GAME_BOARD ngb = rules.board_after_play(gb, nplay);
            if (nminmax == MIN_NODE) next_value = alpha_beta_minmaxMT(alpha, beta, ngb, depth + 1, MIN_NODE);
            else next_value = alpha_beta_minmax_init_auxMT(alpha, beta, ngb, depth + 1, out temp_moves);

            if (alpha < next_value)
            {
                alpha = next_value;
                moves = new GAME_MOVE_DESCRIPTION[temp_moves.Length + 1];
                moves[0] = nplay;
                Array.Copy(temp_moves, 0, moves, 1, temp_moves.Length);
            }

        }
        return alpha;
    }
}
