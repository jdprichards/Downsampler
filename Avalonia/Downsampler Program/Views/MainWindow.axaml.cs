using Avalonia.Controls;
using Avalonia.Interactivity;

using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

using Downsampler.Models;
using Downsampler.ViewModels;

namespace Downsampler.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void CheckBoxDownsampleFolder_Checked(object sender, RoutedEventArgs e)
        {
            btnImportSearchFile.IsVisible = false;
            btnImportSearchFolder.IsVisible = true;
        }

        private void CheckBoxDownsampleFolder_Unchecked(object sender, RoutedEventArgs e)
        {
            btnImportSearchFile.IsVisible = true;
            btnImportSearchFolder.IsVisible = false;
        }

        private void clkExit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }

}