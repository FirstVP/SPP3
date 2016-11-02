using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WPhotoEditor
{
    abstract class Filter
    {
        protected const int minChannel = 0;
        protected const int maxChannel = 255;
        const uint alphaChannel = 0xFF000000;
        const int redChannelOffsetBits = 16;
        const int greenChannelOffsetBits = 8;

        protected void CheckChannel(ref double channel)
        {
            if (channel < minChannel)
                channel = minChannel;
            else 
            if (channel > maxChannel)
                channel = maxChannel;
        }

        protected int CreatePixel (double R, double G, double B)
        {           
            uint newPixel = alphaChannel | ((uint)R << redChannelOffsetBits) | ((uint)G << greenChannelOffsetBits) | ((uint)B);
            int pixel = (int)newPixel;
            return pixel;
        }

        public Bitmap Filtrate(int[,] imageMatrix, int percent)
        {
            Bitmap correctedImage = new Bitmap(imageMatrix.GetLength(0), imageMatrix.GetLength(1));
            for (int i = 0; i < imageMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < imageMatrix.GetLength(1); j++)
                {
                    correctedImage.SetPixel(i, j, Color.FromArgb(CorrectPixel(imageMatrix[i, j], percent)));
                }
            }
            return correctedImage;
        }

        abstract protected int CorrectPixel(int pixel, int percent);
    }
}
