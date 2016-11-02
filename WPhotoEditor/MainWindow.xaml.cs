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
        BrightSettingsWindow bsWindow;
        ColorSettingsWindow clrWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

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
            if (imageWrapper.GetImage() != null)
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
        }

        private void ColorDialog_Click(object sender, RoutedEventArgs e)
        {
            cd = new ColorDialog(imageController);
            cd.ShowDialog();
        }

        private void Redo_item_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mainImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imageController.previousPoint = new System.Drawing.Point(0, 0);
        }

        public void ResetImageDiplay()
        {
            var hBmp = ((System.Drawing.Bitmap)imageWrapper.GetImage()).GetHbitmap();
            try
            {         
                var options = BitmapSizeOptions.FromEmptyOptions();
                mainImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBmp,
                    IntPtr.Zero, Int32Rect.Empty, options); ;
            }
            finally
            {
                DeleteObject(hBmp);
            }
        }

        private void mainImage_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    if (imageWrapper.GetImage() == null)
                        throw new Exception("Не открыта картинка!");
                    System.Drawing.Point mousePoint = new System.Drawing.Point((int)e.GetPosition(mainImage).X, 
                        (int)e.GetPosition(mainImage).Y);
                    imageController.Draw(imageWrapper.GetImage(), mousePoint);

                    ResetImageDiplay();
                    mainImage.InvalidateMeasure();
                    mainImage.InvalidateVisual();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Small_Click(object sender, RoutedEventArgs e)
        {
            imageController.ChooseBrushSize(0);
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            imageController.ChooseBrushSize(1);
        }

        private void Thick_Click(object sender, RoutedEventArgs e)
        {
            imageController.ChooseBrushSize(2);
        }

        private void Rotate90_Click(object sender, RoutedEventArgs e)
        {
            RotateImage(0);
        }

        public void RotateImage(byte mode)
        {
            try
            {
                if (imageWrapper.GetImage() == null)
                    throw new Exception("Не открыта картинка!");
                imageController.RotateImage(imageWrapper, mode);
                ShowImage();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Rotate180_Click(object sender, RoutedEventArgs e)
        {
            RotateImage(1);
        }

        private void Rotate270_Click(object sender, RoutedEventArgs e)
        {
            RotateImage(2);
        }

        public void RestoreImage()
        {
            imageWrapper.RestoreImage();
        }

        public int[,] GetBackupImageMatrix()
        {
            return imageWrapper.GetBackupImageMatrix();
        }

        private void mainImage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                if (imageWrapper.GetImage() == null)
                    throw new Exception("Не открыта картинка!");
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    imageWrapper.SetImage(imageController.ResizeImage(imageWrapper, e.Delta), true);
                    ShowImage();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void BrightSettings_Click(object sender, RoutedEventArgs e)
        {           
            try
            {
                if (imageWrapper.GetImage() == null)
                    throw new Exception("Не открыта картинка!");
                bsWindow = new BrightSettingsWindow(this);
                bsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void ColorSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imageWrapper.GetImage() == null)
                    throw new Exception("Не открыта картинка!");
                clrWindow = new ColorSettingsWindow(this);
                clrWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
