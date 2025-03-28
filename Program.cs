using TicTacToeMinimax;

for (; ; )
{
    Game g = new();
    for (; ; )
    {
        Console.Clear();

        g.Print();

        var winner = g.GetWinner();

        if (winner != null)
        {
            string winnerStr = winner == Game.MarkEnum.X ? "Human player" : "Computer";
            Console.WriteLine($"{winnerStr} wins!");
            break;
        }
        else if (g.IsBoardFull())
        {
            Console.WriteLine("Draw!");
            break;
        }

        var key = Console.ReadKey();
        if (char.IsNumber(key.KeyChar) && int.TryParse(key.KeyChar.ToString(), out int num) && num > 0)
        {
            var ix = Game.NumPadToIx(num);
            if (g.IsEmpty(ix))
            {
                g.PlayerMark(ix);
            }
        }
    }

    Console.WriteLine("Press any key to continue.");
    Console.ReadKey();
}
