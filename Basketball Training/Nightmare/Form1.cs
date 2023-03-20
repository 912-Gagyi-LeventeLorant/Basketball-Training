using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        GravityObject circle = new GravityObject();
        GravityObject circle2 = new GravityObject();
        PictureBox main = new PictureBox();
        Bitmap buffer;
        Timer timer = new Timer();
        Timer help = new Timer();
        Timer fpshelp = new Timer();
        bool on = false, odd = true;
        int xcord, ycord, helpx, helpy;
        int helpx1 = 0, helpy1 = 0;
        int fps = 0 , scoredhelp = 0, goalcounter = 0;
        Rectangle[] wall = new Rectangle[2];
        Rectangle sensor = new Rectangle();
     /*   Rectangle wall[0] = new Rectangle(300, 900, 900, 500);
        Rectangle wall[1] = new Rectangle(1200, 900, 400, 300);*/
        Rectangle playfield = new Rectangle();
        Rectangle[] line = new Rectangle[4];
        Label goal = new Label();
        Timer scored = new Timer();
        SoundPlayer kosar = new SoundPlayer("hittingnet.wav");
        bool bottomin = false;
        string goaltext;
        int pont = 2;
        

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            
            main.Location = new Point(0, 30);

            circle.posx = 600;
            circle.posy = 200;
            circle.radius = 200;

            Controls.Add(main);
            Resize += new EventHandler(Form1_Resize);
            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);
            help.Interval = 10;
            help.Tick += new EventHandler(help_Tick);
            main.MouseDown += new MouseEventHandler(main_MouseDown);
            fpshelp.Interval = 1000;
            fpshelp.Tick += Fps_Tick;
            scored.Interval = 100;
            scored.Tick += Scored_Tick;

            main.Size = new Size(ClientSize.Width, ClientSize.Height - 30);
            buffer = new Bitmap(main.Width, main.Height);

            Graphics g = Graphics.FromImage(buffer);

            circle.DrawCircle(g);

            main.Image = buffer;
            main.Refresh();
            g.Clear(Color.Black);


            goal.BackColor = Color.White;
            goal.Location = new Point(0, 0);
            goal.TextAlign = ContentAlignment.MiddleCenter;
            goal.Font = new Font(new FontFamily("Arial"), 22 , FontStyle.Bold);
            Controls.Add(goal);


        }

        private void Scored_Tick(object sender, EventArgs e)
        {
            scoredhelp++;

            if (scoredhelp % 3 == 0) goal.Text = "";
            else goal.Text = goaltext;


            if (scoredhelp == 13)
            {
                scoredhelp = 0;
                goal.Text = "";
                circle.posx = 480;
                circle.posy = 540;
                circle.v = 30;
                circle.vh = 0;
                main.MouseDown += new MouseEventHandler(main_MouseDown);
                scored.Enabled = false;
            }
        }

        private void Fps_Tick(object sender, EventArgs e)
        {
            label3.Text = "FPS: " + fps.ToString();
            fps = 0;
        }


        void help_Tick(object sender, EventArgs e)
        {

                                
            circle.vu /= 2;
            circle.vh = circle.vh / 2;

        }

        void timer_Tick(object sender, EventArgs e)
        {

            gravity(circle);
            Draw(circle);

            if (sensor.Contains(new Point(circle.posx, circle.posy + circle.radius / 2)))
            {
                bottomin = true;

            }
            if(bottomin) if (sensor.Contains(new Point(circle.posx, circle.posy - circle.radius / 4)))
            {
                bottomin = false;
                goalcounter += pont;
                if(goalcounter > 1) goaltext = goalcounter + " POINTS!";
                else goaltext = goalcounter + " POINT!";
                    goal.Text = goaltext;
                scored.Enabled = true;  
                main.MouseDown -= main_MouseDown;
                kosar.Play();
                }
            


            for (int i = 0; i<2; i++)
            {
                if (circle.hitbox.IntersectsWith(wall[i]))
                {
                    intersects(wall[i]);

                }

            }
            

        }

        private void intersects(Rectangle rectangle)
        {
            if (circle.hitboxx.IntersectsWith(rectangle))
            {
                circle.vh /= -3; ; if (circle.hitboxx.Left < rectangle.Left) circle.posx -= Rectangle.Intersect(rectangle, circle.hitboxx).Width;
                else circle.posx += Rectangle.Intersect(rectangle, circle.hitboxx).Width;
            }
            else if (circle.hitboxy.IntersectsWith(rectangle))
            {
                if (circle.hitboxy.Top < rectangle.Top) { circle.vu = Math.Abs(circle.vu) / -5; circle.v /= -2; circle.v += circle.vu; circle.posy -= Rectangle.Intersect(rectangle, circle.hitboxy).Height; }
                else
                {
                    circle.vu /= -1; circle.v /= -1; circle.v += circle.vu; circle.posy += Rectangle.Intersect(rectangle, circle.hitboxy).Height * 2;
                }
            }
            else if ((rectangle.X - circle.posx) * (rectangle.X - circle.posx) + (rectangle.Y - circle.posy) * (rectangle.Y - circle.posy) <= circle.radius * circle.radius / 4)
            {
                circle.posx -= 5;
                circle.posy -= 5;
                negyediknegyed();
            }
            else if ((rectangle.X + rectangle.Width - circle.posx) * (rectangle.X + rectangle.Width - circle.posx) + (rectangle.Y - circle.posy) * (rectangle.Y - circle.posy) <= circle.radius * circle.radius / 4)
            {
                circle.posx += 5;
                circle.posy -= 5;
                harmadiknegyed();
            }

            void harmadiknegyed()
            {
                int F = (Math.Abs(circle.v) + Math.Abs(circle.vh * 100 )) / 2;
                circle.v = (F * ((circle.radius / 2 - Rectangle.Intersect(circle.hitbox, rectangle).Height + 1) * 100 / (circle.radius / 2))) / -100;
                circle.vh = (F / 100 * ((circle.radius / 2 - Rectangle.Intersect(circle.hitbox, rectangle).Width + 1) * 100 / (circle.radius / 2))) / 100;
            }

            void negyediknegyed()
            {
                int F = (Math.Abs(circle.v)  + Math.Abs(circle.vh * 100 ))/2;
                circle.v = (F * ((circle.radius / 2 - Rectangle.Intersect(circle.hitbox, rectangle).Height + 1) * 100 / (circle.radius / 2))) / -100;
                circle.vh = (F / 100 * ((circle.radius / 2 - Rectangle.Intersect(circle.hitbox, rectangle).Width + 1) * 100 / (circle.radius / 2))) / -100;
            }
        }

        void main_MouseDown(object sender, MouseEventArgs e)
        {
            

            int dx = e.X - circle.posx, dy = e.Y - circle.posy;
            if ((dx * dx + dy * dy) * 4 <= circle.radius * circle.radius)
            {
                circle.vh = 0;
                circle.vu = 0;
                timer.Enabled = false;
                xcord = circle.posx - e.X;
                ycord = circle.posy - e.Y;
                help.Enabled = true;
                main.MouseMove += new MouseEventHandler(main_MouseMove);
                main.MouseUp += new MouseEventHandler(main_MouseUp);
            }

        }

        void main_MouseUp(object sender, MouseEventArgs e)
        {

            

            circle.posx = e.X + xcord;
            circle.posy = e.Y + ycord;

            if(circle.posx + circle.radius / 2 > main.Width / 2)
            {
                pont = 2;
            }

            else pont = 3;

            circle.v = 30 + circle.vu*100;
            Draw(circle);
            if (on == true) { timer.Enabled = true; }
            help.Enabled = false;
            main.MouseMove -= new MouseEventHandler(main_MouseMove);
            main.MouseUp -= new MouseEventHandler(main_MouseUp);
            
        }

        void main_MouseMove(object sender, MouseEventArgs e)
        {
            moving(circle, e);
        }

        private void moving(GravityObject obj, MouseEventArgs e)
        {
            helpx = e.X;
            helpy = e.Y;

            obj.posx = e.X + xcord;
            obj.posy = e.Y + ycord;
            Draw(obj);

            if (odd) { helpx1 = helpx; helpy1 = helpy; odd = false; }
            else
            {
                obj.vh = (obj.vh + (helpx - helpx1));
                obj.vu = (obj.vu + (helpy - helpy1));
                odd = true;
            }
        }



        private void gravity(GravityObject obj)
        {
            if(!(playfield.Contains(circle.hitboxy) && playfield.Contains(circle.hitboxx))){ 


            if (obj.posx + obj.radius / 2 + 1 >= main.Width || obj.posx - obj.radius / 2 - 1 <= 0)
            {
                obj.vh /= -2;
                if (obj.posx + obj.radius / 2 + 1 >= main.Width) obj.posx = main.Width - obj.radius / 2 - 1;
                else if (obj.posx - obj.radius / 2 - 1 <= 0) obj.posx = obj.radius / 2 + 1;
            }

                if (obj.posy + obj.radius / 2 + 1 < main.Height)
                {
                    falling(obj);
                }



                else
            {
                    /* timer.Enabled = false;
                     MessageBox.Show(Math.Abs(obj.vu).ToString());
                     timer.Enabled = true;*/

                    //  Ez működik jól
                    //  obj.v += Math.Abs(obj.vu);
                    //  obj.vu = 0;

                    circle.vu = Math.Abs(circle.vu) / -5; obj.v += obj.vu;

                    if (obj.v > obj.radius * 8)
                {

                    obj.v = obj.v / -4 * 3;
                    falling(obj);
                }


                else if (obj.v >= 0)
                {
                    obj.vh /= 2;
                    obj.v = 0;
                    obj.vu = 0;
                    if (obj.posy + obj.radius / 2 + 1 > main.Height)
                    {
                        obj.posy = main.Height - obj.radius / 2 - 1;
                        Graphics g = Graphics.FromImage(buffer);
                        obj.fall(g);
                        obj.radius = vScrollBar1.Value;

                    };
                }
                else falling(obj);
            }
        }
            else if (obj.posy + obj.radius / 2 + 1 < main.Height)
            {
                falling(obj);
            }
        }

        private void falling(GravityObject obj)
        {
            Graphics g = Graphics.FromImage(buffer);
            obj.fall(g);
            obj.radius = vScrollBar1.Value;
            g.DrawRectangle(new Pen(Brushes.Red), wall[0]);
            g.DrawRectangle(new Pen(Brushes.Red), wall[1]);




        }

        void Form1_Resize(object sender, EventArgs e)
        {
            main.Size = new Size(ClientSize.Width, ClientSize.Height - 30);
            buffer = new Bitmap(main.Width, main.Height);
            goal.Size = new Size(main.Width, 30);
            wall[0] = new Rectangle(main.Width-300, 450, 20, 20);
            wall[1] = new Rectangle(wall[0].Left + 240, wall[0].Top - 250, 20, 270);
            sensor.Location = wall[0].Location;
            sensor.Size = new Size(240, 20);

            playfield = new Rectangle(0, 0, main.Width, main.Height);
            line[0] = new Rectangle(main.Width / 2 - 15, 0, 30, main.Height - 30);
            line[1] = new Rectangle(0, 0, 30, main.Height - 30);
            line[2] = new Rectangle(main.Width - 30, 0, 30, main.Height - 30);
            line[3] = new Rectangle(0, main.Height - 30, main.Width, 30);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Draw(circle);
            
           
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Draw(circle);

            
        }

        private void Draw(GravityObject obj)
        {
            Graphics g = Graphics.FromImage(buffer);
            g.Clear(Color.DarkRed);
            g.FillEllipse(Brushes.White, main.Width / 2 - 50, main.Height / 2 - 50, 100, 100);
            g.FillRectangle(Brushes.White, line[0]);
            g.FillEllipse(Brushes.DarkRed, main.Width / 2 - 25, main.Height / 2 - 25, 50, 50);
            g.FillRectangle(Brushes.White, line[1]);
            g.FillRectangle(Brushes.White, line[2]);
            g.FillRectangle(Brushes.White, line[3]);

            obj.radius = vScrollBar1.Value;
            obj.RedrawCirle(g);

            g.FillRectangle(Brushes.White, wall[0]);
            g.DrawRectangle(new Pen(Brushes.Black), wall[0]);
            g.FillRectangle(Brushes.White, wall[1]);
            g.DrawRectangle(new Pen(Brushes.Black), wall[1]);

            g.FillRectangle(Brushes.Red, new Rectangle(wall[0].Left, wall[0].Top, 240, 20));
            g.DrawRectangle(new Pen(Brushes.Black), new Rectangle(wall[0].Left, wall[0].Top, 240, 20));


            main.Image = buffer;
            main.Refresh();
            fps++;
        }

        private void OnOff_Click(object sender, EventArgs e)
        {
            circle.v = 30;

            if (!timer.Enabled) { timer.Enabled = true; on = true; fpshelp.Enabled = true; }
            else { timer.Enabled = false; on = false; fpshelp.Enabled = true; }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Titlecard titlecard = new Titlecard();
            titlecard.Show();
            this.Close();
        }

        
    }
}
