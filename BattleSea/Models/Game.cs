using System;
using BattleSea.Models.Enums;
using BattleSea.Models.Exceptions;

namespace BattleSea.Models
{
    public class Game : IEquatable<Game>
    {
        public Guid Id { get; }
        private Player _firstPlayer;
        private Player _secondPlayer;

        public delegate void GameOverEventHandler(object sender, GameOverEventArgs e);
        public event GameOverEventHandler GameOver;

        public Player FirstPlayer
        {
            get { return _firstPlayer; }
            set
            {
                //try to unsubscribe
                if (_firstPlayer?.BattleField != null)
                    _firstPlayer.BattleField.Fired -= FirstPlayerOnFired;

                if (value == null) return;
                _firstPlayer = value; _firstPlayer.BattleField.Fired += FirstPlayerOnFired;
            }
        }

        public Player SecondPlayer
        {
            get { return _secondPlayer; }
            set
            {
                //try to unsubscribe
                if (_secondPlayer?.BattleField != null)
                    _secondPlayer.BattleField.Fired -= SecondPlayerOnFired;

                if (value == null) return;
                _secondPlayer = value; _secondPlayer.BattleField.Fired += SecondPlayerOnFired;
            }
        }

        public Game(int fieldSize)
        {
            Id = Guid.NewGuid();
            FirstPlayer = new Player(fieldSize);
            SecondPlayer = new Player(fieldSize);
        }

        public GameState State { get; private set; }

        public Turn Turn { get; private set; }

        public Guid TurnPlayerId { get; private set; }

        public void Start()
        {
            if (State != GameState.Initialized)
                throw new InvalidOperationException($"Cannot start the Game when it's state is {State}.");

            if (!FirstPlayer.IsAvailable || !SecondPlayer.IsAvailable)
                throw new InvalidGameStateException("Cannot start the Game without both players initialized.");

            State = GameState.Started;
            TurnPlayerId = FirstPlayer.Id;
        }

        public Player GetPlayerById(Guid id, bool theOtherOne = false)
        {
            if (FirstPlayer.Id == id)
                return theOtherOne ? SecondPlayer : FirstPlayer;
            else if (SecondPlayer.Id == id)
                return theOtherOne ? FirstPlayer : SecondPlayer;

            throw new ArgumentException("No player with provided ID was found in this game.");
        }

        public bool HasPlayer(Guid id)
        {
            return (FirstPlayer != null && FirstPlayer.Id == id && FirstPlayer.IsAvailable)
                || (SecondPlayer != null && SecondPlayer.Id == id && SecondPlayer.IsAvailable);
        }

        #region Events Handlers

        private void SecondPlayerOnFired(object sender, FiredEventArgs e)
        {
            if (e.Result != CellState.Exploded)
            {
                Turn = Turn.SecondPlayer;
                TurnPlayerId = SecondPlayer.Id;
            }

            CheckGameOver(SecondPlayer);
        }

        private void FirstPlayerOnFired(object sender, FiredEventArgs e)
        {
            if (e.Result != CellState.Exploded)
            {
                Turn = Turn.FirstPlayer;
                TurnPlayerId = FirstPlayer.Id;
            }

            CheckGameOver(FirstPlayer);
        }

        private void CheckGameOver(Player player)
        {
            if (player.BattleField.AllShipsDestroyed)
            {
                GameOver?.Invoke(this, new GameOverEventArgs { WinnerPlayer = GetPlayerById(player.Id, theOtherOne: true) });

                //change state of the game
                State = GameState.Finished;
            }
        }

        #endregion

        public bool Equals(Game other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class GameOverEventArgs
    {
        public Player WinnerPlayer { get; set; }
    }
}