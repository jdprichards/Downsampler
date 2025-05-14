using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Downsampler.Models;
using Downsampler.ViewModels;


namespace Downsampler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileModel _fileModel;

        public MainWindow()
        {
            InitializeComponent();

            FileViewModel viewModel = new FileViewModel();

            _fileModel = new FileModel
            {
                InputFile = "",
                InputFolder = "",
                OutputFolder = ""

            };

            viewModel.File = _fileModel;      
            
            MainDockPanel.DataContext = viewModel;
            tbImportFilePath.DataContext = viewModel;
            tbExportFolder.DataContext = viewModel;
            ProgressBars.DataContext = viewModel;

        }

        private void CheckBoxDownsampleFolder_Checked(object sender, RoutedEventArgs e)
        {
            btnImportSearchFile.Visibility = Visibility.Hidden;
            btnImportSearchFolder.Visibility = Visibility.Visible;
        }

        private void CheckBoxDownsampleFolder_Unchecked(object sender, RoutedEventArgs e) 
        {
            btnImportSearchFile.Visibility = Visibility.Visible;
            btnImportSearchFolder.Visibility = Visibility.Hidden;
        }


        public void clkExit(object  sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void clkImportSearchFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Select a File";

            //Show open folder dialog box
            bool? result = dialog.ShowDialog();

            _fileModel.InputFile = dialog.FileName;
        }

        private void clkImportSearchFolder(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFolderDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Select a Folder";

            //Show open folder dialog box
            bool? result = dialog.ShowDialog();

            _fileModel.InputFile = dialog.FolderName;
        }

        private void clkExportSearchFolder(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFolderDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Select Export Folder";
            bool? result = dialog.ShowDialog();
            _fileModel.OutputFolder = dialog.FolderName;
        }


        private async void clkDownsample(object sender, RoutedEventArgs e)
        {
            ProgressGrid.Visibility = Visibility.Visible;
            btnDownsample.IsEnabled = false;

            if (chkDownsampleFolder.IsChecked == false)
            {
                await Task.Run(async () => 
                {
                    Dispatcher.Invoke(() =>
                    {
                        _fileModel.FileComplete = 0;
                        _fileModel.FileAmount = 1;

                        _fileModel.FileCompleteRatio = _fileModel.FileComplete + "/" + _fileModel.FileAmount;
                        _fileModel.FilePercentage = ((float)_fileModel.FileComplete / (float)_fileModel.FileAmount) * 100.0f;
                        _fileModel.FilePercentageString = Math.Round(_fileModel.FilePercentage, 2) + "%";
                    });

                    await DownsampleFileAsync(_fileModel.InputFile);

                    Dispatcher.Invoke(() =>
                    {
                        _fileModel.FileComplete = 1;

                        _fileModel.FileCompleteRatio = _fileModel.FileComplete + "/" + _fileModel.FileAmount;
                        _fileModel.FilePercentage = ((float)_fileModel.FileComplete / (float)_fileModel.FileAmount) * 100.0f;
                        _fileModel.FilePercentageString = Math.Round(_fileModel.FilePercentage, 2) + "%";

                        btnDownsample.IsEnabled = true;
                    });
                });
                
            }

            else
            {
                await Task.Run(async() => 
                {
                    await DownsampleAsync();

                    Dispatcher.Invoke(() =>
                    {
                        btnDownsample.IsEnabled = true;
                    });
                });
            }

        }


        public async Task DownsampleAsync()
        {
            var files = Directory.GetFiles(_fileModel.InputFile);

            Dispatcher.Invoke(() =>
            {
                _fileModel.FileComplete = 0;
                _fileModel.FileAmount = files.Length;

                _fileModel.FileCompleteRatio = _fileModel.FileComplete + "/" + _fileModel.FileAmount;
                _fileModel.FilePercentage = ((float)_fileModel.FileComplete / (float)_fileModel.FileAmount) * 100.0f;
                _fileModel.FilePercentageString = Math.Round(_fileModel.FilePercentage, 2) + "%";
            });

            foreach (string file in files)
            {
                DownsampleFileAsync(file);

                Dispatcher.Invoke(() =>
                {
                    _fileModel.FileComplete++;

                    _fileModel.FileCompleteRatio = _fileModel.FileComplete + "/" + _fileModel.FileAmount;
                    _fileModel.FilePercentage = ((float)_fileModel.FileComplete / (float)_fileModel.FileAmount) * 100.0f;
                    _fileModel.FilePercentageString = Math.Round(_fileModel.FilePercentage, 2) + "%";
                });

                _fileModel.RowComplete = 0;
            }
        }

        public async Task DownsampleFileAsync(string file)
        {
            try
            {

                _fileModel.RowAmount = File.ReadLines(file).Count();

                StreamReader sr = new StreamReader(file);
                string line = sr.ReadLine();
                string filename = file.Split("\\").Last();
                filename = filename.Split(".").First();
                StreamWriter sw = new StreamWriter(_fileModel.OutputFolder + "\\" + filename + " (Downsampled).csv");
                sw.WriteLine(line);
                _fileModel.RowComplete++;

                float[] totals = new float[line.Split(",").Length - 1];

                int i = 1;
                int index = 0;
                int ratio = 40;

                string writeLine = "";

                line = sr.ReadLine();
                do
                {
                    string[] splitLine = line.Split(",");

                    for (int j = 1; j < splitLine.Length; j++)
                    {
                        totals[j - 1] += float.Parse(splitLine[j]);
                    }

                    if (i == ratio)
                    {
                        writeLine = index + ",";
                        for (int j = 0; j < totals.Length; j++)
                        {
                            float average = totals[j] / ratio;
                            writeLine += average;
                            if (j < totals.Length - 1)
                                writeLine += ",";

                        }

                        sw.WriteLine(writeLine);
                        i = 1;
                        index++;
                        totals = new float[totals.Length];
                        writeLine = "";
                    }
                    else
                    {
                        i++;
                    }

                    Dispatcher.Invoke(() =>
                    {
                        _fileModel.RowComplete++;

                        _fileModel.RowCompleteRatio = _fileModel.RowComplete + "/" + _fileModel.RowAmount;
                        _fileModel.RowPercentage = ((float)_fileModel.RowComplete / (float)_fileModel.RowAmount) * 100.0f;
                        _fileModel.RowPercentageString = Math.Round(_fileModel.RowPercentage, 2) + "%";
                    });


                    line = sr.ReadLine();
                } while (line != null);

                writeLine = index + ",";
                for (int j = 0; j < totals.Length; j++)
                {
                    float average = totals[j] / (i - 1);
                    writeLine += average;
                    if (j < totals.Length - 1)
                        writeLine += ",";
                }

                sw.WriteLine(writeLine);

                sw.Close();
                sr.Close();

                int testend = i;

            }
            catch (Exception ex)
            {
                Exception exception = ex;
            }
        }
    }
}