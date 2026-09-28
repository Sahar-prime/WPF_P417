using System.Collections.ObjectModel;
using System.Windows.Input;
using TickTack.Models;
using TickTack.Models.Bot;

namespace TickTack.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private const Player Human = Player.X;
        private const Player BotPlayer = Player.O;
        private readonly GameBoard board = new();
        private readonly IbotStrategy bot;
        private string statusText = string.Empty;
        private bool playerTurn;
        private bool gameOver;
        public MainViewModel() : this(new MinimaxBot()) { }
        public MainViewModel(IbotStrategy botStrategy)
        {
            bot = botStrategy;

            for (int r = 0; r < GameBoard.Size; r++)
                for (int c = 0; c < GameBoard.Size; c++)
                    Cells.Add(new CellViewModel(r, c, onCellClicked));
            NewGameCommand = new RelayCommand(_ => NewGame());
            NewGame();
        }
        public ObservableCollection<CellViewModel> Cells { get; } = new();
        public ICommand NewGameCommand { get; }
        public string StatusText
        {
            get => statusText;
            set => SetProperty(ref statusText, value);
        }
        public void NewGame()
        {
            board.Clear();
            foreach (var cell in Cells)
            {
                cell.Player = Player.None;
                cell.IsEnabled = true;
            }

            playerTurn = true;
            gameOver = false;
            statusText = "Ваш ход (Х)";
        }
        private async void onCellClicked(CellViewModel cell)
        {
            if(gameOver || !playerTurn) return;
            if (!board.isEmpty(cell.Row, cell.Col)) return;

            ApplyMove(cell, Human);
            if (CheckEnd()) return;

            playerTurn = false;
            SetCellsEnabled(false);
            statusText = "Ход бота...";

            await Task.Delay(350);

            BotMove();
            if(CheckEnd()) return;

            playerTurn = true;
            SetCellsEnabled(true);
            statusText = "Ваш ход (Х)";
        }
        private void BotMove()
        {
            var (row, col) = bot.ChooseMove(board, BotPlayer, Human);
            ApplyMove(GetCell(row, col), BotPlayer);
        }
        private void ApplyMove(CellViewModel cell,Player player)
        {
            board[cell.Row, cell.Col] = player;
            cell.Player = player;
        }
        private bool CheckEnd()
        {
            var winner = board.GetWinner();
            if(winner != Player.None)
            {
                gameOver = true;
                SetCellsEnabled(false);
                StatusText = winner == Human ? "Вы побелиди!" : "Бот победил! ";
                return true;
            }
            if(board.isFull())
            {
                gameOver = true;
                SetCellsEnabled(false);
                StatusText = "Ничья!";
                return true;
            }
            return false;
        }
        private void SetCellsEnabled(bool enabled)
        {
            foreach (var cell in Cells)
            {
                cell.IsEnabled = enabled && board.isEmpty(cell.Row, cell.Col);
            }
        }
        private CellViewModel GetCell(int row, int col) => Cells[row * GameBoard.Size + col];
    }
}