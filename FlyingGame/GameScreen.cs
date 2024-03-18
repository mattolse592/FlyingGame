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
    public partial class GameScreen : UserControl
    {
        List<Star> stars = new List<Star>();
        Player hero;
        Goal goal;

        Random randGen = new Random();

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush greenBrush = new SolidBrush(Color.LimeGreen);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush darkGreenBrush = new SolidBrush(Color.Green);

        Pen whitePen = new Pen(Color.White, 1);
        Pen greenPen = new Pen(Color.Green, 5);
        Rectangle centerRec;



        public static int width, height;
        bool wKeyDown, aKeyDown, sKeyDown, dKeyDown;
        public GameScreen()
        {
            InitializeComponent();

            width = this.Width;
            height = this.Height;

            for (int i = 0; i < 100; i++)
            {
                AddStar();
            }

            hero = new Player(this.Width / 2, this.Height / 2, 140);
            goal = new Goal(2);

            Refresh();
        }
        private void AddStar()
        {
            int x = randGen.Next(width / 2 - 50, width / 2 + 50);
            int y = randGen.Next(height / 2 - 50, height / 2 + 50);

            Rectangle starRec = new Rectangle(x, y, 10, 10);
            centerRec = new Rectangle(width / 2 - 70, height / 2 - 70, 140, 140);

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
            }
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
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
            hero.Move();
            goal.Move();

            xLabel.Text = $"x: {goal.x}";
            yLabel.Text = $"y: {goal.y}";
            sizeLabel.Text = $"size: {goal.size}";
                
            if (goal.color == "green")
            {
                if (hero.Collision(goal))
                {
                    goal = new Goal(randGen.Next(2, 3));
                }
            }

            Refresh();
        }



        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            foreach (Star s in stars)
            {
                e.Graphics.FillPolygon(whiteBrush, s.starPoints);
            }
            #region draw guideLines
            e.Graphics.DrawRectangle(whitePen, width / 2 - 70, height / 2 - 70, 140, 140);
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
                    e.Graphics.DrawRectangle(greenPen, 1341, 75, 525, 525);
                    break;
                case 3:

                    break;
                case 4:

                    break;
            }
            #endregion

            if (goal.color == "red")
            {
                e.Graphics.FillRectangle(redBrush, Convert.ToInt16(goal.x), Convert.ToInt16(goal.y), Convert.ToInt16(goal.size), Convert.ToInt16(goal.size));
            } else
            {
                e.Graphics.FillRectangle(darkGreenBrush, Convert.ToInt16(goal.x), Convert.ToInt16(goal.y), Convert.ToInt16(goal.size), Convert.ToInt16(goal.size));
            }


            e.Graphics.FillRectangle(greenBrush, Convert.ToInt16(hero.x), Convert.ToInt16(hero.y), hero.width, hero.width);

        }
    }
}
