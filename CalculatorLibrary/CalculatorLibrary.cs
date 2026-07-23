using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary;

public class Calculator
{
    readonly JsonWriter writer;

    public Calculator()
    {
        StreamWriter logFile = File.CreateText("calculatorlog.json");
        logFile.AutoFlush = true;
        writer = new JsonTextWriter(logFile)
        {
            Formatting = Formatting.Indented
        };
        writer.WriteStartObject();
        writer.WritePropertyName("Operations");
        writer.WriteStartArray();
    }

    public double DoOperation(double num1, double num2, string op)
    {
        double result = double.NaN;

        writer.WriteStartObject();
        writer.WritePropertyName("Operand1");
        writer.WriteValue(num1);
        writer.WritePropertyName("Operand2");
        writer.WriteValue(num2);
        writer.WritePropertyName("Operation");

        switch (op)
        {
            case "a":
                result = num1 + num2;
                writer.WriteValue("Add");
                break;
            case "s":
                result = num1 - num2;
                writer.WriteValue("Subtract");
                break;
            case "m":
                result = num1 * num2;
                writer.WriteValue("Multiply");
                break;
            case "d":
                if (num2 != 0)
                    result = num1 / num2;
                writer.WriteValue("Divide");
                break;
            case "mo":
                if (num2 != 0)
                    result = num1 % num2;
                writer.WriteValue("Modulo");
                break;
            case "e":
                result = Math.Pow(num1, num2);
                writer.WriteValue("Exponentiation");
                break;
            case "r":
                if (num2 > 0)
                    result = Math.Pow(num1, 1.0 / num2);
                writer.WriteValue("Root");
                break;
            default:
                break;
        }
        writer.WritePropertyName("Result");
        writer.WriteValue(result);
        writer.WriteEndObject();
        return result;
    }

    public void Finish(int lenght)
    {
        writer.WriteEndArray();
        writer.WritePropertyName("Number of uses");
        writer.WriteValue(lenght);
        writer.WriteEndObject();
        writer.Close();
    }
}