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
    /// Логика взаимодействия для ColorSettingsWindow.xaml
    /// </summary>
    public partial class ColorSettingsWindow : Window
    {
        private MainWindow mainWindow;
        int[,] currentImageMatrix;
        int chosenFilterIndex = 0;
        readonly Filter[] currentFilters = { new RedFilter(), new GreenFilter(), new BlueFilter() };

        public ColorSettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            sldR.Tag = "Красный: ";
            sldG.Tag = "Зелёный: ";
            sldB.Tag = "Синий: ";
        }

        private void UpdateAllScrolls()
        {
            UpdateScroll(sldR, lblR);
            UpdateScroll(sldG, lblG);
            UpdateScroll(sldB, lblB);
        }

        private void UpdateScroll(Slider sb, Label lbl)
        {
            lbl.Content = $"{(string)sb.Tag}{((int)sb.Value).ToString()}";
        }

        private void FiltrateImage(int index, Slider tb, Label lbl, bool isSaved)
        {
            chosenFilterIndex = index;
            UpdateScroll(tb, lbl);
            System.Drawing.Bitmap correctedImage = currentFilters[index].Filtrate(currentImageMatrix, (int)tb.Value);
            mainWindow.SetImage(correctedImage, isSaved);
            mainWindow.ShowImage();
        }

        private void sldR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldG.Value = 0;
            sldB.Value = 0;
            UpdateAllScrolls();
            FiltrateImage(0, sldR, lblR, false);
        }

        private void sldG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldR.Value = 0;
            sldB.Value = 0;
            UpdateAllScrolls();
            FiltrateImage(1, sldG, lblG, false);
        }

        private void sldB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldR.Value = 0;
            sldG.Value = 0;
            UpdateAllScrolls();
            FiltrateImage(2, sldB, lblB, false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            sldR.Value = 0;
            sldB.Value = 0;
            sldG.Value = 0;
            UpdateAllScrolls();
        }

        private void Apply()
        {
            mainWindow.RestoreImage();
            mainWindow.ShowImage();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Apply();
        }

        private void btnR_Click(object sender, RoutedEventArgs e)
        {
            sldG.Value = 0;
            sldB.Value = 0;
            FiltrateImage(0, sldR, lblR, true);
            sldR.Value = 0;
            UpdateAllScrolls();
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            Apply();
        }

        private void btnG_Click(object sender, RoutedEventArgs e)
        {
            sldR.Value = 0;
            sldB.Value = 0;
            FiltrateImage(1, sldG, lblG, true);
            sldG.Value = 0;
            UpdateAllScrolls();
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            Apply();
        }

        private void btnB_Click(object sender, RoutedEventArgs e)
        {
            sldR.Value = 0;
            sldG.Value = 0;
            FiltrateImage(2, sldB, lblB, true);
            sldB.Value = 0;
            UpdateAllScrolls();
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            Apply();
        }
    }
}
