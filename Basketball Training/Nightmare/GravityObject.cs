using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class GravityObject
    {
        public int posx, posy, radius;
        private int m, terminalv = 50000;
        public int v = 30, vh = 0, vu = 0;
        public int suruseg = 4;
        public Pen pen = Pens.Black;
        public Brush brush = Brushes.Orange;
        public Rectangle hitboxy = new Rectangle();
        public Rectangle hitboxx = new Rectangle();
        public Rectangle hitbox = new Rectangle();
        private Rectangle leftstrip = new Rectangle();
        private Rectangle rightstrip = new Rectangle();



        public void DrawCircle(Graphics g)
        {
            g.DrawEllipse(pen,posx,posy,radius,radius);
            g.FillEllipse(brush, posx, posy, radius, radius);
            hitboxy = new Rectangle(posx, posy - radius / 2, 1, radius);
            hitboxx = new Rectangle(posx - radius / 2, posy , radius, 1);
            hitbox = Rectangle.Union(hitboxx, hitboxy);
        }

        public void RedrawCirle(Graphics g)
        {
            g.FillEllipse(brush, posx - (radius / 2), posy - (radius / 2), radius, radius);
            g.DrawEllipse(pen, posx - (radius / 2), posy - (radius / 2), radius, radius);
            hitboxy = new Rectangle(posx, posy - radius / 2, 1, radius);
            hitboxx = new Rectangle(posx - radius / 2, posy, radius, 1);
            leftstrip = new Rectangle(posx - radius / 4, posy - radius * 87 / 200, 1, radius * 87 / 100);
            rightstrip = new Rectangle(posx + radius / 4, posy - radius * 87 / 200, 1, radius * 87 / 100);
            hitbox = Rectangle.Union(hitboxx, hitboxy);
            g.DrawRectangle(new Pen(Brushes.Black), hitboxy);
            g.DrawRectangle(new Pen(Brushes.Black), hitboxx);
            g.DrawRectangle(new Pen(Brushes.Black), leftstrip);
            g.DrawRectangle(new Pen(Brushes.Black), rightstrip);

        }

        public void fall(Graphics g)
        {
            m = radius * suruseg;
            terminalv = m * 200;
            if (v < terminalv)
            {
                v = m + v;
            }
            if (vh > radius / 8 )
            {
                vh -= 1;

            }
            else if (vh < radius / -8 )
            {
                vh += 1;
            }
            posx = posx + vh / 10;
            posy = posy + v / 1000 ;
        }



    }
}
