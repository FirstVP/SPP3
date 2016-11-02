using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WPhotoEditor
{
    class BlueFilter: Filter
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
            ChannelOperations(ref B, percent);

            CheckChannel(ref B);

            return CreatePixel(R, G, B);
        }
    }
}
