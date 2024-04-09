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
using System.Media;

namespace FlyingGame
{
    public partial class GameScreen : UserControl
    {
        List<Star> stars = new List<Star>();
        Player hero;
        Goal goal;

        Random randGen = new Random();

        Stopwatch gameWatch = new Stopwatch();

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush greenBrush = new SolidBrush(Color.LimeGreen);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);
        SolidBrush darkGreenBrush = new SolidBrush(Color.Green);
        Pen whitePen = new Pen(Color.White, 1);
        Pen greenPen = new Pen(Color.Green, 5);

        SoundPlayer crashPlayer = new SoundPlayer(Properties.Resources.crash);
        SoundPlayer wrongPlayer = new SoundPlayer(Properties.Resources.wrong);
        SoundPlayer scorePlayer = new SoundPlayer(Properties.Resources.score);

        Rectangle centerRec;

        public static int width, height, score;
        public static int speedMultiplier = 1;
        public static int time = 60;

        public static bool crash;

        int boost = 400;
        int moveDisabledTime = 0;

        float timeLeft;
        bool wKeyDown, aKeyDown, sKeyDown, dKeyDown;

        public GameScreen()
        {
            InitializeComponent();

            width = this.Width;
            height = this.Height;

            score = 0;

            //add stars
            centerRec = new Rectangle(width / 2 - 70, height / 2 - 70, 140, 140);
            for (int i = 0; i < 100; i++)
            {
                AddStar();
            }

            hero = new Player(randGen.Next(20, 1900), randGen.Next(20, 1050), 140);
            goal = new Goal(randGen.Next(1, 5));

            while (hero.Collision(goal))
            {
                hero = new Player(randGen.Next(20, 1900), randGen.Next(20, 1050), 140);
            }

            gameWatch.Start();
            Refresh();
        }
        
        //adds a star in a random location in the center
        private void AddStar()
        {
            int x = randGen.Next(width / 2 - 150, width / 2 + 150);
            int y = randGen.Next(height / 2 - 150, height / 2 + 150);

            Rectangle starRec = new Rectangle(x, y, 10, 10);
            
            while (starRec.IntersectsWith(centerRec))
            {
                x = randGen.Next(width / 2 - 150, width / 2 + 150);
                y = randGen.Next(height / 2 - 150, height / 2 + 150);

                starRec = new Rectangle(x, y, 10, 10);
            }

            double distance = 1;

            stars.Add(new Star(x, y, distance));
        }
        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.D:
                    dKeyDown = true;
                    break;
                case Keys.Space:
                    if (moveDisabledTime == 0)
                    {
                        speedMultiplier = 4;
                    }
                    break;
            }
        }
        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.D:
                    dKeyDown = false;
                    break;
                case Keys.Space:
                    speedMultiplier = 1;
                    break;
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move stars
            foreach (Star s in stars)
            {
                s.Move();

                #region respawn stars
                if (s.x < 0 || s.x > GameScreen.width - 20 || s.y < 0 || s.y > GameScreen.height - 20)
                {
                    s.x = randGen.Next(GameScreen.width / 2 - 50, GameScreen.width / 2 + 50);
                    s.y = randGen.Next(GameScreen.height / 2 - 50, GameScreen.height / 2 + 50);

                    Rectangle starRec = new Rectangle(s.x, s.y, 10, 10);

                    while (starRec.IntersectsWith(centerRec))
                    {
                        s.x = randGen.Next(width / 2 - 150, width / 2 + 150);
                        s.y = randGen.Next(height / 2 - 150, height / 2 + 150);

                        starRec = new Rectangle(s.x, s.y, 10, 10);
                    }

                    s.distance = 1;

                    s.xDistanceFromCenter = (GameScreen.width / 2) - s.x;
                    s.yDistanceFromCenter = (GameScreen.height / 2) - s.y;
                }
                #endregion
            }
            #region accelerate code
            if (wKeyDown == true)
            {
                hero.Accelerate("up");
            }
            if (aKeyDown == true)
            {
                hero.Accelerate("left");
            }
            if (sKeyDown == true)
            {
                hero.Accelerate("down");
            }
            if (dKeyDown == true)
            {
                hero.Accelerate("right");
            }
            #endregion

            if (moveDisabledTime > 0)
            {
                moveDisabledTime--;
            }
            else if (moveDisabledTime == 0)
            {
                hero.Move();
            }

            goal.Move();

            xspeedLabel.Text = "";//hero.xSpeed.ToString("00");
            ySpeedLabel.Text = "";//hero.ySpeed.ToString("00");

            #region boost code
            if (speedMultiplier > 1 && boost > 3)
            {
                boost -= 4;
            }
            else if (boost < 400)
            {
                boost++;
            }

            if (boost <= 3)
            {
                speedMultiplier = 1;
            }
            #endregion

            #region crash code
            if (crash == true)
            {
                crashPlayer.Play();

                crash = false;
                hero = new Player(randGen.Next(20, 1800), randGen.Next(20, 1050), 140);

                while (hero.Collision(goal))
                {
                    hero = new Player(randGen.Next(20, 1800), randGen.Next(20, 1050), 140);
                }

                moveDisabledTime = 100;
            }
            #endregion

            //respawn goal
            if (hero.Collision(goal))
            {
                if (goal.color == "green")
                {
                    scorePlayer.Play();
                    score++;
                }
                else
                {
                    wrongPlayer.Play();
                    score--;
                }
                goal = new Goal(randGen.Next(1, 5));
                scoreLabel.Text = $"Score: {score}";
            }

            Refresh();
        }
        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            timeLeft = time * 1000 - gameWatch.ElapsedMilliseconds;

            var testVar = time * 1000 - gameWatch.ElapsedMilliseconds;
            timerLabel.Text = Convert.ToString(testVar);

            if (timeLeft <= 0)
            {
                gameTimer.Enabled = false;
                countdownTimer.Enabled = false;

                Form1.ChangeScreen(this, new GameOverScreen());
            }
        }
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            foreach (Star s in stars)
            {
                e.Graphics.FillPolygon(whiteBrush, s.starPoints);
            }
            #region draw guideLines
            e.Graphics.DrawRectangle(whitePen, width / 2 - 70, height / 2 - 70, 140, 140);
            e.Graphics.DrawRectangle(whitePen, 0, 0, width - 1, height - 1);
            e.Graphics.DrawLine(whitePen, width / 2 - 70, height / 2 - 70, 0, 0);
            e.Graphics.DrawLine(whitePen, width / 2 + 70, height / 2 - 70, width, 0);
            e.Graphics.DrawLine(whitePen, width / 2 - 70, height / 2 + 70, 0, height);
            e.Graphics.DrawLine(whitePen, width / 2 + 70, height / 2 + 70, width, height);

            switch (goal.type)
            {
                case 1:
                    e.Graphics.DrawRectangle(greenPen, 100, 77, 525, 525);
                    break;
                case 2:
                    e.Graphics.DrawRectangle(greenPen, 1357, 68, 525, 525);
                    break;
                case 3:
                    e.Graphics.DrawRectangle(greenPen, 1357, 653, 525, 525);
                    break;
                case 4:
                    e.Graphics.DrawRectangle(greenPen, 91, 653, 525, 525);
                    break;
            }
            #endregion

            if (goal.color == "red")
            {
                e.Graphics.FillRectangle(redBrush, Convert.ToInt16(goal.x), Convert.ToInt16(goal.y), Convert.ToInt16(goal.size), Convert.ToInt16(goal.size));
            }
            else
            {
                e.Graphics.FillRectangle(darkGreenBrush, Convert.ToInt16(goal.x), Convert.ToInt16(goal.y), Convert.ToInt16(goal.size), Convert.ToInt16(goal.size));
            }

            //draw boost
            e.Graphics.FillRectangle(whiteBrush, 10, 500 - boost, 40, boost);
            e.Graphics.DrawRectangle(whitePen, 10, 100, 40, 400);

            //draw player
            if (moveDisabledTime > 0)
            {
                e.Graphics.FillRectangle(orangeBrush, Convert.ToInt16(hero.x), Convert.ToInt16(hero.y), hero.width, hero.width);
            }
            else
            {
                e.Graphics.FillRectangle(greenBrush, Convert.ToInt16(hero.x), Convert.ToInt16(hero.y), hero.width, hero.width);
            }
        }
    }
}
