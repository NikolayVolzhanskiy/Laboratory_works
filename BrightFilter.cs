using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Laboratory_work1
{
    internal class BrightFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            double k = 20;
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(
                Clamp((int)(sourceColor.R + k), 0, 255),
                Clamp((int)(sourceColor.G + k), 0, 255),
                Clamp((int)(sourceColor.B + k), 0, 255));
            return resultColor;
        }
    }
}
