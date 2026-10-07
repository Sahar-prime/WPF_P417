using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Task2
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private int _moves;
        private string _timeDisplay = "00:00";
        private bool _isProcessing;
        private CardModel _firstSelectedCard;

        public ObservableCollection<CardModel> Cards { get; set; } 
            = new ObservableCollection<CardModel>();

        public int Moves
        {
            get => _moves;
            set { _moves = value; OnPropertyChanged(); }
        }

        public string TimeDisplay
        {
            get => _timeDisplay;
            set { _timeDisplay = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            SetupTimer();
            StartNewGame();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}