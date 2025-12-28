using System.Runtime.CompilerServices;

namespace Крестики_нолики
{
    public partial class Form1 : Form
    {
        private int[,] board = new int[10, 10];
        private int currentPlayer = 1;
        private int player1Score = 0;
        private int player2Score = 0;
        private bool gameOver = false;
        private Point[] winLine = null;
        private int cellSize = 30; // Размер клетки уменьшен для 10x10 поля
        private int boardSize = 300; // Общий размер доски (10 клеток * 30)
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ClientSize = new Size(boardSize, boardSize + 100);
            this.Text = "Крестики-нолики 10x10";

            Button newGameBtn = new Button();
            newGameBtn.Text = "Новая игра";
            newGameBtn.Location = new Point(50, boardSize + 10);
            newGameBtn.Size = new Size(100, 30);
            newGameBtn.Click += button1_Click;
            this.Controls.Add(newGameBtn);

            Button resetScoreBtn = new Button();
            resetScoreBtn.Text = "Сбросить счет";
            resetScoreBtn.Location = new Point(150, boardSize + 10);
            resetScoreBtn.Size = new Size(100, 30);
            resetScoreBtn.Click += button2_Click;
            this.Controls.Add(resetScoreBtn);

            InitializeBoard();
        }
        private void InitializeBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    board[i, j] = 0;
                }
            }
            currentPlayer = 1;
            gameOver = false;
            winLine = null;
            this.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeBoard();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            player1Score = 0;
            player2Score = 0;
            InitializeBoard();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(Color.White);

            Pen pen = new Pen(Color.Black, 1);
            for (int i = 1; i < 10; i++)
            {
                g.DrawLine(pen, i * cellSize, 0, i * cellSize, boardSize);
                g.DrawLine(pen, 0, i * cellSize, boardSize, i * cellSize);
            }

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 1)
                    {
                        Pen xPen = new Pen(Color.DeepPink, 2);
                        g.DrawLine(xPen, j * cellSize + 5, i * cellSize + 5, j * cellSize + cellSize - 5, i * cellSize + cellSize - 5);
                        g.DrawLine(xPen, j * cellSize + cellSize - 5, i * cellSize + 5, j * cellSize + 5, i * cellSize + cellSize - 5);
                    }
                    else if (board[i, j] == 2)
                    {
                        Pen oPen = new Pen(Color.Purple, 2);
                        g.DrawEllipse(oPen, j * cellSize + 5, i * cellSize + 5, cellSize - 10, cellSize - 10);
                    }
                }
            }
            if (winLine != null)
            {
                Pen winPen = new Pen(Color.AliceBlue, 3);
                g.DrawLine(winPen, winLine[0], winLine[1]);
            }

            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            g.DrawString($"Игрок 1 (X): {player1Score}", font, brush, 10, boardSize + 50);
            g.DrawString($"Игрок 2 (O): {player2Score}", font, brush, 160, boardSize + 50);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (gameOver) return;
            int row = e.Y / cellSize;
            int col = e.X / cellSize;
            if (row >= 0 && row < 10 && col >= 0 && col < 10 && board[row, col] == 0)
            {
                board[row, col] = currentPlayer;
                this.Invalidate();
                if (CheckWin(currentPlayer))
                {
                    gameOver = true;
                    if (currentPlayer == 1)
                        player1Score++;
                    else
                        player2Score++;

                    string winner = currentPlayer == 1 ? "Игрок 1 (X)" : "Игрок 2 (O)";
                    MessageBox.Show($"Молодец ты сломал её {winner}!", "Результат игры");
                }
                else if (IsBoardFull())
                {
                    gameOver = true;
                    MessageBox.Show("Ну ок");
                }
                else
                {
                    currentPlayer = currentPlayer == 1 ? 2 : 1;
                }
            }
        }

        private bool CheckWin(int player)
        {
            // Проверка по горизонталям
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    if (board[i, j] == player && board[i, j + 1] == player && board[i, j + 2] == player &&
                        board[i, j + 3] == player && board[i, j + 4] == player)
                    {
                        winLine = new Point[]
                        {
                            new Point(j * cellSize + cellSize/2, i * cellSize + cellSize/2),
                            new Point((j + 4) * cellSize + cellSize/2, i * cellSize + cellSize/2)
                        };
                        return true;
                    }
                }
            }

            // Проверка по вертикалям
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (board[i, j] == player && board[i + 1, j] == player && board[i + 2, j] == player &&
                        board[i + 3, j] == player && board[i + 4, j] == player)
                    {
                        winLine = new Point[]
                        {
                            new Point(j * cellSize + cellSize/2, i * cellSize + cellSize/2),
                            new Point(j * cellSize + cellSize/2, (i + 4) * cellSize + cellSize/2)
                        };
                        return true;
                    }
                }
            }

            // Проверка диагоналей (слева направо)
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    if (board[i, j] == player && board[i + 1, j + 1] == player && board[i + 2, j + 2] == player &&
                        board[i + 3, j + 3] == player && board[i + 4, j + 4] == player)
                    {
                        winLine = new Point[]
                        {
                            new Point(j * cellSize + cellSize/2, i * cellSize + cellSize/2),
                            new Point((j + 4) * cellSize + cellSize/2, (i + 4) * cellSize + cellSize/2)
                        };
                        return true;
                    }
                }
            }

            // Проверка диагоналей (справа налево)
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 4; j < 10; j++)
                {
                    if (board[i, j] == player && board[i + 1, j - 1] == player && board[i + 2, j - 2] == player &&
                        board[i + 3, j - 3] == player && board[i + 4, j - 4] == player)
                    {
                        winLine = new Point[]
                        {
                            new Point(j * cellSize + cellSize/2, i * cellSize + cellSize/2),
                            new Point((j - 4) * cellSize + cellSize/2, (i + 4) * cellSize + cellSize/2)
                        };
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 0)
                        return false;
                }
            }
            return true;
        }
    }
}


