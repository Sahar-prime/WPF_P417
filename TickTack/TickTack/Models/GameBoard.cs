using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TickTack.Models
{
    public class GameBoard
    {
        public const int Size = 3;
        private readonly Player[,] cells = new Player[Size, Size];
        public Player this[int row,int col]
        {
            get => cells[row,col];
            set => cells[row,col] = value;
        }
        public void Clear()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    cells[r, c] = Player.None;
                }
            }
        }
        public bool isEmpty(int row, int col) => cells[row, col] == Player.None;
        public bool isFull()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (cells[r,c] == Player.None) return false;
                }
            }
            return true;
        }
        public IEnumerable<(int Row,int Col)> EmptyCells()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (cells[r, c] == Player.None) yield return (r,c);
                }
            }
        }
        public Player GetWinner()
        {
            for (int i = 0; i < Size; i++)
            {
                if (cells[i, 0] != Player.None 
                    && cells[i, 0] == cells[i, 1] 
                    && cells[i, 1] == cells[i, 2]) return cells[i, 0];
            
                if (cells[0, i] != Player.None 
                    && cells[0, i] == cells[1, i] 
                    && cells[1, i] == cells[2, i]) return cells[0, i];
            }

            if (cells[0, 0] != Player.None
                    && cells[0, 0] == cells[1, 1]
                    && cells[1, 1] == cells[2, 2]) 
                return cells[0, 0];
            if (cells[0, 2] != Player.None
                   && cells[0, 2] == cells[1, 1]
                   && cells[1, 1] == cells[2, 0]) 
                return cells[0, 0];
            return Player.None;
        }
    }
}
