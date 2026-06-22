using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectFour
{
    public class GameForm : Form
    {
        private int[,] board = new int[Board.ROWS, Board.COLS];
        private bool gameOver = false;

        private BoardControl boardControl;
        private Label statusLabel;
        private Button restartButton;

public GameForm()
{
    Text = "Connect Four";
    StartPosition = FormStartPosition.CenterScreen;
    FormBorderStyle = FormBorderStyle.FixedSingle;
    MaximizeBox = false;

    // 1. Setup the status label
    statusLabel = new Label();
    statusLabel.Text = "";
    statusLabel.Left = 10;
    statusLabel.Top = 15; 
    statusLabel.Width = 200;

    restartButton = new Button();
    restartButton.Text = "Restart";
    restartButton.Left = 220;
    restartButton.Top = 10;
    restartButton.Click += RestartGame!;


    boardControl = new BoardControl();
    boardControl.Left = 0;
    boardControl.Top = 50; 
    boardControl.SetBoard(board);
    boardControl.ColumnClicked += OnColumnClicked;

    Controls.Add(boardControl);
    Controls.Add(statusLabel);
    Controls.Add(restartButton);

    ClientSize = new Size(boardControl.Width, boardControl.Height + 50);
}

        private void OnColumnClicked(int col)
        {
            if (gameOver)
                return;

            if (!Board.IsValid(board, col))
                return;

            PlacePiece(col, Board.PLAYER);

            if (CheckGameEnd(Board.PLAYER))
                return;

            statusLabel.Text = "";

            int aiCol = AI.Minimax(board, 5, int.MinValue, int.MaxValue, true).Item1;

            PlacePiece(aiCol, Board.AI);

            if (CheckGameEnd(Board.AI))
                return;

            statusLabel.Text = "";
        }

        private void PlacePiece(int col, int piece)
        {
            int row = Board.GetRow(board, col);
            board[row, col] = piece;

            boardControl.SetBoard(board);
        }

        private bool CheckGameEnd(int player)
        {
            if (Board.WinningMove(board, player))
            {
                gameOver = true;
                statusLabel.Text = player == Board.PLAYER ? "You win!" : "AI wins!";
                MessageBox.Show(statusLabel.Text);
                return true;
            }

            if (Board.IsFull(board))
            {
                gameOver = true;
                statusLabel.Text = "Draw!";
                MessageBox.Show("Draw!");
                return true;
            }

            return false;
        }

        private void RestartGame(object sender, EventArgs e)
        {
            board = new int[Board.ROWS, Board.COLS];
            gameOver = false;
            boardControl.SetBoard(board);
            statusLabel.Text = "";
        }
    }
}