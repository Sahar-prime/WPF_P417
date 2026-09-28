using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TickTack.Models;

namespace TickTack.ViewModels
{
    public class CellViewModel : ObservableObject
    {
        private Player player = Player.None;
        private bool isEnabled;
        public CellViewModel(int row,int col,Action<CellViewModel> onCellClicked)
        {
            Row = row;
            Col = col;
            ClickCommand = new RelayCommand(_ => onCellClicked(this), _ => isEnabled);
        }
        public int Row { get; }
        public int Col { get; }
        public Player Player
        {
            get => player;
            set
            {
                if (SetProperty(ref player, value))
                    OnPropertyChanged(nameof(Symbol));
            }
        }
        public string Symbol => Player switch
        {
            Player.X => "X",
            Player.O => "O",
            _ => string.Empty
        };
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }
        public ICommand ClickCommand { get; }
    }
}
