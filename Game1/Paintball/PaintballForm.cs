using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paintball
{
    public partial class PaintballForm : Form
    {
        private const int PlayerRadius = 20;
        private const int BallSpeed = 5;

        private bool isMousePressed = false;
        private Point playerPosition;
        private Point ballPosition;
        private int ballDirectionX = 1;
        private int ballDirectionY = 1;
        private System.Windows.Forms.Timer gameTimer;

        public PaintballForm()
        {
            InitializeComponent();

            // Установка начальной позиции игрока
            playerPosition = new Point(
                ClientSize.Width / 2 - PlayerRadius,
                ClientSize.Height - PlayerRadius * 2
            );

            // Установка начальной позиции мяча
            ballPosition = new Point(
                ClientSize.Width / 2 - PlayerRadius / 2,
                ClientSize.Height / 2 - PlayerRadius / 2
            );

            // Запуск игрового таймера
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 30; // 30 миллисекунд
            gameTimer.Tick += gameTimer_Tick;
            gameTimer.Start();
        }

        private void PaintballForm_Paint(object sender, PaintEventArgs e)
        {
            // Отрисовка игрока (круга)
            e.Graphics.FillEllipse(Brushes.Blue, new Rectangle(playerPosition, new Size(PlayerRadius * 2, PlayerRadius * 2)));

            // Отрисовка мяча (круга)
            e.Graphics.FillEllipse(Brushes.Red, new Rectangle(ballPosition, new Size(PlayerRadius, PlayerRadius)));
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Перемещение мяча
            ballPosition.X += BallSpeed * ballDirectionX;
            ballPosition.Y += BallSpeed * ballDirectionY;

            // Отскок мяча от границ окна
            if (ballPosition.X <= 0 || ballPosition.X >= ClientSize.Width - PlayerRadius)
                ballDirectionX *= -1;
            if (ballPosition.Y <= 0 || ballPosition.Y >= ClientSize.Height - PlayerRadius)
                ballDirectionY *= -1;

            // Проверка столкновения мяча с игроком
            if (Math.Abs(ballPosition.X + PlayerRadius - playerPosition.X - PlayerRadius) < PlayerRadius &&
                Math.Abs(ballPosition.Y + PlayerRadius - playerPosition.Y - PlayerRadius) < PlayerRadius)
            {
                // Мяч попал в игрока, игра закончена
                gameTimer.Stop();
                MessageBox.Show("Вы проиграли!");
                Close();
            }

            // Перерисовка окна
            Refresh();

        }

        private void PaintballForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMousePressed)
            {
                // Перемещение игрока
                playerPosition.X = e.X - PlayerRadius;
                playerPosition.Y = e.Y - PlayerRadius;
            }
        }

        private void PaintballForm_MouseDown(object sender, MouseEventArgs e)
        {
            isMousePressed = true;
        }

        private void PaintballForm_MouseUp(object sender, MouseEventArgs e)
        {
            isMousePressed = false;
        }

    }
}
