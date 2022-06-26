using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Laboratory_work1
{
    internal class SepiaFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            double k = 20;
            Color sourceColor = sourceImage.GetPixel(x, y);
            int Intensity = (int)(0.299 * sourceColor.R + 0.587 * sourceColor.G + 0.114 * sourceColor.B);
            Color resultColor = Color.FromArgb(
                Clamp((int)(Intensity + 3 * k), 0, 255), 
                Clamp((int)(Intensity + 0.9 * k), 0, 255),
                Clamp((int)(Intensity - 1 * k), 0, 255));
            return resultColor;
        }
    }
}
