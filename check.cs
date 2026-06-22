using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectFour
{
    public class BoardControl : Control
    {
        private int[,] board = new int[Board.ROWS, Board.COLS];

        public event Action<int>? ColumnClicked;

        private const int CellSize = 70;
        private const int PaddinSize = 10;

        public BoardControl()
        {
            DoubleBuffered = true;

            Width = Board.COLS * CellSize + PaddinSize * 2;
            Height = Board.ROWS * CellSize + PaddinSize * 2;

            MouseClick += OnMouseClick;
        }

        public void SetBoard(int[,] newBoard)
        {
            board = newBoard;
            Invalidate();
        }

        private void OnMouseClick(object? sender, MouseEventArgs e)
        {
            int col = (e.X - PaddinSize) / CellSize;

            if (col >= 0 && col < Board.COLS)
                ColumnClicked?.Invoke(col);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;

            for (int r = 0; r < Board.ROWS; r++)
            {
                for (int c = 0; c < Board.COLS; c++)
        {
                    int x = PaddinSize + c * CellSize;
                    int y = PaddinSize + (Board.ROWS - 1 - r) * CellSize;

                    Brush brush = Brushes.White;

                    if (board[r, c] == Board.PLAYER)
                        brush = Brushes.Brown;
                    else if (board[r, c] == Board.AI)
                        brush = Brushes.LightGreen;

                    g.FillEllipse(brush, x + 5, y + 5, CellSize - 10, CellSize - 10);
                    g.DrawEllipse(Pens.Black, x + 5, y + 5, CellSize - 10, CellSize - 10);
                }
            }
        }
    }
}