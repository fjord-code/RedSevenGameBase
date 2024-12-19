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
                throw new InvalidOperationException($"The value is not defined for the color {nameof(color)}");

            return _colorsSortOrder[color];
        }
    }

    struct Card(Colors color, Numbers number)
    {    
        public Color Color { get; } = new Color(color);
        public Numbers Number { get; } = number;     

        private class SortCardAscendingHelper : IComparer<Card>
        {
            int IComparer<Card>.Compare(Card x, Card y)
            {
                int numberComparisson = x.Number.CompareTo(y.Number);

                return numberComparisson != 0 ? numberComparisson : x.Color.Value.CompareTo(y.Color.Value);
            }
        }

        public static IComparer<Card> SortCardAscending => new SortCardAscendingHelper();
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
