namespace Крестики_нолики
{
    public partial class Form1 : Form
    {
        private int[,] board = new int[3, 3]; // 0 - пусто, 1 - X, 2 - O
        private int currentPlayer = 1; // 1 - X, 2 - O
        private int player1Score = 0;
        private int player2Score = 0;
        private bool gameOver = false;
        private Point[] winLine = null;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ClientSize = new Size(300, 400);
            this.Text = "Крестики-нолики";
            Button newGameBtn = new Button();
            newGameBtn.Text = "Новая игра";
            newGameBtn.Location = new Point(50, 310);
            newGameBtn.Size = new Size(100, 30);
            newGameBtn.Click += button1_Click;
            this.Controls.Add(newGameBtn);

            Button resetScoreBtn = new Button();
            resetScoreBtn.Text = "Сбросить счет";
            resetScoreBtn.Location = new Point(150, 310);
            resetScoreBtn.Size = new Size(100, 30);
            resetScoreBtn.Click += button2_Click;
            this.Controls.Add(resetScoreBtn);

            InitializeBoard();
        }
        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
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

            Pen pen = new Pen(Color.Black, 2);
            for (int i = 1; i < 3; i++)
            {
                g.DrawLine(pen, i * 100, 0, i * 100, 300);
                g.DrawLine(pen, 0, i * 100, 300, i * 100);
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 1)
                    {
                        Pen xPen = new Pen(Color.DeepPink, 3);
                        g.DrawLine(xPen, j * 100 + 20, i * 100 + 20, j * 100 + 80, i * 100 + 80);
                        g.DrawLine(xPen, j * 100 + 80, i * 100 + 20, j * 100 + 20, i * 100 + 80);
                    }
                    else if (board[i, j] == 2)
                    {
                        Pen oPen = new Pen(Color.Purple, 3);
                        g.DrawEllipse(oPen, j * 100 + 20, i * 100 + 20, 60, 60);
                    }
                }
            }
            if (winLine != null)
            {
                Pen winPen = new Pen(Color.AliceBlue, 5);
                g.DrawLine(winPen, winLine[0], winLine[1]);
            }

            Font font = new Font("Arial", 12);
            Brush brush = Brushes.Black;
            g.DrawString($"Игрок 1 (X): {player1Score}", font, brush, 10, 350);
            g.DrawString($"Игрок 2 (O): {player2Score}", font, brush, 160, 350);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (gameOver) return;
            int row = e.Y / 100;
            int col = e.X / 100;
            if (row >= 0 && row < 3 && col >= 0 && col < 3 && board[row, col] == 0)
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
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
                {
                    winLine = new Point[]
                    {
                        new Point(15, i * 100 + 50),
                        new Point(285, i * 100 + 50)
                    };
                    return true;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                if (board[0, j] == player && board[1, j] == player && board[2, j] == player)
                {
                    winLine = new Point[]
                    {
                        new Point(j * 100 + 50, 15),
                        new Point(j * 100 + 50, 285)
                    };
                    return true;
                }
            }
            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
            {
                winLine = new Point[]
                {
                    new Point(15, 15),
                    new Point(285, 285)
                };
                return true;
            }

            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            {
                winLine = new Point[]
                {
                    new Point(285, 15),
                    new Point(15, 285)
                };
                return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == 0)
                        return false;
                }
            }
            return true;
        }
    }
}

