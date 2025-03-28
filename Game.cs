namespace TicTacToeMinimax
{
    class Game
    {
        private readonly MarkEnum?[] state = new MarkEnum?[9];

        private static readonly List<int[]> winStateIndices = [
            [0, 1, 2],  // Rows
            [3, 4, 5],
            [6, 7, 8],
            [0, 3, 6],  // Columns
            [1, 4, 7],
            [2, 5, 8],
            [0, 4, 8],  // Diagonals
            [2, 4, 6]
        ];

        private static readonly List<(int, int)> numpadKeyMap = [
            (7, 0),
            (8, 1),
            (9, 2),
            (4, 3),
            (5, 4),
            (6, 5),
            (1, 6),
            (2, 7),
            (3, 8),
        ];

        public enum MarkEnum
        {
            X,
            O
        }

        public void Print()
        {
            for (int i = 0; i < state.Length; i++)
            {
                string stringRepresentation = state[i] switch
                {
                    MarkEnum.X => "X",
                    MarkEnum.O => "O",
                    _ => (IxToNumpad(i)).ToString(),
                };

                Console.ForegroundColor = state[i] switch
                {
                    MarkEnum.X => ConsoleColor.Green,
                    MarkEnum.O => ConsoleColor.Red,
                    _ => ConsoleColor.DarkGray,
                };

                Console.Write(stringRepresentation.PadRight(2));

                if ((i + 1) % 3 == 0)
                    Console.WriteLine();

                Console.ResetColor();
            }

        }

        static public int NumPadToIx(int num) => numpadKeyMap.Single(x => x.Item1 == num).Item2;
        static private int IxToNumpad(int num) => numpadKeyMap.Single(x => x.Item2 == num).Item1;

        public void PlayerMark(int num)
        {
            state[num] = MarkEnum.X;

            ComputerMove();
        }

        public bool IsEmpty(int n) => state[n] == null;

        private void ComputerMove()
        {
            if (GetWinner(state) != null || IsBoardFull(state))
                return;

            int bestScore = int.MinValue;
            int? bestMove = null;

            for (int i = 0; i < state.Length; i++)
            {
                if (state[i] != null)
                    continue;

                var clonedState = (MarkEnum?[])state.Clone();
                clonedState[i] = MarkEnum.O;

                int score = Minimax(clonedState, MarkEnum.X);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }

            if (bestMove.HasValue)
                state[bestMove.Value] = MarkEnum.O;
        }

        private static int Minimax(MarkEnum?[] clonedState, MarkEnum next)
        {
            var winner = GetWinner(clonedState);

            if (winner != null)
                return winner == MarkEnum.O ? 1 : -1;

            if (IsBoardFull(clonedState))
                return 0;

            if (next == MarkEnum.X)
            {
                // Minimizing (X is computer's opponent)
                int best = int.MaxValue;
                for (int i = 0; i < clonedState.Length; i++)
                {
                    if (clonedState[i] != null)
                        continue;

                    MarkEnum?[] newState = (MarkEnum?[])clonedState.Clone();

                    newState[i] = MarkEnum.X;

                    best = int.Min(best, Minimax(newState, MarkEnum.O));
                }
                return best;
            }
            else
            {
                // Maximizing (O is computer)
                int best = int.MinValue;
                for (int i = 0; i < clonedState.Length; i++)
                {
                    if (clonedState[i] != null)
                        continue;

                    MarkEnum?[] newState = (MarkEnum?[])clonedState.Clone();

                    newState[i] = MarkEnum.O;

                    best = int.Max(best, Minimax(newState, MarkEnum.X));
                }
                return best;
            }
        }

        public bool IsBoardFull() => IsBoardFull(state);
        private static bool IsBoardFull(MarkEnum?[] currentState) => currentState.All(x => x != null);

        public MarkEnum? GetWinner() => GetWinner(state);
        private static MarkEnum? GetWinner(MarkEnum?[] currentState)
        {
            foreach (var win in winStateIndices)
            {
                MarkEnum? winEnum = currentState[win[0]];

                if (winEnum == null)
                {
                    continue;
                }

                foreach (int index in win)
                {
                    if (winEnum != currentState[index])
                    {
                        winEnum = null;
                        break;
                    }
                }

                if (winEnum != null)
                    return winEnum;
            }

            return null;
        }
    }
}
