using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Task2
{
    public class CardModel : INotifyPropertyChanged
    {
        private bool _isMatched;
        private bool _isOpen;
        public int Id { get; set; }
        public string Symbol { get; set; }

        public bool IsOpen
        {
            get => _isOpen;
            set { _isOpen = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentValue)); }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set { _isMatched = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentValue)); }
        }

        public string CurrentValue => (IsOpen || IsMatched) ? Symbol : "?";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}