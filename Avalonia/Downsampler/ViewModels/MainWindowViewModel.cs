using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Downsampler.Models;
using Downsampler.Views;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Downsampler.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowModel windowModel { get; set; } = new MainWindowModel();

        
        public async Task<string> OpenOSDialog_ReturnString(bool boolFile)
        {
            
            var topLevel = App.TopLevel;

            if (topLevel != null && topLevel.StorageProvider.CanOpen)
            {
                if (boolFile)
                {
                    var file = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                    {
                        AllowMultiple = false,
                        Title = "Select a File",

                    });

                    // Will always have only one file

                    if (file[0] != null)
                        return file[0].TryGetLocalPath();
                }
                else
                {

                    var file = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        AllowMultiple = false,
                        Title = "Select a Folder",

                    });

                    if (file[0] != null)
                        return file[0].TryGetLocalPath();
                }
            }

            return "";
        }

        public async void ClkImportSearchFile()
        {
            try
            {
                await Task.Run(() => windowModel.InputFile = OpenOSDialog_ReturnString(true).Result);
            }
            catch (Exception ex) { }
        }

        public async void ClkImportSearchFolder()
        {
            try 
            {
                await Task.Run(() => windowModel.InputFile = OpenOSDialog_ReturnString(false).Result);
            }
            catch (Exception ex) { }
        }

        public async void ClkExportSearchFolder()
        {
            try
            {
                await Task.Run(() => windowModel.OutputFolder = OpenOSDialog_ReturnString(false).Result);
            }
            catch(Exception ex) { }
        }

        public async Task DownsampleAsync()
        {
            string[] files = [null];
            if (windowModel.InputFile.EndsWith(".csv"))
                files[0] = windowModel.InputFile;
            else
                files = Directory.GetFiles(windowModel.InputFile);

            windowModel.DisableDownsample = true;
            windowModel.HideProgress = false;

            Dispatcher.UIThread.Invoke(() =>
            {
                windowModel.FileComplete = 0;
                windowModel.FileAmount = files.Length;

                windowModel.FileCompleteRatio = windowModel.FileComplete + "/" + windowModel.FileAmount;
                windowModel.FilePercentage = ((float)windowModel.FileComplete / (float)windowModel.FileAmount) * 100.0f;
                windowModel.FilePercentageString = Math.Round(windowModel.FilePercentage, 2) + "%";
            });

            foreach (string file in files)
            {
                await Task.Run(()=> DownsampleFileAsync(file));

                Dispatcher.UIThread.Invoke(() =>
                {
                    windowModel.FileComplete++;

                    windowModel.FileCompleteRatio = windowModel.FileComplete + "/" + windowModel.FileAmount;
                    windowModel.FilePercentage = ((float)windowModel.FileComplete / (float)windowModel.FileAmount) * 100.0f;
                    windowModel.FilePercentageString = Math.Round(windowModel.FilePercentage, 2) + "%";
                });

                windowModel.RowComplete = 0;
            }

            windowModel.DisableDownsample = false;
        }

        public async Task DownsampleFileAsync(string file)
        {
            try
            {
                windowModel.RowAmount = File.ReadLines(file).Count();

                StreamReader sr = new StreamReader(file);
                string line = sr.ReadLine();
                string filename = file.Split("\\").Last();
                filename = filename.Split(".").First();
                StreamWriter sw = new StreamWriter(windowModel.OutputFolder + "\\" + filename + " (Downsampled).csv");
                sw.WriteLine(line);
                windowModel.RowComplete++;

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

                    Dispatcher.UIThread.Invoke(() =>
                    {
                        windowModel.RowComplete++;

                        windowModel.RowCompleteRatio = windowModel.RowComplete + "/" + windowModel.RowAmount;
                        windowModel.RowPercentage = ((float)windowModel.RowComplete / (float)windowModel.RowAmount) * 100.0f;
                        windowModel.RowPercentageString = Math.Round(windowModel.RowPercentage, 2) + "%";
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
