using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;

namespace WPhotoEditor
{
    public class ImageWrapper
    {
        Bitmap currentImage;
        Bitmap backupImage;

        public void Show(System.Windows.Controls.Image panel)
        {
            panel.Width = currentImage.Width;
            panel.Height = currentImage.Height;
            panel.Source = BitmapToBitmapSource(currentImage);


        }

        public void SetImage(Bitmap img, bool isBackup)
        {
            currentImage = img;
            if (isBackup)
            {
                backupImage = (Bitmap)img.Clone();
            }
            GC.Collect();
        }

        public Image GetImage()
        {
            return currentImage;
        }

        public Image GetBackupImage()
        {
            return backupImage;
        }

        internal int[,] GetBackupImageMatrix()
        {
            int[,] result = new int[backupImage.Width, backupImage.Height];
            for (int i = 0; i < backupImage.Width; i++)
            {
                for (int j = 0; j < backupImage.Height; j++)
                {
                    result[i, j] = backupImage.GetPixel(i, j).ToArgb();
                }
            }
            return result;
        }

        internal void RestoreImage()
        {
            SetImage(backupImage, true);
        }

        public static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            BitmapSource bitSrc = null;
            var hBitmap = source.GetHbitmap();
            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception e)
            {
                bitSrc = null;
                return null;
            }
            return bitSrc;
        }
    }
}
