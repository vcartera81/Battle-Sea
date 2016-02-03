using System;
using BattleSea.Models.Enums;

namespace BattleSea.Models
{
    public class Game
    {
        public Guid Id { get; }

        public delegate void GameOverEventHandler(object sender, GameOverEventArgs e);
        public event GameOverEventHandler GameOver;

        public Player FirstPlayer { get; }
        public Player SecondPlayer { get; }

        public Game(int fieldSize)
        {
            Id = Guid.NewGuid();
            FirstPlayer = new Player(fieldSize);
            SecondPlayer = new Player(fieldSize);

            //register event handlers
            FirstPlayer.BattleField.Fired += FirstPlayerOnFired;
            SecondPlayer.BattleField.Fired += SecondPlayerOnFired;
        }

        public GameState State { get; private set; }

        public Turn Turn { get; private set; }

        public void Start()
        {
            if (State == GameState.Initialized)
                State = GameState.Started;
            else
                throw new InvalidOperationException($"Cannot start the Game when it's state is {State}.");
        }

        public Player GetPlayerById(Guid id, bool theOtherOne = false)
        {
            if (FirstPlayer.Id == id)
                return theOtherOne ? SecondPlayer : FirstPlayer;
            else if (SecondPlayer.Id == id)
                return theOtherOne ? FirstPlayer : SecondPlayer;

            throw new ArgumentException("No player with provided ID was found in this game.");
        }

        #region Events Handlers

        private void SecondPlayerOnFired(object sender, FiredEventArgs e)
        {
            if (e.Result != CellState.Exploded)
                Turn = Turn.SecondPlayer;

            CheckGameOver(SecondPlayer);
        }

        private void FirstPlayerOnFired(object sender, FiredEventArgs e)
        {
            if (e.Result != CellState.Exploded)
                Turn = Turn.FirstPlayer;

            CheckGameOver(FirstPlayer);
        }

        private void CheckGameOver(Player player)
        {
            if (player.BattleField.AllShipsDestroyed)
                GameOver?.Invoke(this, new GameOverEventArgs { WinnerPlayer = GetPlayerById(player.Id, theOtherOne:true) });

            //change state of the game
            State = GameState.Finished;
        }

        #endregion
    }

    public class GameOverEventArgs
    {
        public Player WinnerPlayer { get; set; }
    }
}