using System;
using BattleSea.Models.Enums;
using WebGrease.Css.Extensions;

namespace BattleSea.Models
{
    public class Game
    {
        public Guid Id { get; }

        private readonly int _fieldSize;

        public Game(int fieldSize)
        {
            Id = Guid.NewGuid();
            FirstPlayer = new Player(fieldSize);
            SecondPlayer = new Player(fieldSize);
            _fieldSize = fieldSize;

            //register event handlers
            FirstPlayer.BattleField.Fired += FirstPlayerOnFired;
            SecondPlayer.BattleField.Fired += SecondPlayerOnFired;
        }

        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }

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
            Turn = Turn.FirstPlayer;
        }

        private void FirstPlayerOnFired(object sender, FiredEventArgs e)
        {
            Turn = Turn.SecondPlayer;
        }

        #endregion
    }
}