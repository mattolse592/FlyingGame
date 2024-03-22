using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyingGame
{
    public partial class GameOverScreen : UserControl
    {
        public GameOverScreen()
        {
            InitializeComponent();

            scoreLabel.Text = $"Score: {GameScreen.score}";
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            Form1.ChangeScreen(this, new GameScreen());
            GameScreen.time = Convert.ToInt16(timeSelect.Value);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
