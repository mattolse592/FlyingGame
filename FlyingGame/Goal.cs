using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingGame
{
    internal class Goal
    {
        public double x = GameScreen.width / 2 - 70;
        public double y = GameScreen.height / 2 - 70;

        public int type;
        public string color = "red";
        public double size = 140;

        double xSpeed = 0.5;

        public Goal(int type)
        {
            this.type = type;
        }

        public void Move()
        {
            switch (type)
            {
                case 1:
                    x -= 4.1 * xSpeed;
                    y -= 3;

                    if (x < 100)
                    {
                        x = 100;
                        y = 77;
                        color = "green";
                        size = 523;
                    }
                    break;
                case 2:
                    if (size < 523)
                    {
                        x += 2.4 * xSpeed;
                        y -= 3;
                    }
                    else if (size >= 523)
                    {
                        color = "green";
                        size = 523;
                    }

                    break;
                case 3:
                    if (size < 523)
                    {
                        x += 2.4 * xSpeed;
                        y += 0.8;
                    }
                    else if (size >= 523)
                    {
                        color = "green";
                        size = 523;
                    }
                    break;
                case 4:
                    if (size < 523)
                    {
                        x -= 4.1 * xSpeed;
                        y += 0.8;
                    }
                    else if (size >= 523)
                    {
                        color = "green";
                        size = 523;
                    }
                    break;
            }

            size += 2.5;
            xSpeed += 0.01;
        }
    }
}
