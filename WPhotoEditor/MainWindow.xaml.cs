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
using System.Drawing;
using Microsoft.Win32;

namespace WPhotoEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            OpenFileDialog openFileDialog = new OpenFileDialog();           
            openFileDialog.Filter = "Файлы изображений |*.bmp;*.jpg;*.gif;*.png";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == true)
            {
                Bitmap currentPicture = new Bitmap(openFileDialog.FileName);                
                mainImage.Width = currentPicture.Width;
                mainImage.Height = currentPicture.Height;
                mainImage.Source = BitmapToBitmapSource(currentPicture);

                MainShaderEffect effect = new MainShaderEffect();
                effect.Brightness = 0.5;
                mainImage.Effect = effect;

                // addPicture(currentPicture);            
                Title = openFileDialog.FileName;

            }
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
                    BitmapSizeOptions.FromWidthAndHeight (source.Width, source.Height));
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
