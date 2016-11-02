using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WPhotoEditor
{
    class BWFilter : Filter
    {
        double gamma = 2.2;
        double bwRedTransforation = 0.2989;
        double bwGreenTransforation = 0.5870;
        double bwBlueTransforation = 0.1140;

        protected override int CorrectPixel(int pixel, int percent)
        {
            Color color = Color.FromArgb(pixel);
            double R = color.R;
            double G = color.G;
            double B = color.B;

            double bWChannel = 
                bwRedTransforation * R + bwGreenTransforation * G + bwBlueTransforation * B;

            R = bWChannel;
            G = bWChannel;
            B = bWChannel;

            CheckChannel(ref R);
            CheckChannel(ref G);
            CheckChannel(ref B);

            return CreatePixel (R, G, B);
        }
    }
}
