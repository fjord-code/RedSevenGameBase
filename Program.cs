using System.Runtime.CompilerServices;

namespace RedSevenGameBase
{
    /// <summary>
    /// Enumeration <c>Numbers</c> models a list of numbers that can be used as cards numbers in the Red Seven game.
    /// </summary>
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

    /// <summary>
    /// Enumeration <c>Colors</c> models a list of colors that can be used as cards colors in the Red Seven game.
    /// </summary>
    /// <remarks>
    /// Each enumeration member is mapped to a character that represents a color in the console.
    /// </remarks>
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

    /// <summary>
    /// Structure represents a color of a card of the Red Seven game along with it's value that is used to compare cards by a color.
    /// </summary>
    struct Color
    {
        /// <summary>
        /// A mapping between colors and their comparable values.
        /// </summary>
        private static readonly Dictionary<Colors, int> _colorsSortOrder = new Dictionary<Colors, int> 
        { 
            { Colors.Red, 700 },
            { Colors.Orange, 600 },
            { Colors.Yellow, 500 },
            { Colors.Green, 400 },
            { Colors.Cyan, 300 },
            { Colors.Blue, 200 },
            { Colors.Purple, 100 },
        };

        /// <summary>
        /// Property represents a color from the <c>Colors</c> enumeration which is a set of characters. 
        /// </summary>
        public Colors Name { get; private set; }
        /// <summary>
        /// Property stores a comparable value of a color.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Creates an instance of <c>Color</c> from a <c>Colors</c> enumeration member.
        /// </summary>
        /// <param name="color"><c>Colors</c> enumeration member.</param>
        public Color(Colors color)
        {
            this.Name = color;
            this.Value = GetColorsValue(color);
        }

        /// <summary>
        /// Creates an instance of <c>Color</c> from a color's name that should be a part of the <c>Colors</c> enumeration.
        /// </summary>
        /// <param name="colorName">Color's name.</param>
        public Color(char colorName) : this((Colors)Enum.ToObject(typeof(Colors), colorName))
        {

        }

        /// <summary>
        /// Returns a comparable value of a color.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <returns>Comparable value of a color.</returns>
        /// <exception cref="InvalidOperationException">Throws an exception if there are no value for the color.</exception>
        private static int GetColorsValue(Colors color)
        {
            
            if (!_colorsSortOrder.TryGetValue(color, out var value))
                throw new InvalidOperationException($"The value is not defined for the color {color}");

            return value;
        }

        public override bool Equals(object? obj)
        {
            return obj is Color color &&
                   Name == color.Name &&
                   Value == color.Value;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Value);
    }

    /// <summary>
    /// Structure models a card from the Red Seven game.
    /// </summary>
    /// <param name="color">Color of the card.</param>
    /// <param name="number">Numeric value of the card.</param>
    struct Card(Colors color, Numbers number) : IComparable<Card>
    {
        /// <summary>
        /// Color of the card.
        /// </summary>
        public Color Color { get; } = new Color(color);
        /// <summary>
        /// Numeric value of the card.
        /// </summary>
        public Numbers Number { get; } = number;             

        public int CompareTo(Card other)
        {
            int numberComparisson = this.Number.CompareTo(other.Number);

            return numberComparisson != 0 ? numberComparisson : this.Color.Value.CompareTo(other.Color.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is Card card &&
                   EqualityComparer<Color>.Default.Equals(Color, card.Color) &&
                   Number == card.Number;
        }

        public override string ToString() => $"{(int)Number} {(char)Color.Name}";

        public override int GetHashCode() => HashCode.Combine(Number, Color);
    }

    /// <summary>
    /// Models a combination of cards from the Red Seven game.
    /// </summary>
    struct CardCombination
    {
        /// <summary>
        /// List of cards in the combination.
        /// </summary>
        private List<Card> _cards;
        /// <summary>
        /// The highest card in the combination.
        /// </summary>
        private Card? _highestCard;

        /// <summary>
        /// The highest card in the combination.
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if there is no highest card that is possible if the combination is empty.</exception>
        public Card HighestCard
        {
            get
            {
                if (!_highestCard.HasValue)
                    throw new InvalidOperationException("There is no highest card in the card combination.");

                return _highestCard.Value;                
            }
        }

        /// <summary>
        /// Creates an instance of the <c>CardCombination</c> structure.
        /// </summary>
        public CardCombination()
        {
            _cards = new List<Card>();
            _highestCard = null;
        }

        /// <summary>
        /// Adds a card to the combination.
        /// </summary>
        /// <param name="card">Card to add to the combination.</param>
        /// <remarks>
        /// While adding a card there is also a check to define the highest card of the combination performing.
        /// </remarks>
        public void Add(Card card)
        {
            _cards.Add(card);

            if (!_highestCard.HasValue || _highestCard.Value.CompareTo(card) < 0)
                _highestCard = card;
        }

        public override bool Equals(object? obj)
        {
            return obj is CardCombination combination &&
                   EqualityComparer<List<Card>>.Default.Equals(_cards, combination._cards) &&
                   EqualityComparer<Card?>.Default.Equals(_highestCard, combination._highestCard);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_cards, _highestCard);
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
