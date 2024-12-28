using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedSevenGameBase
{
    /// <summary>
    /// <c>Presenter</c> implements methods for bidirectional data transmition with the <c>Console</c>.
    /// </summary>
    internal class Presenter : IPresenter
    {
        /// <summary>
        /// Continuosly asks user to input data of type <c>T</c> in the text format until user provides correct data.
        /// </summary>
        /// <typeparam name="T">Type of data to return.</typeparam>
        /// <param name="prompt">Prompt to user.</param>
        /// <param name="converter">Function should convert given string to <c>T</c>.</param>
        /// <returns>Instance of <c>T</c>.</returns>
        private T GetUserInput<T>(string prompt, Converter<string, T> converter)
        {
            T result;
            while (true)
            {
                try
                {
                    Console.Write(prompt);
                    var userInput = Console.ReadLine();

                    result = converter.Invoke(userInput);

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Try again!");
                }
            }
        }

        /// <summary>
        /// Returns a card from user.
        /// </summary>
        /// <param name="prompt">Prompt to show.</param>
        /// <returns><c>Card</c> instance.</returns>
        public Card GetCard(string prompt)
        {
            return GetUserInput(prompt, (text) => new Card(text));
        }

        /// <summary>
        /// Returns a combination length from user.
        /// </summary>
        /// <param name="prompt">Prompt to show.</param>
        /// <returns>Combination length.</returns>
        public int GetCombinationLength(string prompt)
        {
            return GetUserInput(prompt, (text) => int.Parse(text));
        }     

        /// <summary>
        /// Prints a <c>message</c> to console.
        /// </summary>
        /// <param name="message">Message to print.</param>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
