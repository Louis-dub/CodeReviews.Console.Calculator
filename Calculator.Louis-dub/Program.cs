using CalculatorLibrary;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace CalculatorProgram;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        Calculator calculator = new();
        int length = 0;
        while (!endApp)
        {
            string? numInput1 = "";
            string? numInput2 = "";
            double result = 0;
            string? action = "";

            while (action != "D" && action != "C")
            {
                Console.Write("Type 'D' to clear your calculation history, or type 'C' to use Calculator: ");
                action = Console.ReadLine();
            }

            if (action == "D")
            {
                try
                {
                    File.Delete("calculatorlog.json");
                    calculator.Finish(length);
                    calculator = new();
                    length = 0;
                    Console.WriteLine("History successfully cleared.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.Write("Type a number, and then press Enter: ");
            numInput1 = Console.ReadLine();

            double cleanNum1 = 0;
            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter an integer value: ");
                numInput1 = Console.ReadLine();
            }

            Console.Write("Type another number, and then press Enter: ");
            numInput2 = Console.ReadLine();

            double cleanNum2 = 0;
            while (!double.TryParse(numInput2, out cleanNum2))
            {
                Console.Write("This is not valid input. Please enter an integer value: ");
                numInput2 = Console.ReadLine();
            }

            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.WriteLine("\tmo - Modulo");
            Console.WriteLine("\te - Exponentiation");
            Console.WriteLine("\tr - Root");
            Console.Write("Your option? ");

            string? op = Console.ReadLine();

            if (op == null || ! Regex.IsMatch(op, "^(a|s|m|d|mo|e|r)$"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            { 
                try
                {
                    result = calculator.DoOperation(cleanNum1, cleanNum2, op); 
                    length++;
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else Console.WriteLine("Your result: {0:0.##}\n", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            }
            Console.WriteLine("------------------------\n");

            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;

            Console.WriteLine("\n");
        }
        calculator.Finish(length);
        return;
    }
}