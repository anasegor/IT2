using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadanie2_IT
{
    public partial class Form1 : Form
    {
        private double v1, v2, v3;
        private double f_d;
        private int N,N1,N2;
        private int N12, N22;
        private int padding = 10;
        private int left_keys_padding = 20;
        private PointF[] func_points;
        private PointF[] diff;
        private double[] pred;
        private double t;
        private int T;//количество отсчетов окна
        private PointF[] oursill;//порог
        private PointF[] convolut;//свертка
        private void Form1_Load(object sender, EventArgs e)
        {
           
            for_v1.Text = "3";
            for_v2.Text = "13";
            for_v3.Text = "6";
            for_fd.Text = "150";
            for_N.Text = "1000";
            for_N1.Text = "200";
            for_N2.Text = "600";
            for_sill.Text = "0,05";
            for_T.Text = "50";
        }



        public Form1()
        {
            InitializeComponent();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox2.Image = null;
            pictureBox2.Update();
            int wX1, hX1;
            Graphics graphics1 = pictureBox1.CreateGraphics();
            wX1 = pictureBox1.Width;
            hX1 = pictureBox1.Height;
            int wX2, hX2;
            Graphics graphics2 = pictureBox2.CreateGraphics();
            wX2 = pictureBox2.Width;
            hX2 = pictureBox2.Height;
            Pen pen = new Pen(Color.DarkRed, 2f);
            Pen pen1 = new Pen(Color.Black, 2f);
            Pen pen2 = new Pen(Color.Blue, 2f);
            //Ось X
            Point KX1, KX2;
            KX1 = new Point(30, (hX1 / 2));
            KX2 = new Point(wX1 - 10, (hX1 / 2));
            graphics1.DrawLine(pen1, KX1, KX2);
            Point KX11, KX21;
            KX11 = new Point(30, (hX2 / 2));
            KX21 = new Point(wX2- 10, (hX2 / 2));
            graphics2.DrawLine(pen1, KX11, KX21);

            //Ось Y
            Point KY1, KY2;
            KY1 = new Point(30, 10);
            KY2 = new Point(30, hX1 - 10);
            graphics1.DrawLine(pen1, KY1, KY2);
            Point KY11, KY21;
            KY11 = new Point(30, 10);
            KY21 = new Point(30, hX2 - 10);
            graphics2.DrawLine(pen1, KY11, KY21);
            //сетка
            int actual_width1 = wX1 - 2 * padding - left_keys_padding;
            int actual_height1 = hX1 - 2 * padding;
            int actual_width2 = wX2 - 2 * padding - left_keys_padding;
            int actual_height2 = hX2 - 2 * padding;
            int actual_top = padding;
            int actual_bottom1 = actual_top + actual_height1;
            int actual_left = padding + left_keys_padding;
            int actual_right1 = actual_left + actual_width1;
            int actual_bottom2 = actual_top + actual_height2;
            int actual_right2 = actual_left + actual_width2;
            Pen GridPen = new Pen(Color.Gray, 1f);
            int grid_size = 11;
            float maxY = 0;
            GenerateSignal();
            for (int i = 0; i < N; i++)
            {
                if (maxY < Math.Abs(func_points[i].Y)) maxY = Math.Abs(func_points[i].Y);//макс значение Y
            }
            PointF K1, K2, K3, K4;
            PointF K5, K6, K7, K8;
            for (double i = 0.5; i < grid_size; i += 1.0)
            {
                //вертикальная
                K1 = new PointF((float)(actual_left + i * actual_width1 / grid_size), actual_top);
                K2 = new PointF((float)(actual_left + i * actual_width1 / grid_size), actual_bottom1);
                graphics1.DrawLine(GridPen, K1, K2);
                K5 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_top);
                K6 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_bottom2);
                graphics2.DrawLine(GridPen, K5, K6);
                double v = 0 + i * ((double)(N / f_d) - 0) / grid_size;
                string s1 = v.ToString("0.00");
                graphics1.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width1 / grid_size, actual_bottom1 + 0);
                graphics2.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width2 / grid_size, actual_bottom2 + 0);
                //горизонатальная
                K3 = new PointF(actual_left, (float)(actual_top + i * actual_height1 / grid_size));
                K4 = new PointF(actual_right1, (float)(actual_top + i * actual_height1 / grid_size));
                K7 = new PointF(actual_left, (float)(actual_top + i * actual_height2 / grid_size));
                K8 = new PointF(actual_right2, (float)(actual_top + i * actual_height2 / grid_size));
                double g = 0 + i * (double)(maxY / grid_size);
                string s2 = g.ToString("0.00");
                graphics1.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding, actual_bottom1 - (float)i * actual_height1 / grid_size - hX1 / 2);//????
                graphics1.DrawLine(GridPen, K3, K4);
                graphics2.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding, actual_bottom2 - (float)i * actual_height2 / grid_size - hX2 / 2);//????
                graphics2.DrawLine(GridPen, K7, K8);
            }
            PointF actual_tb = new PointF(actual_top, actual_bottom1);//для y
            PointF actual_rl = new PointF(actual_right1, actual_left);//для x
            PointF actual_tb2 = new PointF(actual_top, actual_bottom2);//для y
            PointF actual_rl2 = new PointF(actual_right2, actual_left);//для x
            PointF from_toX = new PointF(0, (float)(N / f_d));
            PointF from_toY = new PointF(-maxY * (float)1.2, maxY * (float)1.2);
            float maxY1=0;
            Diffs();
            LPFilter();
            SearchBorder();
            for_N1_2.Text = N12.ToString();
            for_N2_2.Text = N22.ToString();
            for (int i = 0; i < N; i++)
            {
                if (maxY1 < Math.Abs(diff[i].Y)) maxY1 = Math.Abs(diff[i].Y);//макс значение Y
            }
            PointF from_toY1 = new PointF(-maxY1 * (float)1.2, maxY1* (float)1.2);
            convert_range_graph(ref func_points, actual_rl, actual_tb, from_toX, from_toY);
            graphics1.DrawLines(pen1, func_points);
            convert_range_graph(ref diff, actual_rl2, actual_tb2, from_toX, from_toY1);
            convert_range_graph(ref oursill, actual_rl2, actual_tb2, from_toX, from_toY1);
            convert_range_graph(ref  convolut, actual_rl2, actual_tb2, from_toX, from_toY1);
            graphics2.DrawLines(pen1, diff);
            graphics2.DrawLines(pen, oursill);
            graphics2.DrawLines(pen2, convolut);
        }
        public void GenerateSignal()
        {
            if (for_N1.Text != "" || for_N2.Text != "")
            {
                N1 = Convert.ToInt32(for_N1.Text);
                N2 = Convert.ToInt32(for_N2.Text);
            }
            else { MessageBox.Show("параметры N1,N2 по умолчанию", "Внимание!"); }
            if (for_v1.Text != "" || for_v2.Text != "" || for_v3.Text != "")
            {
                v1 = Convert.ToDouble(for_v1.Text);
                v2 = Convert.ToDouble(for_v2.Text);
                v3 = Convert.ToDouble(for_v3.Text);
            }
            else { MessageBox.Show("параметры v по умолчанию", "Внимание!"); }
            
            if (for_fd.Text != "" || for_N.Text != "")
            {
                f_d = Convert.ToDouble(for_fd.Text);
                N = Convert.ToInt32(for_N.Text);
                T = Convert.ToInt32(for_T.Text);
                t = Convert.ToDouble(for_sill.Text);

            }
            else { MessageBox.Show("параметры f_d,N,порога и окна по умолчанию", "Внимание!"); }
            func_points = new PointF[N];
            oursill = new PointF[N];
            float dt = (float)(1 / f_d);
            for (int i = 0; i < N1; i++) 
            {
                func_points[i]=new PointF((float)i * dt, (float)Math.Sin(2 * Math.PI * v1 * (float)i*dt + 0) );
                oursill[i]= new PointF((float)i * dt, (float)t);//порог
            }
            double phi2 = 2 * Math.PI * N1 * (v1 - v2) / f_d;
            for (int i = N1; i < N2; i++)
            {
                func_points[i] = new PointF((float)i * dt, (float)Math.Sin(2 * Math.PI * v2 * (float)i * dt + (float)phi2));
                oursill[i] = new PointF((float)i * dt, (float)t);//порог
            }
            double phi3 = 2 * Math.PI * N2 * (v2 - v3) / f_d + phi2;
            for (int i = N2; i < N; i++)
            {
                func_points[i] = new PointF((float)i * dt, (float)Math.Sin(2 * Math.PI * v3 * (float)i * dt + (float)phi3));
                oursill[i] = new PointF((float)i * dt, (float)t);//порог
            }
        }
        public void Diffs()
        {
            double w = 2 * Math.PI * v2;
            double a1=(-2 * Math.Cos(w / f_d));
            diff = new PointF[N];
            pred = new double[N];
            pred[0]= func_points[0].Y;
            diff[0]= new PointF(func_points[0].X, (float)Math.Abs(func_points[0].Y - pred[0]));
            pred[1] = (-func_points[1].Y * a1);
            diff[1] = new PointF(func_points[1].X,(float)Math.Abs(func_points[1].Y - pred[1]));
            for (int i = 2; i < N; i++)
            {
                double temp = -a1 * func_points[i-1].Y - func_points[i - 2].Y;
                pred[i]=temp;
                diff[i]= new PointF(func_points[i].X, (float)Math.Abs(pred[i] - func_points[i].Y));
            }
        }
        public void LPFilter()
        {
            List<double> arr = new List<double>();
            List<double> it = new List<double>();
            double temp=0;
            for (int i = (int)(T/ 2); i < (N - T / 2); i++)
            {
                for (int j = (int)(i - T / 2); j < (i + T/ 2); j++)
                {
                    temp += 1*diff[j].Y;
                }
                arr.Add(temp/T);
                it.Add(func_points[i].X);
                temp = 0;
            }
            convolut = new PointF[arr.Count];
            for(int i=0;i<arr.Count;i++)
            {
                convolut[i] = new PointF((float)it[i],(float)arr[i] );
            }
        }
        public void SearchBorder()
        {
            bool first=false;
            for (int i = 0; i < N - T; i++)
            {
                if (convolut[i].Y <= t && first == false)//если меньше порога 
                {
                    first = true;
                    N12 = i + T / 2;
                }
                if (convolut[i].Y <= t && first == true)
                {
                    N22 = i + T / 2;
                }
            }
        }

        public void convert_range_graph(ref PointF[] data, PointF actual_rl, PointF actual_tb, PointF from_toX, PointF from_toY)
        {
            //actual-размер:X-top/right Y-right,left
            //from_to: X-мин, Y-макс
            float kx = (actual_rl.X - actual_rl.Y) / (from_toX.Y - from_toX.X);
            float ky = (actual_tb.X - actual_tb.Y) / (from_toY.Y - from_toY.X);
            for (int i = 0; i < data.Length; i++)
            {
                data[i].X = (data[i].X - from_toX.X) * kx + actual_rl.Y;
                data[i].Y = (data[i].Y - from_toY.X) * ky + actual_tb.Y;
            }
        }
    }
}
