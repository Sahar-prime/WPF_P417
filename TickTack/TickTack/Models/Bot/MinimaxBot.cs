using System;
using System.Collections.Generic;
using System.Text;

namespace TickTack.Models.Bot
{
    public class MinimaxBot : IbotStrategy
    {
        private readonly Random random = new();
        public (int Row, int Col) ChooseMove(GameBoard board, Player bot, Player opponent)
        {
            int bestScore = int.MinValue;
            var bestMoves = new List<(int, int)>();

            foreach (var (r,c) in board.EmptyCells())
            {
                board[r, c] = bot;
                int score = Minimax(board,bot,opponent,isMax: false,depth: 0);
                board[r, c] = Player.None;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMoves.Clear();
                    bestMoves.Add((r, c));
                }
                else if(score == bestScore)
                {
                    bestMoves.Add((r, c));
                }
            }
            return bestMoves[random.Next(bestMoves.Count)];
        }
        private int Minimax(GameBoard board,Player bot, Player opponent,bool isMax, int depth)
        {
            var winner = board.GetWinner();
            if(winner == bot) return 10 - depth;
            if(winner == opponent) return depth - 10;
            if (board.isFull()) return 0;

            int best = isMax? int.MinValue : int.MaxValue;

            foreach (var (r,c) in board.EmptyCells())
            {
                board[r, c] = isMax ? bot : opponent;
                int score = Minimax(board, bot, opponent, !isMax, depth + 1);
                board[r, c] = Player.None;

                best = isMax ? Math.Max(best, score) : Math.Min(best, score);
            }
            return best;
        }
    }
}
