namespace BattleSea.Models
{
    public class Player
    {
        public Player(int fieldSize)
        {
            BattleField = new BattleField(fieldSize);
        }

        public BattleField BattleField { get; set; }
    }
}