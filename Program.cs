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
                string input = Console.ReadLine();

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
        int score = 0;

        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                if (boardState[r, c] == AI)
                    score += 2;

                if (boardState[r, c] == PLAYER)
                    score -= 2;
            }
        }

        return score;
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