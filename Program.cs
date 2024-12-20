using System.Runtime.CompilerServices;

namespace RedSevenGameBase
{
    enum Numbers
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7
    }

    enum Colors
    { 
        Red = 'R',
        Orange = 'O', 
        Yellow = 'Y', 
        Green = 'G', 
        Cyan = 'C', 
        Blue = 'B', 
        Purple = 'P'
    }

    struct Color
    {
        private static Dictionary<Colors, int> _colorsSortOrder = new Dictionary<Colors, int> 
        { 
            { Colors.Red, 700 },
            { Colors.Orange, 600 },
            { Colors.Yellow, 500 },
            { Colors.Green, 400 },
            { Colors.Cyan, 300 },
            { Colors.Blue, 200 },
            { Colors.Purple, 100 },
        };

        public char Name { get; private set; }
        public int Value { get; private set; }

        public Color(Colors color)
        {
            this.Name = (char)color;
            this.Value = GetColorsValue(color);
        }

        private static int GetColorsValue(Colors color)
        {
            if (!_colorsSortOrder.ContainsKey(color))
                throw new InvalidOperationException($"The value is not defined for the color {color}");

            return _colorsSortOrder[color];
        }
    }

    struct Card(Colors color, Numbers number) : IComparable<Card>
    {    
        public Color Color { get; } = new Color(color);
        public Numbers Number { get; } = number;             

        public int CompareTo(Card other)
        {
            int numberComparisson = this.Number.CompareTo(other.Number);

            return numberComparisson != 0 ? numberComparisson : this.Color.Value.CompareTo(other.Color.Value);
        }

        public override string ToString() => $"{(byte)Number} {Color.Name}";
    }

    struct CardCombination
    {
        private List<Card> _cards;
        private Card? _highestCard;

        public Card HighestCard
        {
            get
            {
                if (_highestCard.HasValue) 
                    return _highestCard.Value;

                throw new InvalidOperationException("There is no highest card in the card combination.");
            }
        }

        public CardCombination()
        {
            _cards = new List<Card>();
            _highestCard = null;
        }

        public void Add(Card card)
        {
            _cards.Add(card);

            if (!_highestCard.HasValue || _highestCard.Value.CompareTo(card) < 0)
                _highestCard = card;
        }
    }

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
                throw new InvalidOperationException("The card is already in use.");

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
                AddCardFromConsoleToCombination(ref cardCombination);

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

            var highestCardsComparisson = firstCombination.HighestCard.CompareTo(secondCombination.HighestCard);
            if (highestCardsComparisson > 0)
                Console.WriteLine($"The first combination wins.\n{firstCombination.HighestCard}");
            else if (highestCardsComparisson == 0)
                Console.WriteLine("The game is tied.");
            else
                Console.WriteLine($"The second combination wins.\n{secondCombination.HighestCard}");
        }

        static void Main(string[] args)
        {
            StartGame();
        }
    }
}
