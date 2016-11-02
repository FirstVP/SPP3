using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace WPhotoEditor
{
    public class ImageController
    {
        const double resizePower = 0.01;

        public Point previousPoint = new Point(0, 0);
        public Pen Pen { get; set; }

        int[] PenSize { get; } = new int[] { 20, 40, 100 };

        public ImageController()
        {
            Pen = new Pen(Color.Black, PenSize[0]);
            Pen.EndCap = LineCap.Round;
            Pen.StartCap = LineCap.Round;
        }

        public Bitmap ResizeImage(ImageWrapper imageWrapper, int direction)
        {
            Image image = imageWrapper.GetImage();
            double scale = 1.0;
            scale += Math.Sign(direction) * resizePower;
            int width = Convert.ToInt32(Math.Round(image.Width * scale));
            int height = Convert.ToInt32(Math.Round(image.Height * scale));

            var resizedRect = new Rectangle(0, 0, width, height);
            var resizedImage = new Bitmap(width, height);

            resizedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, resizedRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return resizedImage;

        }

        public void RotateImage(ImageWrapper imageWrapper, byte typeNumber)
        {
            RotateFlipType[] rotateTypesSet = { RotateFlipType.Rotate90FlipNone, RotateFlipType.Rotate180FlipNone, RotateFlipType.Rotate270FlipNone };
            imageWrapper.GetImage().RotateFlip(rotateTypesSet[typeNumber]);
            imageWrapper.GetBackupImage().RotateFlip(rotateTypesSet[typeNumber]);
        }

        public void ChooseBrushSize(int size)
        {
            Pen.Width = PenSize[size];
        }

        internal void Draw(Image image, Point mousePoint)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                if ((previousPoint.X == 0) && (previousPoint.Y == 0))
                {
                    previousPoint.X = mousePoint.X;
                    previousPoint.Y = mousePoint.Y;
                }
                g.DrawLine(Pen, previousPoint, mousePoint);
                previousPoint.X = mousePoint.X;
                previousPoint.Y = mousePoint.Y;
            }
        }
    }
}
