using MemoryGame.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Task2.Models;

namespace Task2.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private static readonly string[] AllSymbols = {
            "🍎","🍌","🍇","🍓","🍒","🍑","🥝","🍍",
            "🥥","🍋","🍊","🍉","🍐","🥭","🫐","🍅",
            "🌽","🥕","🍄","🌶️","🥦","🧄","🧅","🥔",
            "🍠","🥑","🍆","🫒","🥜","🌰","🍞","🧀"
        };

        private readonly Random random = new();
        private DispatcherTimer? gameTimer;
        private DispatcherTimer? flipTimer;

        private int _moves;
        private int _secondsElapsed;
        private int _matchesFound;
        private Card? _firstClicked;
        private Card? _secondClicked;

        public int Moves
        {
            get => _moves;
            set => SetProperty(ref _moves, value);
        }

        public int SecondsElapsed
        {
            get => _secondsElapsed;
            set => SetProperty(ref _secondsElapsed, value);
        }

        public ObservableCollection<Card> Cards { get; } = new();

        public ICommand CardClickCommand { get; }
        public ICommand RestartCommand { get; }

        public MainViewModel()
        {
            CardClickCommand = new RelayCommand(ExecuteCardClick);
            RestartCommand = new RelayCommand(_ => StartNewGame());

            SetupTimers();
            StartNewGame();
        }

        private void SetupTimers()
        {
            gameTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            gameTimer.Tick += GameTimer_Tick;

            flipTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            flipTimer.Tick += FlipTimer_Tick;
        }

        private void StartNewGame()
        {
            Moves = 0;
            SecondsElapsed = 0;
            _matchesFound = 0;
            _firstClicked = null;
            _secondClicked = null;

            flipTimer?.Stop();
            gameTimer?.Start();

            var selectedSymbols = AllSymbols.OrderBy(_ => random.Next()).Take(8).ToList();

            var gameSymbols = selectedSymbols.Concat(selectedSymbols).OrderBy(_ => random.Next()).ToList();
            
            Cards.Clear();
            foreach (var symbol in gameSymbols)
            {
                Cards.Add(new Card
                {
                    Value = symbol,
                    Content = "?",
                    IsRevealed = false,
                    IsMatched = false
                });
            }
        }

        private void ExecuteCardClick(object? parameter)
        {
            if (parameter is not Card clickedCard) return;

            if (clickedCard.IsRevealed || clickedCard.IsMatched || (flipTimer?.IsEnabled ?? false))
                return;

            // Переворачиваем карточку лицевой стороной вверх
            clickedCard.Content = clickedCard.Value;
            clickedCard.IsRevealed = true;

            if (_firstClicked == null)
            {
                _firstClicked = clickedCard;
            }
            else
            {
                _secondClicked = clickedCard;
                Moves++;

                if (_firstClicked.Value == _secondClicked.Value)
                {
                    _firstClicked.IsMatched = true;
                    _secondClicked.IsMatched = true;

                    _matchesFound++;
                    _firstClicked = null;
                    _secondClicked = null;

                    if (_matchesFound == Cards.Count / 2)
                    {
                        gameTimer?.Stop();
                        MessageBox.Show($"Поздравляем! Вы нашли все пары за {Moves} ходов и {SecondsElapsed} сек.", "Победа!");
                    }
                }
                else
                {
                    flipTimer?.Start();
                }
            }
        }

        private void FlipTimer_Tick(object? sender, EventArgs e)
        {
            flipTimer?.Stop();

            if (_firstClicked != null && _secondClicked != null)
            {
                ResetCard(_firstClicked);
                ResetCard(_secondClicked);
            }

            _firstClicked = null;
            _secondClicked = null;
        }

        private void ResetCard(Card card)
        {
            card.Content = "?";
            card.IsRevealed = false;
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            SecondsElapsed++;
        }
    }
}