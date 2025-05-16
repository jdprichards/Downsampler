using CommunityToolkit.Mvvm.ComponentModel;
using Downsampler.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Downsampler.Models
{
    public class MainWindowModel : ObservableObject
    {
        private string _inputFile = "";
        private string _outputFolder;

        private int _fileAmount;
        private int _fileComplete;
        private string _fileCompleteRatio;

        private int _rowAmount;
        private int _rowComplete;
        private string _rowCompleteRatio;

        private float _filePercentage;
        private float _rowPercentage;

        private string _filePercentageString;
        private string _rowPercentageString;

        private bool _hideProgress = true;
        private bool _disableDownsample = false;

        public string InputFile
        {
            get => _inputFile;
            set => SetProperty(ref _inputFile, value);
        }

        public string OutputFolder
        {
            get => _outputFolder;
            set => SetProperty(ref _outputFolder, value);
        }

        public int FileAmount
        {
            get => _fileAmount;
            set => SetProperty(ref _fileAmount, value);
        }

        public int FileComplete
        {
            get => _fileComplete;
            set => SetProperty(ref _fileComplete, value);
        }

        public string FileCompleteRatio
        {
            get => _fileCompleteRatio;
            set => SetProperty(ref _fileCompleteRatio, value);
        }

        public int RowAmount
        {
            get => _rowAmount;
            set => SetProperty(ref _rowAmount, value);
        }


        public int RowComplete
        {
            get => _rowComplete;
            set => SetProperty(ref _rowComplete, value);
        }

        public string RowCompleteRatio
        {
            get => _rowCompleteRatio;
            set => SetProperty(ref _rowCompleteRatio, value);
        }

        public float FilePercentage
        {
            get => _filePercentage;
            set => SetProperty(ref _filePercentage, value);
        }

        public string FilePercentageString
        {
            get => _filePercentageString;
            set => SetProperty(ref _filePercentageString, value);
        }

        public float RowPercentage
        {
            get => _rowPercentage;
            set => SetProperty(ref _rowPercentage, value);
        }

        public string RowPercentageString
        {
            get => _rowPercentageString;
            set => SetProperty(ref _rowPercentageString, value);
        }

        public bool HideProgress
        {
            get => _hideProgress;
            set => SetProperty(ref _hideProgress, value);
        }

        public bool DisableDownsample
        {
            get => _disableDownsample;
            set => SetProperty(ref _disableDownsample, value);
        }
    }
}
