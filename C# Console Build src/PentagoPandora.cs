using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago_Tests
{
    static class PentagoPandora
    {
        //only possible because we already know there is a winning strategy
        //the ultimate Pentago AI is about to be born !!!

        static Pentago_Rules rules;

        static void get_board_identifier(Pentago_GameBoard gb, out byte id1, out ulong id2)
        {
            Pentago_GameBoard.hole_state[] board = gb.board;
            id1 = 0; id2 = 0;
            for (int i = 0; i < 32; i++)
            {
                if (board[i] == Pentago_GameBoard.hole_state.is_empty) continue;
                id2 += ((ulong)(board[i] == Pentago_GameBoard.hole_state.has_black ? 1 : 2)) << (i * 2);
            }

            for (int i = 0; i < 4; i++)
            {
                if (board[i + 30] == Pentago_GameBoard.hole_state.is_empty) continue;
                id1 += Convert.ToByte((board[i] == Pentago_GameBoard.hole_state.has_black ? 1 : 2) << (i * 2));
            }

        }

        static string get_board_file(Pentago_GameBoard gb)
        {
            Pentago_GameBoard.hole_state[] board = gb.board;
            int numWhites = 0;
            // numBlacks = 0;
            for (int i = 0; i < 36; i++)
            {
                if (board[i] == Pentago_GameBoard.hole_state.is_empty) continue;
                if (board[i] == Pentago_GameBoard.hole_state.has_white) numWhites++;
                //else numBlacks++;
            }

            return "pent_" + numWhites;
        }

        static Pentago_Move get_play(Pentago_GameBoard gb)
        {
            //check if victory is possible within this play


            //check if victory is possible within this play
            byte id1; ulong id2;
            get_board_identifier(gb, out id1, out id2);

            //read stuff from binary file
            using (var fileStream = new FileStream(get_board_file(gb), FileMode.Open, FileAccess.Read, FileShare.None))
            using (var bw = new BinaryReader(fileStream))
            {
                while (bw.BaseStream.Position != bw.BaseStream.Length)
                {
                    byte readID1 = bw.ReadByte();
                    ulong readID2 = bw.ReadUInt64();
                    int readIndex = bw.ReadInt16();
                    int readSqr2Rot = bw.ReadByte();
                    bool readRotD = bw.ReadBoolean();
                    if (readID1 == id1 && readID2 == id2) return new Pentago_Move(readIndex, readSqr2Rot, readRotD);
                }
            }
            return null;
        }

        static void createDataFiles()
        {
            for (int i = 0; i < 18; i++)
            {
                if (File.Exists("pent_" + i))
                {
                    File.Delete("pent_" + i);
                }

                // Create the file.
                using (FileStream fs = File.Create("pent_" + i)) ;
            }
        }

        static void appendData(string filename, byte id1, ulong id2, short index, byte square2rotate, bool rotDir)
        {
            using (var fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fileStream))
            {
                bw.Write(id1);
                bw.Write(id2);
                bw.Write(index);
                bw.Write(square2rotate);
                bw.Write(rotDir);
            }
        }

        public static void BUILD_PANDORA()
        {
            createDataFiles();
            rules = new Pentago_Rules(Pentago_Rules.EvaluationFunction.func1, Pentago_Rules.NextStatesFunction.all_states);
            BUILD_PANDORA_MINIMAX_MAX(new Pentago_GameBoard());
        }

        static bool BUILD_PANDORA_MINIMAX_MAX(Pentago_GameBoard gb)
        {
            bool? winner;
            if (gb.game_ended(out winner))
            {
                if (winner != null && winner == Pentago_GameBoard.whites_turn) return true;
                else return false;
            }

            Pentago_GameBoard auxOriginal = gb.Clone();

            Pentago_Move[] plays1 = rules.possible_plays(gb);
            //check for victory on 1st step
            foreach (Pentago_Move pm in plays1)
            {
                if (pm.state_after_move(auxOriginal).game_ended(out winner))
                {
                    if (winner != null && winner == Pentago_GameBoard.whites_turn) return true;
                }
            }

            //no victory found
            //apply second step for each possible
            Pentago_Move[] plays2 = Pentago_Rules.all_possible_rotate_squares_moves;
            foreach (Pentago_Move pm1 in plays1)
            {
                foreach (Pentago_Move pm2 in plays2)
                    if (BUILD_PANDORA_MINIMAX_MIN(pm2.state_after_move(pm1.state_after_move(auxOriginal))))
                    {
                        byte id1; ulong id2;
                        get_board_identifier(auxOriginal, out id1, out id2);
                        appendData(get_board_file(auxOriginal), id1, id2, (short)pm1.index, (byte)pm2.square2rotate, pm2.rotDir);
                        return true;
                    }
            }

            return false;
        }

        static bool BUILD_PANDORA_MINIMAX_MIN(Pentago_GameBoard gb)
        {
            bool? winner;
            if (gb.game_ended(out winner))
            {
                if (winner != null && winner == Pentago_GameBoard.whites_turn) return true;
                else return false;
            }

            Pentago_GameBoard auxOriginal = gb.Clone();

            Pentago_Move[] plays1 = rules.possible_plays(gb);
            //check for victory on 1st step
            foreach (Pentago_Move pm in plays1)
            {
                if (pm.state_after_move(auxOriginal).game_ended(out winner))
                { 
                    if (winner!=null && winner == Pentago_GameBoard.blacks_turn) return false;
                }
            }

            //no victory found
            //apply second step for each possible
            Pentago_Move[] plays2 = Pentago_Rules.all_possible_rotate_squares_moves;
            foreach (Pentago_Move pm1 in plays1)
            {
                foreach (Pentago_Move pm2 in plays2)
                    if (!BUILD_PANDORA_MINIMAX_MAX(pm2.state_after_move(pm1.state_after_move(auxOriginal))))
                    {
                        return false;
                    }
            }

            return true;
        }


        public static void test_simple_save_read()
        {
            createDataFiles();

            Pentago_GameBoard gb = new Pentago_GameBoard();

            byte id1; ulong id2;
            get_board_identifier(gb,out id1,out id2);

            appendData(get_board_file(gb), id1, id2, 2,1,false);
            appendData(get_board_file(gb), id1, id2, 2, 1, false);

            Console.WriteLine( get_play(gb).ToString() );

        }

    }
}
