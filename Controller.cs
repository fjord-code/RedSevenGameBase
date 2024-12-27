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
    /// Data request from the game. Called when the game is needed for some data from user.
    /// </summary>
    /// <typeparam name="T">Type of data the game receives.</typeparam>
    /// <param name="sender">Sender of the request.</param>
    /// <param name="e">Request parameters.</param>
    /// <returns></returns>
    delegate T DataRequest<T>(object sender, GameEventArgs e);

    /// <summary>
    /// Message from the game. Called when the game sends a message.
    /// </summary>
    /// <param name="sender">Sender of the message.</param>
    /// <param name="e">Message parameters.</param>
    delegate void Message(object sender, GameEventArgs e);

    /// <summary>
    /// Game API. Members that a game should implement to be able to connect to a presenter.
    /// </summary>
    interface IGame
    {
        /// <summary>
        /// Fires when the game is getting a card from a presenter.
        /// </summary>
        public event DataRequest<Card> GettingCardEventHandler;
        /// <summary>
        /// Fires when the game is getting a combination length from a presenter.
        /// </summary>
        public event DataRequest<int> GettingCombinationLengthEventHandler;
        /// <summary>
        /// Fires when the game is sending a message to a presenter.
        /// </summary>
        public event Message SendingMessageEventHandler;

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
        /// Returns typed data from the presenter.
        /// </summary>
        /// <typeparam name="T">Type of data to return.</typeparam>
        /// <param name="prompt">Prompt to a user to describe the data.</param>
        /// <returns>Typed data from a user.</returns>
        public T GetUserInput<T>(string prompt);
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

            game.GettingCardEventHandler += (object sender, GameEventArgs e) => presenter.GetUserInput<Card>(e.GameMessage);
            game.GettingCombinationLengthEventHandler += (object sender, GameEventArgs e) => presenter.GetUserInput<int>(e.GameMessage);
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
