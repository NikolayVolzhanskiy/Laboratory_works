using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Laboratory_work1
{
    public partial class Form1 : Form
    {
        int x1, x2, y1, y2, lineWidth, selectIndex;
        Bitmap image;
        Bitmap copyImage;
        bool mouseDown, chek;
        Bitmap snapshot;
        Bitmap tempDraw;
        Color color;
        string selectedTool;
        Point[] points = null;

        public Form1()
        {
            InitializeComponent();
            snapshot = new Bitmap(pictureBox1.ClientRectangle.Width, this.ClientRectangle.Height);
            color = Color.Black;
            lineWidth = 2;
            points = new Point[] {new Point(250, 500), new Point(400, 600), new Point(100, 400), new Point(800, 300)};
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image filter |*.png;*.jpg;*.bmp | All files(*.*) | *.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
                copyImage = image;
            }
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Image"; // Имя поумолчанию
            dialog.DefaultExt = ".jpg"; // Расширение файла поумолчанию
            dialog.Filter = "Image filter |*.png; *.jpg; *.bmp| All files (*.*) | *.*"; ; // Filter files by extension

            if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName.Length > 0)
            {
               using(StreamWriter sw = new StreamWriter(dialog.FileName, true))
                {
                    sw.WriteLine(pictureBox1.Image);
                    sw.Close();
                }
            }
        }

        private void сбросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            image = copyImage;
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        //  Отображение процесса обработки

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        //  2я лабораторная работа
        //  Изменения метода отображения изображения через ПКМ

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void stretchImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void centerImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        //  Рисование и изменение цвета

        private void цветToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog1.Color;
            }
        }

        //выбор 
        private void toolStripButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton btn in toolStrip1.Items)
            {
                btn.Checked = false;
            }

            ToolStripButton btnClicked = sender as ToolStripButton;
            btnClicked.Checked = true;
            selectedTool = btnClicked.Name;
            криваяБезьеToolStripMenuItem.Checked = false;
            кубическийСплайнToolStripMenuItem.Checked = false;
            chek = false;
        }

        //выбор толщины линии
        private void ptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripMenuItem itm in толщинаToolStripMenuItem.DropDownItems)
            {
                itm.Checked = false;
            }

            ToolStripMenuItem itmClicked = sender as ToolStripMenuItem;
            itmClicked.Checked = true;
            lineWidth = int.Parse(itmClicked.Text.Remove(1));
        }
        
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            x1 = e.X;
            y1 = e.Y;

            tempDraw = (Bitmap)snapshot.Clone();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            snapshot = (Bitmap)tempDraw.Clone();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (!chek) { // 2я лаба
                if (mouseDown)
                {
                    x2 = e.X;
                    y2 = e.Y;
                    pictureBox1.Refresh();
                }
            }
            else { // 5я лаба реакция на мышь
                for (int i = 0; i < points.Length; i++)
                {
                    if (Math.Abs(points[i].X - e.X) < 4)
                    {
                        if (Math.Abs(points[i].Y - e.Y) < 4)
                        {
                            selectIndex = i;
                            break;
                        }
                    }
                }

                if (MouseButtons.Left == e.Button)
                {
                    points[selectIndex] = e.Location;
                }

                pictureBox1.Refresh(); 
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            switch (selectedTool)
            {
                case "Pencil":
                    if (null != tempDraw)
                    {
                        Graphics g = Graphics.FromImage(tempDraw);
                        Pen myPen = new Pen(color, lineWidth);
                        g.DrawLine(myPen, x1, y1, x2, y2);
                        myPen.Dispose();
                        e.Graphics.DrawImageUnscaled(tempDraw, 0, 0);
                        g.Dispose();
                        x1 = x2;
                        y1 = y2;
                    }
                    break;

                case "Line":
                    if (null != tempDraw)
                    {
                        tempDraw = (Bitmap)snapshot.Clone();
                        Graphics g = Graphics.FromImage(tempDraw);
                        Pen myPen = new Pen(color, lineWidth);
                        g.DrawLine(myPen, x1, y1, x2, y2);
                        myPen.Dispose();
                        e.Graphics.DrawImageUnscaled(tempDraw, 0, 0);
                        g.Dispose();
                    }
                    break;

                case "Rectangle":
                    if (null != tempDraw)
                    {
                        tempDraw = (Bitmap)snapshot.Clone();
                        Graphics g = Graphics.FromImage(tempDraw);
                        Pen myPen = new Pen(color, lineWidth);
                        g.DrawRectangle(myPen, x1, y1, x2 - x1, y2 - y1);
                        myPen.Dispose();
                        e.Graphics.DrawImageUnscaled(tempDraw, 0, 0);
                        g.Dispose();
                    }
                    break;

                case "Ellipse":
                    if(null!= tempDraw)
                    {
                        tempDraw= (Bitmap)snapshot.Clone();
                        Graphics g = Graphics.FromImage(tempDraw);
                        Pen myPen = new Pen(color, lineWidth);
                        g.DrawEllipse(myPen, x1, y1, x2 - x1, y2 - y1);
                        myPen.Dispose();
                        e.Graphics.DrawImageUnscaled(tempDraw, 0, 0);
                        g.Dispose();
                    }
                    break;
                    // 5я лаба
                case "Bezier":
                    {
                        Graphics g = e.Graphics;
                        Pen myPen = new Pen(color, lineWidth);
                        Curves curves = new Curves(points);
                        curves.DrawSpline(g, selectIndex, myPen);
                        curves.DrawPoints(g, selectIndex);
                    }
                    break;

                case "Spline":
                    {
                        Graphics g = e.Graphics;
                        Pen myPen = new Pen(color, lineWidth);
                        Curves curves = new Curves(points);
                        curves.DrawBezier(g, selectIndex, myPen);
                        curves.DrawPoints(g, selectIndex);
                    }
                    break;

                default:
                    break;

            }
        }

        //  3я и 4я лабораторная работа. Фильтры

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void фильтрГаусаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void оттенкиСерогоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void яркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BrightFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new EmbossingFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        //5я лабораторная работа. Сплайны

        
        private void кубическийСплайнToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton btn in toolStrip1.Items)
            {
                btn.Checked = false;
            }
            selectedTool = "Bezier";
            кубическийСплайнToolStripMenuItem.Checked = true;
            криваяБезьеToolStripMenuItem.Checked = false;
            chek = true;
        }

        private void криваяБезьеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton btn in toolStrip1.Items)
            {
                btn.Checked = false;
            }
            selectedTool = "Spline";
            криваяБезьеToolStripMenuItem.Checked = true;
            кубическийСплайнToolStripMenuItem.Checked = false;
            chek = true;
        }
    }
}
