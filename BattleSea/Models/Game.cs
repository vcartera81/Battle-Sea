using System;

namespace BattleSea.Models
{
    public class Game
    {
        public Guid Id { get; }

        public Game(int fieldSize)
        {
            Id = Guid.NewGuid();
            FirstPlayer = new Player(fieldSize);
            SecondPlayer = new Player(fieldSize);
        }

        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }

        public bool Started { get; set; }
    }
}