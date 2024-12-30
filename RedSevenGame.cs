using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSevenGameBase
{
    /// <summary>
    /// Models a Red Seven Game.
    /// </summary>
    internal class RedSevenGame : IGame
    {
        /// <value>
        /// Set of cards that are already used in the game.
        /// </value>
        private HashSet<Card> _usedCards;
        /// <value>
        /// Card combination of the first player.
        /// </value>
        private CardCombination _firstCardCombination;
        /// <value>
        /// Card combination of the second player.
        /// </value>
        private CardCombination _secondCardCombination;

        /// <inheritdoc/>
        public event Message? SendingMessageEventHandler;
        /// <inheritdoc/>
        public event FillCardCombination? FillingCardCombinationEventHandler;

        /// <summary>
        /// Creates a <c>RedSevenGame</c> instance.
        /// </summary>
        public RedSevenGame()
        {
            _usedCards = new HashSet<Card>();
            _firstCardCombination = new CardCombination();
            _secondCardCombination = new CardCombination();
        }

        /// <summary>
        /// Sends the message to show to event listeners.
        /// </summary>
        /// <param name="message">Message to show.</param>
        private void ShowMessage(string message)
        {
            SendingMessageEventHandler?.Invoke(this, new GameEventArgs(message));
        }

        /// <summary>
        /// Fills the card combination with cards.
        /// </summary>
        /// <param name="cardCombination">Card combination to fill with cards.</param>
        /// <param name="combinationName">Name of the card combination.</param>
        private void FillCardCombination(CardCombination cardCombination, string combinationName)
        {
            cardCombination.Clear();

            Action<Card> addCardCallbacks = cardCombination.Add;
            addCardCallbacks += (card) => _usedCards.Add(card);

            FillingCardCombinationEventHandler?.Invoke(
                this,
                new GameEventArgs(combinationName),
                (card) => !_usedCards.Contains(card),
                addCardCallbacks
            );
        }

        /// <inheritdoc/>
        public void StartGame()
        {
            _usedCards.Clear();

            FillCardCombination(_firstCardCombination, "the first");
            FillCardCombination(_secondCardCombination, "the second");

            ShowGameResults();
        }

        /// <summary>
        /// Shows the game results.
        /// </summary>
        private void ShowGameResults()
        {
            var highestCardsComparisson = _firstCardCombination.GetHighestCard().CompareTo(_secondCardCombination.GetHighestCard());
            if (highestCardsComparisson > 0)
            {
                ShowMessage($"The first combination wins.\n{_firstCardCombination.GetHighestCard()}");
            }
            else if (highestCardsComparisson == 0)
            {
                ShowMessage("The game is tied.");
            }
            else
            {
                ShowMessage($"The second combination wins.\n{_secondCardCombination.GetHighestCard()}");
            }
        }
    }
}
