using System;

namespace ConnectFour
{
    public static class AI
    {
        public static (int, int) Minimax(int[,] boardState, int depth, int alpha, int beta, bool maximizing)
        {
            var valid = Board.GetValidMoves(boardState);

            if (depth == 0 ||
                valid.Count == 0 ||
                Board.WinningMove(boardState, Board.PLAYER) ||
                Board.WinningMove(boardState, Board.AI))
            {
                return (-1, Evaluate(boardState));
            }

            int bestCol = valid[0];

            if (maximizing)
            {
                int value = int.MinValue;

                foreach (int col in valid)
                {
                    int[,] copy = Board.Copy(boardState);

                    int row = Board.GetRowSim(copy, col);
                    copy[row, col] = Board.AI;

                    int score = Minimax(copy, depth - 1, alpha, beta, false).Item2;

                    if (score > value)
                    {
                        value = score;
                        bestCol = col;
                    }

                    alpha = Math.Max(alpha, value);

                    if (alpha >= beta)
                        break;
                }

                return (bestCol, value);
            }
            else
            {
                int value = int.MaxValue;

                foreach (int col in valid)
                {
                    int[,] copy = Board.Copy(boardState);

                    int row = Board.GetRowSim(copy, col);
                    copy[row, col] = Board.PLAYER;

                    int score = Minimax(copy, depth - 1, alpha, beta, true).Item2;

                    if (score < value)
                    {
                        value = score;
                        bestCol = col;
                    }

                    beta = Math.Min(beta, value);

                    if (alpha >= beta)
                        break;
                }

                return (bestCol, value);
            }
        }

        public static int Evaluate(int[,] boardState)
        {
            if (Board.WinningMove(boardState, Board.AI))
                return 1000000;

            if (Board.WinningMove(boardState, Board.PLAYER))
                return -1000000;

            int score = 0;

            for (int r = 0; r < Board.ROWS; r++)
            {
                if (boardState[r, 3] == Board.AI)
                    score += 6;
                else if (boardState[r, 3] == Board.PLAYER)
                    score -= 6;
            }

            for (int r = 0; r < Board.ROWS; r++)
            {
                for (int c = 0; c < Board.COLS - 3; c++)
                {
                    score += EvaluateWindow(
                        boardState[r, c],
                        boardState[r, c + 1],
                        boardState[r, c + 2],
                        boardState[r, c + 3]);
                }
            }

            for (int c = 0; c < Board.COLS; c++)
            {
                for (int r = 0; r < Board.ROWS - 3; r++)
                {
                    score += EvaluateWindow(
                        boardState[r, c],
                        boardState[r + 1, c],
                        boardState[r + 2, c],
                        boardState[r + 3, c]);
                }
            }

            for (int r = 0; r < Board.ROWS - 3; r++)
            {
                for (int c = 0; c < Board.COLS - 3; c++)
                {
                    score += EvaluateWindow(
                        boardState[r, c],
                        boardState[r + 1, c + 1],
                        boardState[r + 2, c + 2],
                        boardState[r + 3, c + 3]);
                }
            }

            for (int r = 3; r < Board.ROWS; r++)
            {
                for (int c = 0; c < Board.COLS - 3; c++)
                {
                    score += EvaluateWindow(
                        boardState[r, c],
                        boardState[r - 1, c + 1],
                        boardState[r - 2, c + 2],
                        boardState[r - 3, c + 3]);
                }
            }

            return score;
        }

        private static int EvaluateWindow(int a, int b, int c, int d)
        {
            int ai = 0;
            int player = 0;
            int empty = 0;

            int[] window = { a, b, c, d };

            foreach (int cell in window)
            {
                if (cell == Board.AI)
                    ai++;
                else if (cell == Board.PLAYER)
                    player++;
                else
                    empty++;
            }

            if (ai == 4)
                return 1000;

            if (ai == 3 && empty == 1)
                return 50;

            if (ai == 2 && empty == 2)
                return 10;

            if (player == 4)
                return -1000;

            if (player == 3 && empty == 1)
                return -80;

            if (player == 2 && empty == 2)
                return -5;

            return 0;
        }
    }
}
