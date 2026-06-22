using System;
using System.Collections.Generic;

namespace ConnectFour
{
    public static class Board
    {
        public const int ROWS = 6;
        public const int COLS = 7;
        public const int EMPTY = 0;
        public const int PLAYER = 1;
        public const int AI = 2;
        public const int WINDOW = 4;

        public static bool IsValid(int[,] boardState, int col)
        {
            return boardState[ROWS - 1, col] == EMPTY;
        }

        public static int GetRow(int[,] boardState, int col)
        {
            for (int r = 0; r < ROWS; r++)
            {
                if (boardState[r, col] == EMPTY)
                    return r;
            }

            return -1;
        }

        public static List<int> GetValidMoves(int[,] boardState)
        {
            List<int> moves = new List<int>();

            for (int c = 0; c < COLS; c++)
            {
                if (boardState[ROWS - 1, c] == EMPTY)
                    moves.Add(c);
            }

            return moves;
        }

        public static bool IsFull(int[,] boardState)
        {
            return GetValidMoves(boardState).Count == 0;
        }

        public static bool WinningMove(int[,] boardState, int piece)
        {
            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLS - 3; c++)
                {
                    if (boardState[r, c] == piece &&
                        boardState[r, c + 1] == piece &&
                        boardState[r, c + 2] == piece &&
                        boardState[r, c + 3] == piece)
                        return true;
                }
            }

            for (int c = 0; c < COLS; c++)
            {
                for (int r = 0; r < ROWS - 3; r++)
                {
                    if (boardState[r, c] == piece &&
                        boardState[r + 1, c] == piece &&
                        boardState[r + 2, c] == piece &&
                        boardState[r + 3, c] == piece)
                        return true;
                }
            }

            for (int r = 0; r < ROWS - 3; r++)
            {
                for (int c = 0; c < COLS - 3; c++)
                {
                    if (boardState[r, c] == piece &&
                        boardState[r + 1, c + 1] == piece &&
                        boardState[r + 2, c + 2] == piece &&
                        boardState[r + 3, c + 3] == piece)
                        return true;
                }
            }

            for (int r = 3; r < ROWS; r++)
            {
                for (int c = 0; c < COLS - 3; c++)
                {
                    if (boardState[r, c] == piece &&
                        boardState[r - 1, c + 1] == piece &&
                        boardState[r - 2, c + 2] == piece &&
                        boardState[r - 3, c + 3] == piece)
                        return true;
                }
            }

            return false;
        }

        public static int[,] Copy(int[,] boardState)
        {
            int[,] copy = new int[ROWS, COLS];

            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLS; c++)
                {
                    copy[r, c] = boardState[r, c];
                }
            }

            return copy;
        }

        public static int GetRowSim(int[,] boardState, int col)
        {
            for (int r = 0; r < ROWS; r++)
            {
                if (boardState[r, col] == EMPTY)
                    return r;
            }
            return -1;
        }
    }
}
