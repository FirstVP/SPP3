using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WPhotoEditor
{
    class RedFilter : Filter
    {
        private void ChannelOperations(ref double channel, int percent)
        {
            channel = (channel + percent * 128 / 100);
        }

        protected override int CorrectPixel(int pixel, int percent)
        {
            Color color = Color.FromArgb(pixel);

            double R = color.R;
            double G = color.G;
            double B = color.B;
            ChannelOperations(ref R, percent);

            CheckChannel(ref R);

            return CreatePixel(R, G, B);
        }
    }
}
