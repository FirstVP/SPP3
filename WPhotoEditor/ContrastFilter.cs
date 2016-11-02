using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WPhotoEditor
{
    class ContrastFilter: Filter
    {
        const double constrastParameter = 100.0;

        private void ChannelOperations (ref double channel, double contrast)
        {
            channel /= 255.0; 
            channel -= 0.5;
            channel *= contrast;
            channel += 0.5;
            channel *= 255;
        }

        protected override int CorrectPixel(int pixel, int percent)
        {
            Color color = Color.FromArgb(pixel);
            double contrastPower = (constrastParameter + (percent+1)) / constrastParameter;
            contrastPower *= contrastPower;

            double R = color.R;
            double G = color.G;
            double B = color.B;
            ChannelOperations(ref R, contrastPower);
            ChannelOperations(ref G, contrastPower);
            ChannelOperations(ref B, contrastPower);

            CheckChannel(ref R);
            CheckChannel(ref G);
            CheckChannel(ref B);

            return CreatePixel (R, G, B);
        }
    }
}
