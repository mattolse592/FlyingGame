using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlyingGame
{
    internal class Player
    {
        public double x, y;
        public int width;

        double xSpeed;
        double ySpeed;

        public Player(double x, double y, int width)
        {
            this.x = x;
            this.y = y;
            this.width = width;
        }

        public void Accelerate(string direction)
        {
            if (direction == "up")
            {
                ySpeed -= 0.1;
            }
            else if (direction == "down")
            {
                ySpeed += 0.1;
            }
            else if (direction == "left")
            {
                xSpeed -= 0.1;
            }
            else
            {
                xSpeed += 0.1;
            }
        }

        public void Move()
        {
            y += ySpeed;
            x += xSpeed;

            if (y < 0)
            {
                ySpeed = 0;
                y = 0;
            }
            else if (y > GameScreen.height - width)
            {
                ySpeed = 0;
                y = GameScreen.height - width;
            }
            if (x < 0)
            {
                xSpeed = 0;
                x = 0;
            }
            else if (x > GameScreen.width - width)
            {
                xSpeed = 0;
                x = GameScreen.width - width;
            }
        }

        public bool Collision(Goal goal)
        {
            Rectangle playerRec = new Rectangle(Convert.ToInt16(x), Convert.ToInt16(y), width, width);
            Rectangle goalRec = new Rectangle(Convert.ToInt16(goal.x), Convert.ToInt16(goal.y), Convert.ToInt16(goal.size), Convert.ToInt16(goal.size));

            if (playerRec.IntersectsWith(goalRec))
            { 
                return true;
            }
            return false;
        }
    }
}
