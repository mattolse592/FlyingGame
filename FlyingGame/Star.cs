using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlyingGame
{
    internal class Star
    {
        public int height = 10;
        public int x, y;

        public PointF[] starPoints = new PointF[10];

        Random randGen = new Random();

        public double xDistanceFromCenter;
        public double yDistanceFromCenter;
        public double distance;

        public Star(int x, int y, double distance)
        {
            this.x = x;
            this.y = y;
            this.distance = distance;

            xDistanceFromCenter = (GameScreen.width / 2) - x;
            yDistanceFromCenter = (GameScreen.height / 2) - y;

            SetPoints();
        }

        private void SetPoints()
        {
            starPoints = new PointF[4];
            starPoints[0] = new PointF(0 + x, 0 + y);
            starPoints[1] = new PointF(20 + x, 0 + y);
            starPoints[2] = new PointF(20 + x, 20 + y);
            starPoints[3] = new PointF(0 + x, 20 + y);
        }

        public void Move()
        {
            distance += 0.1;

            x -= Convert.ToInt32(xDistanceFromCenter / 100 + distance * (xDistanceFromCenter / 70));
            y -= Convert.ToInt32(yDistanceFromCenter / 100 + distance * (yDistanceFromCenter / 70));

            SetPoints();
        }
    }
}
