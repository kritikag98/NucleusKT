using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{   
    class Program
    {
        //Method for validating the inputs for the double datatype
        static double validation()
        {
            double x;
            var xAsString = Console.ReadLine();
            while (!double.TryParse(xAsString, out x))
            {
                Console.WriteLine("This is not a number!");
                xAsString = Console.ReadLine();
            }
            return x;
        }

        //Main method
        static void Main(string[] args)
        {
            //Take input for variable a
            Console.WriteLine("Enter the value of a: ");
            double a = validation();

            //Take input for variable b
            Console.WriteLine("Enter the value of b: ");
            double b = validation();

            //Ask the user to enter the choices accordingly for sum, difference and product
            Console.WriteLine("Enter choice 1 for sum, 2 for difference and 3 for product: ");
            int ch;

            //Validating the input for int type
            var xAsString = Console.ReadLine();
            while (!int.TryParse(xAsString, out ch))
            {
                Console.WriteLine("This is not a number!");
                xAsString = Console.ReadLine();
            }

            //Declaring the result variable
            double res = 0;

            //Switch case
            switch(ch)
            {
                //Print the sum
                case 1: res = a + b;
                    break;

                //Print the difference
                case 2: res = a - b;
                    break;

                //Print the product
                case 3: res = a * b;
                    break;
            }

            Console.WriteLine(String.Format("The {0} is {1}", (ch==1)?"sum":((ch==2)?"difference":(ch==3)?"product":"wrong choice"), res));
            
            //A Readline statement so that the application doesn't close abruptly without displaying the output
            Console.ReadLine();
        }
    }
}
