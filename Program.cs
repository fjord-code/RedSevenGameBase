using System.Runtime.CompilerServices;

namespace RedSevenGameBase
{  
    internal class Program
    {
        static void Main(string[] args)
        {
            var controller = new Controller<RedSevenGame, Presenter>();

            controller.StartGame();
        }
    }
}
