using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSevenGameBase
{
    /// <summary>
    /// Event arguments for the game events.
    /// </summary>
    class GameEventArgs : EventArgs
    {
        /// <value>
        /// Message from the game.
        /// </value>
        public string GameMessage { get; }

        /// <summary>
        /// Creates a new instance of the <c>GameEventArgs</c> class.
        /// </summary>
        /// <param name="gameMessage">Message from the game.</param>
        public GameEventArgs(string gameMessage)
        {
            GameMessage = gameMessage;
        }
    }

    /// <summary>
    /// Message from the game. Called when the game sends a message.
    /// </summary>
    /// <param name="sender">Sender of the message.</param>
    /// <param name="e">Message parameters.</param>
    delegate void Message(object sender, GameEventArgs e);

    /// <summary>
    /// Fills a card combination with cards.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="e">Message parameters.</param>
    /// <param name="checkCardCallback">Callback that checks if we can add a card to a combination.</param>
    /// <param name="addCardCallback">Callback that add a card to a combination.</param>
    delegate void FillCardCombination(object sender, GameEventArgs e, Func<Card, bool> checkCardCallback, Action<Card> addCardCallback);

    /// <summary>
    /// Game API. Members that a game should implement to be able to connect to a presenter.
    /// </summary>
    interface IGame
    {
        /// <summary>
        /// Fires when the game is filling a card combination.
        /// </summary>
        public event FillCardCombination? FillingCardCombinationEventHandler;
        /// <summary>
        /// Fires when the game is sending a message to a presenter.
        /// </summary>
        public event Message? SendingMessageEventHandler;

        /// <summary>
        /// Starts a game loop.
        /// </summary>
        public void StartGame();
    }

    /// <summary>
    /// Presenter API. List of members a present should implement to be able to connect to a game.
    /// </summary>
    interface IPresenter
    {
        /// <summary>
        /// Fills a card combination by calling related callbacks.
        /// </summary>
        /// <param name="combinationName">Name of the combination to show to user.</param>
        /// <param name="checkCardCallback">Function that should check whether it possible to add a card or not.</param>
        /// <param name="addCardCallback">Function that adds a card to a combination.</param>
        public void FillCardCombination(string combinationName, Func<Card, bool> checkCardCallback, Action<Card> addCardCallback);
        /// <summary>
        /// Shows a message by the presenter.
        /// </summary>
        /// <param name="message">Message to show.</param>
        public void ShowMessage(string message);
    }

    /// <summary>
    /// Controller binds a game of type <c>G</c> and a presenter of type <c>P</c> to create an interactive game.
    /// </summary>
    /// <typeparam name="G">Implementation of game logic.</typeparam>
    /// <typeparam name="P">Implementation of presenter. </typeparam>
    internal class Controller<G, P>
        where G : IGame, new()
        where P : IPresenter, new()
    {
        /// <value>
        /// Object with game logic implementation.
        /// </value>
        private readonly G game;
        /// <value>
        /// Object with presenter logic implementation.
        /// </value>
        private readonly P presenter;

        /// <summary>
        /// Creates a new <c>Controller</c> instance that binds together a game of type <c>G</c> and a presenter of type <c>P</c>.
        /// </summary>
        public Controller() 
        {
            game = new G();
            presenter = new P();

            game.FillingCardCombinationEventHandler += 
                (object sender, GameEventArgs e, Func<Card, bool> checkCardCallback, Action<Card> addCardCallback) =>
                    presenter.FillCardCombination(e.GameMessage, checkCardCallback, addCardCallback);
            game.SendingMessageEventHandler += (object sender, GameEventArgs e) => presenter.ShowMessage(e.GameMessage);
        }

        /// <summary>
        /// Starts a game.
        /// </summary>
        public void StartGame()
        {
            game.StartGame();
        }
    }
}
