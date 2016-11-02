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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPhotoEditor
{
    /// <summary>
    /// Логика взаимодействия для ColorDialog.xaml
    /// </summary>
    public partial class ColorDialog : Window
    {
        ImageController imageController;
        public ColorDialog(ImageController imageController)
        {
            InitializeComponent();
            this.imageController = imageController;
        }

        private void ClrPckerEvent(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var mediaColor = ClrPcker.SelectedColor.Value;
            var drawingColor = System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
            imageController.Pen.Color = drawingColor;
        }
    }
}
