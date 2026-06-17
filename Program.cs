using System;
using System.Collections.Generic;

class Program
{
    const int ROWS = 6;
    const int COLS = 7;
    const int EMPTY = 0;
    const int PLAYER = 1;
    const int AI = 2;
    const int WINDOW = 4;

    static int[,] board = new int[ROWS, COLS];

    static void Main()
    {
        int turn = PLAYER;

        while (true)
        {
            PrintBoard();
            if (turn == PLAYER)
            {
                Console.Write("Player (0-6): ");
                string input = Console.ReadLine()!;

                if (!int.TryParse(input, out int col) || col < 0 || col >= COLS)
                    continue;

                if (!IsValid(col))
                    continue;

                int row = GetRow(col);
                board[row, col] = PLAYER;

                if (WinningMove(board, PLAYER))
                {
                    PrintBoard();
                    Console.WriteLine("You win!");
                    break;
                }

                turn = AI;
            }
            else
            {
                Console.WriteLine("AI thinking...");

                int col = Minimax(board, 5, int.MinValue, int.MaxValue, true).Item1;

                int row = GetRow(col);
                board[row, col] = AI;

                if (WinningMove(board, AI))
                {
                    PrintBoard();
                    Console.WriteLine("AI wins!");
                    break;
                }
                turn = PLAYER;
            }
        }
    }
    static void PrintBoard()
    {
        Console.Clear();

        for (int r = ROWS - 1; r >= 0; r--)
        {
            for (int c = 0; c < COLS; c++)
                Console.Write(board[r, c] + " ");

            Console.WriteLine();
        }

        Console.WriteLine("0 1 2 3 4 5 6");
    }

    static bool IsValid(int col)
    {
        return board[ROWS - 1, col] == EMPTY;
    }

    static int GetRow(int col)
    {
        for (int r = 0; r < ROWS; r++)
        {
            if (board[r, col] == EMPTY)
                return r;
        }

        return -1;
    }

    static List<int> GetValidMoves(int[,] boardState)
    {
        List<int> moves = new List<int>();

        for (int c = 0; c < COLS; c++)
        {
            if (boardState[ROWS - 1, c] == EMPTY)
                moves.Add(c);
        }

        return moves;
    }

    static bool WinningMove(int[,] boardState, int piece)
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

    static (int, int) Minimax(int[,] boardState, int depth, int alpha, int beta, bool maximizing)
    {
        List<int> valid = GetValidMoves(boardState);

        if (depth == 0 ||
            valid.Count == 0 ||
            WinningMove(boardState, PLAYER) ||
            WinningMove(boardState, AI))
        {
            return (-1, Evaluate(boardState));
        }

        int bestCol = valid[0];

        if (maximizing)
        {
            int value = int.MinValue;

            foreach (int col in valid)
            {
                int[,] copy = Copy(boardState);

                int row = GetRowSim(copy, col);
                copy[row, col] = AI;

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
                int[,] copy = Copy(boardState);

                int row = GetRowSim(copy, col);
                copy[row, col] = PLAYER;

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

    static int Evaluate(int[,] boardState)
    {
        if (WinningMove(boardState, AI))
            return 1000000;

        if (WinningMove(boardState, PLAYER))
            return -1000000;

        int score = 0;

        for (int r = 0; r < ROWS; r++)
        {
            if (boardState[r, 3] == AI)
                score += 6;
            else if (boardState[r, 3] == PLAYER)
                score -= 6;
        }

        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS - 3; c++)
            {
                score += EvaluateWindow(
                    boardState[r, c],
                    boardState[r, c + 1],
                    boardState[r, c + 2],
                    boardState[r, c + 3]);
            }
        }

        for (int c = 0; c < COLS; c++)
        {
            for (int r = 0; r < ROWS - 3; r++)
            {
                score += EvaluateWindow(
                    boardState[r, c],
                    boardState[r + 1, c],
                    boardState[r + 2, c],
                    boardState[r + 3, c]);
            }
        }

        for (int r = 0; r < ROWS - 3; r++)
        {
            for (int c = 0; c < COLS - 3; c++)
            {
                score += EvaluateWindow(
                    boardState[r, c],
                    boardState[r + 1, c + 1],
                    boardState[r + 2, c + 2],
                    boardState[r + 3, c + 3]);
            }
        }

        for (int r = 3; r < ROWS; r++)
        {
            for (int c = 0; c < COLS - 3; c++)
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
    static int EvaluateWindow(int a, int b, int c, int d)
    {
        int ai = 0;
        int player = 0;
        int empty = 0;

        int[] window = { a, b, c, d };

        foreach (int cell in window)
        {
            if (cell == AI)
                ai++;
            else if (cell == PLAYER)
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
    static int[,] Copy(int[,] boardState)
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
    static int GetRowSim(int[,] boardState, int col)
    {
        for (int r = 0; r < ROWS; r++)
        {
            if (boardState[r, col] == EMPTY)
                return r;
        }
        return -1;
    }
}
