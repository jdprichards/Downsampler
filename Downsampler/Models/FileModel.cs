using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downsampler.Models
{
    public class FileModel : INotifyPropertyChanged
    {
        private string _inputFile;
        private string _inputFolder;
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

        public string InputFile
        {
            get { return _inputFile; }
            set
            {
                _inputFile = value;
                RaisePropertyChanged(nameof(InputFile));
            }
        }

        public string InputFolder
        {
            get { return _inputFolder; }
            set
            {
                _inputFolder = value;
                RaisePropertyChanged(nameof(InputFolder));
            }
        }

        public string OutputFolder
        {
            get { return _outputFolder;}
            set
            {
                _outputFolder = value;
                RaisePropertyChanged(nameof(OutputFolder));
            }
        }

        public int FileAmount
        {
            get { return _fileAmount; }
            set
            {
                _fileAmount = value;
                RaisePropertyChanged(nameof(FileAmount));
            }
        }

        public int FileComplete
        {
            get { return _fileComplete; }
            set
            {
                _fileComplete = value;
                RaisePropertyChanged(nameof(FileComplete));
            }
        }

        public string FileCompleteRatio
        {
            get { return _fileCompleteRatio; }
            set
            {
                _fileCompleteRatio = value;
                RaisePropertyChanged(nameof(FileCompleteRatio));
            }
        }

        public int RowAmount
        {
            get { return _rowAmount; }
            set
            {
                _rowAmount = value;

                RaisePropertyChanged(nameof(RowAmount));
            }
        }

        
        public int RowComplete
        {
            get { return _rowComplete; }
            set
            {
                _rowComplete = value;
                RaisePropertyChanged(nameof(RowComplete));
            }
        }

        public string RowCompleteRatio
        {
            get { return _rowCompleteRatio; }
            set
            {
                _rowCompleteRatio = value;
                RaisePropertyChanged(nameof(RowCompleteRatio));
            }
        }

        public float FilePercentage
        {
            get { return _filePercentage; }
            set
            {
                _filePercentage = value;
                RaisePropertyChanged(nameof(FilePercentage));
            }
        }

        public string FilePercentageString
        {
            get { return _filePercentageString; }
            set
            {
                _filePercentageString = value;
                RaisePropertyChanged(nameof(FilePercentageString)); 
            }
        }

        public float RowPercentage
        {
            get { return _rowPercentage;}
            set
            {
                _rowPercentage = value;
                RaisePropertyChanged(nameof(RowPercentage));
            }
        }

        public string RowPercentageString
        {
            get { return _rowPercentageString; }
            set
            {
                _rowPercentageString = value;
                RaisePropertyChanged(nameof(RowPercentageString));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            /*PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }*/
        }
    }
}
