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
    /// Логика взаимодействия для BrightSettingsWindow.xaml
    /// </summary>
    public partial class BrightSettingsWindow : Window
    {
        private MainWindow mainWindow;
        int[,] currentImageMatrix;
        int chosenFilterIndex = 0;
        readonly Filter[] currentFilters = { new BrightFilter(), new ContrastFilter(), new BWFilter() };

        public BrightSettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            sldBright.Tag = "Яркость: ";
            sldContrast.Tag = "Контрастность: ";
        }

        private void UpdateAllScrolls()
        {
            UpdateScroll(sldBright, lblBright);
            UpdateScroll(sldContrast, lblContrast);
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

        private void sldBright_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldContrast.Value = 0;
            UpdateAllScrolls();
            FiltrateImage(0, sldBright, lblBright, false);
        }

        private void sldContrast_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sldBright.Value = 0;
            UpdateAllScrolls();
            FiltrateImage(1, sldContrast, lblContrast, false);
        }

        private void btnBright_Click(object sender, RoutedEventArgs e)
        {
            sldContrast.Value = 0;
            FiltrateImage(0, sldBright, lblBright, true);
            sldBright.Value = 0;
            UpdateAllScrolls();
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            mainWindow.RestoreImage();
            mainWindow.ShowImage();
        }

        private void btnContrast_Click(object sender, RoutedEventArgs e)
        {
            sldBright.Value = 0;
            FiltrateImage(1, sldContrast, lblContrast, true);
            sldContrast.Value = 0;
            UpdateAllScrolls();
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            Apply();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            sldBright.Value = 0;
            sldContrast.Value = 0;
            UpdateScroll(sldBright, lblBright);
            UpdateScroll(sldContrast, lblContrast);
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

        private void btnBW_Click(object sender, RoutedEventArgs e)
        {
            sldBright.Value = 0;
            sldContrast.Value = 0;
            FiltrateImage(2, sldContrast, lblContrast, true);
            UpdateAllScrolls();
            currentImageMatrix = mainWindow.GetBackupImageMatrix();
            Apply();
        }
    }
}
