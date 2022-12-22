using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Space_race
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle();
        Rectangle player2 = new Rectangle();

        int obsticleHeight = 7;
        List<int> obsticleSpeed = new List<int>();
        List<int> obsticleSpeed2 = new List<int>();

        Random randGen = new Random();

        int spawnRateRate = 500;

        Stopwatch spawnRate = new Stopwatch();
        Stopwatch timer = new Stopwatch();

        int visableTImer = 30;


        List<Rectangle> obsticles = new List<Rectangle>();
        List<Rectangle> obsticles2 = new List<Rectangle>();

        bool wDown = false;
        bool sDown = false;
        bool iDown = false;
        bool kDown = false;

        int playerSpeeds = 4;

        int player1Score = 0;
        int player2Score = 0;

        Font font = new Font("Times New Roman", 40);

        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Pen orangePen = new Pen(Color.Orange, 10);
        Pen whitePen = new Pen(Color.White, 10);

        bool player1Win = false;
        bool player2Win = false;
        bool tie = false;

        string gameState = "menu";

        public Form1()
        {
            InitializeComponent();
            player1 = new Rectangle(Convert.ToInt32(this.Width * 0.33) + player1.Width / 2 - 75, this.Height - player1.Height - 100, 40, 40);
            player2 = new Rectangle(Convert.ToInt32(this.Width * 0.66) + player2.Width / 2 + 90, this.Height - player2.Height - 100, 40, 40);
            this.Focus();
            ticks.Enabled = true;
        }


        private void ticks_Tick(object sender, EventArgs e)
        {
            if (wDown == true)
            {
                player1.Y -= playerSpeeds;
            }
            if (sDown == true && player1.Y < this.Height - player1.Height - 40)
            {
                player1.Y += playerSpeeds;
            }

            if (iDown == true)
            {
                player2.Y -= playerSpeeds;
            }
            if (kDown == true && player2.Y < this.Height - player2.Height - 40)
            {
                player2.Y += playerSpeeds;
            }

            if (spawnRate.Elapsed.TotalMilliseconds > spawnRateRate)
            {
                for (int i = 0; i < 2; i++)
                {
                    int yValue = randGen.Next(this.Height - obsticleHeight - 100);
                    int y = randGen.Next(5, 11);
                    int x = randGen.Next(10, 21);
                    obsticles.Add(new Rectangle(0, yValue, x, y));
                    obsticleSpeed.Add(randGen.Next(1, 6));
                }
            }

            for (int i = 0; i < obsticles.Count; i++)
            {
                int obsticleMove = obsticles[i].X + obsticleSpeed[i];
                obsticles[i] = new Rectangle(obsticleMove, obsticles[i].Y, obsticles[i].Width, obsticles[i].Height);
                if (obsticles[i].IntersectsWith(player1))
                {
                    player1 = new Rectangle(Convert.ToInt32(this.Width * 0.33) + player1.Width / 2 - 94, this.Height - player1.Height - 50, 40, 40);
                }
                if (obsticles[i].X > this.Width / 2 - obsticles[i].Width)
                {
                    obsticles.RemoveAt(i);
                    obsticleSpeed.RemoveAt(i);
                }
            }

            if (spawnRate.Elapsed.TotalMilliseconds > spawnRateRate)
            {
                for (int i = 0; i < 2; i++)
                {
                    int yValue = randGen.Next(this.Height - obsticleHeight - 100);
                    int y = randGen.Next(5, 11);
                    int x = randGen.Next(10, 21);
                    obsticles2.Add(new Rectangle(this.Width / 2, yValue, x, y));
                    obsticleSpeed2.Add(randGen.Next(1, 6));
                }
                spawnRate.Restart();
            }

            for (int i = 0; i < obsticles2.Count; i++)
            {
                int obsticleMove = obsticles2[i].X + obsticleSpeed2[i];
                obsticles2[i] = new Rectangle(obsticleMove, obsticles2[i].Y, obsticles2[i].Width, obsticles2[i].Height);
                if (obsticles2[i].IntersectsWith(player2))
                {
                    player2 = new Rectangle(Convert.ToInt32(this.Width * 0.66) + player2.Width / 2 + 70, this.Height - player2.Height - 50, 40, 40);
                }
                if (obsticles2[i].X > this.Width)
                {
                    obsticles2.RemoveAt(i);
                    obsticleSpeed2.RemoveAt(i);
                }
            }


            if (player1.Y < 0)
            {
                player1Score++;
                player1 = new Rectangle(Convert.ToInt32(this.Width * 0.33) + player1.Width / 2 - 94, this.Height - player1.Height - 50, 40, 40);
                if (player1Score == 3)
                {
                    gameState = "restart";
                    ticks.Enabled = false;
                }
            }

            if (player2.Y < 0)
            {
                player2Score++;
                player2 = new Rectangle(Convert.ToInt32(this.Width * 0.66) + player2.Width / 2 + 70, this.Height - player2.Height - 50, 40, 40);
                if (player2Score == 3)
                {
                    gameState = "restart";
                    ticks.Enabled = false;
                }
            }

            if (timer.Elapsed.TotalSeconds >= 1)
            {
                visableTImer--;
                timer.Restart();
                if (visableTImer == 0)
                {
                    ticks.Enabled = false;
                    if (player2Score > player1Score)
                    {
                        gameState = "restart";
                        player2Win = true;
                    }
                    if (player2Score < player1Score)
                    {
                        gameState = "restart";
                        player1Win = true;
                    }
                    if (player2Score == player1Score)
                    {
                        gameState = "restart";
                        tie = true;
                    }
                }
            }
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "menu")
            {
                e.Graphics.DrawString("Space Race", font, orangeBrush, this.Width / 2 - 140, this.Height / 2 - 300);
            }
            if (player1Win == false && player2Win == false && tie == false && gameState == "running")
            {
                e.Graphics.FillRectangle(orangeBrush, player1);
                e.Graphics.FillRectangle(orangeBrush, player2);
                e.Graphics.DrawLine(whitePen, this.Width / 2, this.Height, this.Width / 2, 0);

                foreach (Rectangle i in obsticles)
                {
                    e.Graphics.FillRectangle(whiteBrush, i);
                }
                foreach (Rectangle i in obsticles2)
                {
                    e.Graphics.FillRectangle(whiteBrush, i);
                }
                if (player1Score > 0)
                {
                    e.Graphics.DrawString($"{player1Score}", font, orangeBrush, 0, 0);
                }
                if (player2Score > 0)
                {
                    e.Graphics.DrawString($"{player2Score}", font, orangeBrush, this.Width - 70, 0);
                }
                e.Graphics.DrawString($"{visableTImer}", font, orangeBrush, this.Width / 2 - 35, 0);
            }
            if (player1Win == true && gameState == "restart")
            {
                button2.Visible = true;
                button3.Visible = true;
                button2.Text = "Play Again";
                e.Graphics.DrawString("Player 1 Wins", font, orangeBrush, this.Width / 2 - 110, this.Height / 2 - 300);
            }
            if (player2Win == true && gameState == "restart")
            {
                button2.Visible = true;
                button3.Visible = true;
                button2.Text = "Play Again";
                e.Graphics.DrawString("Player 2 Wins", font, orangeBrush, this.Width / 2 - 110, this.Height / 2 - 300);
            }
            if (tie == true && gameState == "restart")
            {
                button2.Visible = true;
                button3.Visible = true;
                button2.Text = "Play Again";
                e.Graphics.DrawString("Tie", font, orangeBrush, this.Width / 2 - 40, this.Height / 2 - 300);
                Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (gameState == "restart")
            {
                player1 = new Rectangle(Convert.ToInt32(this.Width * 0.33) + player1.Width / 2 - 75, this.Height - player1.Height - 100, 40, 40);
                player2 = new Rectangle(Convert.ToInt32(this.Width * 0.66) + player2.Width / 2 + 90, this.Height - player2.Height - 100, 40, 40);
                obsticleHeight = 7;
                obsticleSpeed.Clear();
                obsticleSpeed2.Clear();

                spawnRate.Reset();
                timer.Reset();
                spawnRate.Start();
                timer.Start();

                visableTImer = 30;


                obsticles.Clear();
                obsticles2.Clear();

                wDown = false;
                sDown = false;
                iDown = false;
                kDown = false;

                player1Score = 0;
                player2Score = 0;

                player1Win = false;
                player2Win = false;
                tie = false;

                gameState = "running";
                Refresh();
                button2.Visible = false;
                button3.Visible = false;
                ticks.Enabled = true;

            }
            else
            {
                gameState = "running";
                spawnRate.Start();
                timer.Start();
                ticks.Enabled = true;
                button2.Visible = false;
                button3.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.I:
                    iDown = true;
                    break;
                case Keys.K:
                    kDown = true;
                    break;
            }
        }

        private void Form1_KeyUp_1(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.I:
                    iDown = false;
                    break;
                case Keys.K:
                    kDown = false;
                    break;
            }
        }
    }
}
