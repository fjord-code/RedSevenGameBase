using System.Runtime.CompilerServices;

namespace RedSevenGameBase
{  
    internal class Program
    {
        private static HashSet<Card> _usedCards = new HashSet<Card>();

        static int GetCombinationLenghtFromConsole()
        {
            Console.Write("Input the combination length: ");

            return int.Parse(Console.ReadLine());
        }

        static Card GetCardFromConsole()
        {
            Console.Write("Input a card (value and color separated by a space): ");

            var rawInput = Console.ReadLine();
            var rawValues = rawInput.Split(' ');
            var value = int.Parse(rawValues[0]);
            var color = rawValues[1][0];

            return new Card(
                (Colors)Enum.ToObject(typeof(Colors), color),
                (Numbers)Enum.ToObject(typeof(Numbers), value)
            );
        }

        static void AddCardToCombination(ref CardCombination cardCombination, Card card)
        {
            if (_usedCards.Contains(card))
            {
                throw new InvalidOperationException("The card is already in use.");
            }

            _usedCards.Add(card);
            cardCombination.Add(card);
        }

        static void AddCardFromConsoleToCombination(ref CardCombination cardCombination)
        {
            var card = GetCardFromConsole();
            AddCardToCombination(ref cardCombination, card);
        }

        static CardCombination CreateCardCombinationFromConsole(int combinationLength)
        {
            var cardCombination = new CardCombination();

            for (var i = 0; i < combinationLength; i++)
            {
                AddCardFromConsoleToCombination(ref cardCombination);
            }

            return cardCombination;
        }

        static CardCombination CreateCardCombinationFromConsole()
        {
            var combinationLength = GetCombinationLenghtFromConsole();
            return CreateCardCombinationFromConsole(combinationLength);
        }

        static void StartGame()
        {
            _usedCards.Clear();

            Console.WriteLine("Input the first combination.");
            var firstCombination = CreateCardCombinationFromConsole();

            Console.WriteLine("Input the second combination.");
            var secondCombination = CreateCardCombinationFromConsole();

            var highestCardsComparisson = firstCombination.GetHighestCard().CompareTo(secondCombination.GetHighestCard());
            if (highestCardsComparisson > 0)
            {
                Console.WriteLine($"The first combination wins.\n{firstCombination.GetHighestCard()}");
            }
            else if (highestCardsComparisson == 0)
            {
                Console.WriteLine("The game is tied.");
            }
            else
            {
                Console.WriteLine($"The second combination wins.\n{secondCombination.GetHighestCard()}");
            }
        }

        static void Main(string[] args)
        {
            StartGame();
        }
    }
}
