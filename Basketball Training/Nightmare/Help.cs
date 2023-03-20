using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Help : Form
    {
        

        private void Help_Load(object sender, EventArgs e)
        {

        }
        PictureBox main = new PictureBox();
        Bitmap buffer;
        Timer timer = new Timer();
        GravityObject circle = new GravityObject();
        Label goal = new Label();
        Timer help = new Timer();
        Timer push = new Timer();
        Random rand = new Random();
        Rectangle playfield = new Rectangle();
        Rectangle[] line = new Rectangle[4];
        public Help()
        {
            InitializeComponent();
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            main.Location = new Point(0, 30);

            push.Interval = 1000;
            push.Tick += Push_Tick;

            /*  circle.posx = 600;
              circle.posy = 200;
              circle.radius = 200;*/

            Controls.Add(main);
            Resize += new EventHandler(Form1_Resize);
            timer.Interval = 10;
            timer.Tick += new EventHandler(timer_Tick);


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
            goal.Font = new Font(new FontFamily("Arial"), 22, FontStyle.Bold);
            Controls.Add(goal);

            circle.v = 30;

            timer.Enabled = true;
            push.Enabled = true;

            circle.v += rand.Next(10, 1000) * (1 - rand.Next(0, 3));
            circle.vh += rand.Next(10, 100) * (1 - rand.Next(0, 3));

        }

        private void Push_Tick(object sender, EventArgs e)
        {
            if (circle.v == 0) circle.posy = rand.Next(300, 1500);

            if (circle.v < 0 && circle.posy > 200)
                circle.v -= rand.Next(10000, 20000);
            circle.vh += rand.Next(300, 500) * (1 - rand.Next(0, 3));

        }

        



        void timer_Tick(object sender, EventArgs e)
        {

            gravity(circle);
            Draw(circle);


        }
        private void gravity(GravityObject obj)
        {
            if (!(playfield.Contains(circle.hitboxy) && playfield.Contains(circle.hitboxx)))
            {


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
                            obj.radius = 151;

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
            obj.radius = 151;




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

            obj.radius = 151;
            obj.RedrawCirle(g);



            main.Image = buffer;
            main.Refresh();
        }

        void Form1_Resize(object sender, EventArgs e)
        {
            main.Size = new Size(ClientSize.Width, ClientSize.Height - 30);
            buffer = new Bitmap(main.Width, main.Height);
            goal.Size = new Size(ClientSize.Width, 30);

            playfield = new Rectangle(0, 0, main.Width, main.Height);
            line[0] = new Rectangle(main.Width / 2 - 15, 0, 30, main.Height - 30);
            line[1] = new Rectangle(0, 0, 30, main.Height - 30);
            line[2] = new Rectangle(main.Width - 30, 0, 30, main.Height - 30);
            line[3] = new Rectangle(0, main.Height - 30, main.Width, 30);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Titlecard titlecard = new Titlecard();
            titlecard.Show();
            this.Close();
        }
    }
}
