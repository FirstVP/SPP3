using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using Xceed.Wpf.Toolkit;

namespace WPhotoEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImageWrapper imageWrapper = new ImageWrapper();
        ImageController imageController = new ImageController();
        ColorDialog cd;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            ShowOpenDialog();    
        }

        private void ShowOpenDialog()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Открыть";
            openDialog.CheckPathExists = true;
            openDialog.Filter = "Файлы изображений |*.bmp;*.jpg;*.gif;*.png";
            openDialog.FilterIndex = 1;

            if (openDialog.ShowDialog() == true)
            {                                
                Title = openDialog.FileName;
                try
                {
                    System.Drawing.Bitmap currentPicture = new System.Drawing.Bitmap(openDialog.FileName);
                    SetImage(currentPicture, true);
                    ShowImage();
                }
                catch
                {
                    System.Windows.MessageBox.Show("Ошибка открытия файла");
                }
            }

        }

        public void ShowImage()
        {
            try
            {
                if (imageWrapper.GetImage() == null)
                    throw new Exception("Не открыта картинка!");
                imageWrapper.Show(mainImage);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void SetImage(System.Drawing.Bitmap image, bool isNew)
        {
            imageWrapper.SetImage(image, isNew);
        }   

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ShowSaveDialog();
        }

        private void ShowSaveDialog()
        {
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить как...";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Файлы изображений | *.bmp; *.jpg; *.gif; *.png";
            if (savedialog.ShowDialog() == true)
            {
                try
                {
                    imageWrapper.GetImage().Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Ошибка сохранения файла");
                }
            }
        }

        private void ColorDialog_Click(object sender, RoutedEventArgs e)
        {
            cd = new ColorDialog(imageController);
            cd.ShowDialog();
        }

        private void Redo_item_Click(object sender, RoutedEventArgs e)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext dc = drawingVisual.RenderOpen();
            Rect rect = new Rect(0, 0, imageWrapper.GetImage().Width, imageWrapper.GetImage().Height);
            dc.DrawImage(Bitmap2BitmapImage((System.Drawing.Bitmap)imageWrapper.GetImage()), rect);
            dc.DrawLine(new Pen(Brushes.Yellow, 4), new Point(0, 0), new Point(100, 100));
            dc.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, 4), new Rect(10, 10, 100, 100));
            dc.Close();
            mainImage.Source = new DrawingImage(drawingVisual.Drawing);
          

           // System.Drawing.Image img = ImageWpfToGDI(mainImage);
           // img.Save("111.jpg");
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private BitmapImage Bitmap2BitmapImage(System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }

        private System.Drawing.Image ImageWpfToGDI(Image image)
        {
            MemoryStream ms = new MemoryStream();
            var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image.Source as System.Windows.Media.Imaging.BitmapSource));
            encoder.Save(ms);
            ms.Flush();
            return System.Drawing.Image.FromStream(ms);
        }

    }
}
