using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the value of a: ");
            var aAsString = Console.ReadLine();
            double a;
            while (!double.TryParse(aAsString, out a))
            {
                Console.WriteLine("This is not a number!");
                aAsString = Console.ReadLine();
            }
            Console.WriteLine("Enter the value of b: ");
            var bAsString = Console.ReadLine();
            double b;
            while (!double.TryParse(bAsString, out b))
            {
                Console.WriteLine("This is not a number!");
                bAsString = Console.ReadLine();
            }
            Console.WriteLine("Enter choice 1 for sum, 2 for difference and 3 for product: ");
            int ch = Convert.ToInt32(Console.ReadLine());
            double res = 0;
            switch(ch)
            {
                case 1: res = a + b;
                    break;
                case 2: res = a - b;
                    break;
                case 3: res = a * b;
                    break;
                default: Console.WriteLine("Wrong choice");
                    break;
            }
            Console.WriteLine("The result is: " + res);
            Console.ReadLine();
        }
    }
}
