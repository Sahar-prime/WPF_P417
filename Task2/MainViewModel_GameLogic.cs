namespace Task2
{
    public partial class MainViewModel
    {
        public void StartNewGame()
        {
            StopTimer();
            ResetTimerDisplay();

            Moves = 0;
            _firstSelectedCard = null;
            _isProcessing = false;

            List<string> symbols = new List<string> 
            { "🍎", "🍐", "🍋", "🍉", "🍇", "🍓", "🍒", "🥝" };
            List<CardModel> newCards = new List<CardModel>();

            int idCounter = 0;
            foreach (var symbol in symbols)
            {
                newCards.Add(new CardModel { Id = idCounter++, Symbol = symbol });
                newCards.Add(new CardModel { Id = idCounter++, Symbol = symbol });
            }

            // Перемешивание
            Random rand = new Random();
            var shuffledCards = newCards.OrderBy(c => rand.Next()).ToList();

            Cards.Clear();
            foreach (var card in shuffledCards)
            {
                Cards.Add(card);
            }

            StartTimer();
        }

        public async Task SelectCardAsync(CardModel selectedCard)
        {
            if (_isProcessing || selectedCard.IsOpen || selectedCard.IsMatched)
                return;

            selectedCard.IsOpen = true;

            if (_firstSelectedCard == null)
            {
                _firstSelectedCard = selectedCard;
            }
            else
            {
                Moves++;
                _isProcessing = true;

                if (_firstSelectedCard.Symbol == selectedCard.Symbol)
                {
                    // Совпадение найдено
                    _firstSelectedCard.IsMatched = true;
                    selectedCard.IsMatched = true;
                    _firstSelectedCard = null;
                    _isProcessing = false;

                    CheckWinCondition();
                }
                else
                {
                    // Символы разные — держим карту открытой 1 секунду и закрываем
                    await Task.Delay(1000);
                    _firstSelectedCard.IsOpen = false;
                    selectedCard.IsOpen = false;
                    _firstSelectedCard = null;
                    _isProcessing = false;
                }
            }
        }

        private void CheckWinCondition()
        {
            if (Cards.All(c => c.IsMatched))
            {
                StopTimer();
                System.Windows.MessageBox.Show(
                    $"Отличная работа! Вы открыли все пары за {Moves} ходов и {TimeDisplay} времени.",
                    "Победа!"
                );
            }
        }
    }
}