using MemoryGame.ViewModels;

namespace Task2.Models
{
    public class Card : ObservableObject
    {
        private string _content = "?";
        private bool _isMatched;
        private bool _isRevealed;

        public string Value { get; set; }

        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public bool IsMatched
        {
            get => _isMatched;
            set => SetProperty(ref _isMatched, value);
        }

        public bool IsRevealed
        {
            get => _isRevealed;
            set => SetProperty(ref _isRevealed, value);
        }
    }
}