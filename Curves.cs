using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Laboratory_work1
{
    internal class Curves
    {
        private Point[] points = null;

        public Curves(Point[] _points){
			points = _points;
		}

		// Рисует кубический сплайн.
		public void DrawSpline(Graphics graphics, int selectIndex, Pen pen)
		{
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			//using (Pen pen = new Pen(Color.Blue, 2))
			{
				graphics.DrawCurve(pen, points);
			}
		}

		// Рисует кривую Безье
		public void DrawBezier(Graphics graphics, int selectIndex, Pen pen)
		{
			
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			using (Pen _pen = new Pen(Color.Gray, 1))
			{
				graphics.DrawLine(_pen, points[0], points[1]);
				graphics.DrawLine(_pen, points[2], points[3]);
			}

			
			//using (Pen pen = new Pen(Color.Blue, 2))
			{
				graphics.DrawBezier(pen,
									points[0], points[1],
									points[2], points[3]);
			}
		}

		// Рисует контрольные точки
		public void DrawPoints(Graphics graphics, int selectIndex)
		{
			
			int size = 5;
			for (int i = 0; i < points.Length; i++)
			{
				if (selectIndex == i)
				{
					using (Pen pen = new Pen(Color.Red, 3))
					{
						graphics.DrawEllipse(pen,
											 points[i].X - size / 2, points[i].Y - size / 2,
											 size, size);
					}
				}
				else
				{
					using (Pen pen = new Pen(Color.Green, 3))
					{
						graphics.DrawEllipse(pen,
											 points[i].X - size / 2, points[i].Y - size / 2,
											 size, size);
					}
				}
			}
		}
	}
}
