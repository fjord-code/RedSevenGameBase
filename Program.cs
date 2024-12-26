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
        /// <value>
        /// A mapping between colors and their comparable values.
        /// </value>
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

        /// <value>
        /// Property represents a color from the <c>Colors</c> enumeration which is a set of characters. 
        /// </value>
        public Colors Name { get; private set; }
        /// <value>
        /// Property stores a comparable value of a color.
        /// </value>
        public int Value { get; private set; }

        /// <summary>
        /// Creates an instance of <c>Color</c> from a <c>Colors</c> enumeration member.
        /// </summary>
        /// <param name="color"><c>Colors</c> enumeration member.</param>
        public Color(Colors color)
        {
            this.Name = color;
            this.Value = GetColorValue(color);
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
        private static int GetColorValue(Colors color)
        {

            if (!_colorsSortOrder.TryGetValue(color, out var value))
            {
                throw new InvalidOperationException($"The value is not defined for the color {color}");
            }

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
    struct Card : IComparable<Card>
    {
        /// <value>
        /// Color of the card.
        /// </value>
        public Color Color { get; }
        /// <value>
        /// Numeric value of the card.
        /// </value>
        public Numbers Number { get; }

        /// <summary>
        /// Creates an instance of the <c>Card</c> structure from the <c>color</c> and <c>number</c>.
        /// </summary>
        /// <param name="color">Color of the card.</param>
        /// <param name="number">Numeric value of the card.</param>
        public Card(Colors color, Numbers number)
        {
            Color = new Color(color);
            Number = number;
        }

        /// <summary>
        /// Creates a <c>Card</c> instance from its string representation.
        /// </summary>
        /// <param name="card">String representation of the card.</param>
        /// <remarks>The string representation must contain a number and a color separated by a space, e.g. "1 P", "2 C", ...</remarks>
        /// <example>
        /// <code>
        /// Card card = new Card("7 R");
        /// </code>
        /// </example>
        public Card(string card)
        {            
            try
            {
                var values = card.Split(' ');

                Number = (Numbers)Enum.ToObject(typeof(Numbers), int.Parse(values[0]));
                Color = new Color(values[2][0]);
            }
            catch 
            {
                throw new InvalidOperationException("Invalid card format. The format should be a number and a color separated by a space. E.g.: \"1 R\"");
            }            
        }

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
        /// <value>
        /// List of cards in the combination.
        /// </value>
        private List<Card> _cards;
        /// <value>
        /// The highest card in the combination.
        /// </value>
        private Card? _highestCard;

        /// <value>
        /// The highest card in the combination.
        /// </value>
        /// <exception cref="InvalidOperationException">Throws if there is no highest card that is possible if the combination is empty.</exception>
        public Card HighestCard
        {
            get
            {
                if (!_highestCard.HasValue)
                {
                    throw new InvalidOperationException("There is no highest card in the card combination.");
                }

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
            {
                _highestCard = card;
            }
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

    class GameEventArgs : EventArgs
    {
        public string? GameMessage { get; }

        public GameEventArgs(string gameMessage)
        {
            GameMessage = gameMessage;
        }
    }

    interface IGame
    {
        public delegate Card GettingCardEventHandler(object sender, GameEventArgs e);
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

            var highestCardsComparisson = firstCombination.HighestCard.CompareTo(secondCombination.HighestCard);
            if (highestCardsComparisson > 0)
            {
                Console.WriteLine($"The first combination wins.\n{firstCombination.HighestCard}");
            }
            else if (highestCardsComparisson == 0)
            {
                Console.WriteLine("The game is tied.");
            }
            else
            {
                Console.WriteLine($"The second combination wins.\n{secondCombination.HighestCard}");
            }
        }

        static void Main(string[] args)
        {
            StartGame();
        }
    }
}
