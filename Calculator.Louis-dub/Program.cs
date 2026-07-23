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
        List<double> history = [];
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
                    calculator.Finish(history.Count);
                    calculator = new();
                    history = [];
                    Console.WriteLine("History successfully cleared.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            if (history.Count > 0)
            {
                Console.WriteLine("\nRecent results:");
                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine($"  [r{i + 1}] {history[i]}");
                }
                Console.WriteLine("Tip: type a number, or 'r' followed by its number above (e.g. r1) to reuse a result.\n");
            }

            Console.Write("Type a number, and then press Enter: ");
            numInput1 = Console.ReadLine();

            double cleanNum1 = FindResult(history, numInput1);
            while (double.IsNaN(cleanNum1) && !double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("Invalid input. Enter a number, or rN to reuse result N (e.g. r1): ");
                numInput1 = Console.ReadLine();
                cleanNum1 = FindResult(history, numInput1);
            }

            Console.Write("Type another number, and then press Enter: ");
            numInput2 = Console.ReadLine();

            double cleanNum2 = FindResult(history, numInput2);
            while (double.IsNaN(cleanNum2) && !double.TryParse(numInput2, out cleanNum2))
            {
                Console.Write("Invalid input. Enter a number, or rN to reuse result N (e.g. r1): ");
                numInput2 = Console.ReadLine();
                cleanNum2 = FindResult(history, numInput2);
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
                    history.Add(result);
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
        calculator.Finish(history.Count);
        return;
    }

    static double FindResult(List<double> history, string? r)
    {
        int id = 0;
        bool end = false;

        if (r?[0] == 'r' && r.Length >= 2) {
            for (int i = 1; i < r.Length && !end; i++)
            {
                if (r[i] < '0' || r[i] > '9')
                    end = true;
                else
                    id = id * 10 + (r[i] - '0');
            }
            if (!end && id - 1 < history.Count)
                return history[id - 1];
        }
        return double.NaN;
    }
}