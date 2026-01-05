using System;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Transactions;
using Yocto_Roger_2._1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Yocto_Roger_v._2._1
{

    internal class AIMath
    {
        static public Random rand = new Random();
        public static void Rounding(ref double[] neurons)
        {
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = (int)Math.Round(neurons[i]);
        }
        public static double sigmoida(double value) //активация
        {
            double answer = 1.0 / (1.0 + Math.Exp(-value));
            if (Parameters.isDebug)
                Console.WriteLine("Sigmoida> " + answer);
            return answer;
        }
        public static int writeOutput(double[] binary) //двиучное в десятичное
        {
            int result = 0;
            for (int i = 0; i < binary.Length; i++)
                if (binary[i] != 0.0)
                    result += 1 << (7 - i);
            return result;
        }
        public static int[] WriteInput(ref double[] inNeurons, int? v1 = null, int? v2 = null) //конвертация десятичного числа в двиучное
        {
            int[] values = new int[2];

            if (v1.HasValue && v2.HasValue)
            {
                values[0] = v1.Value;
                values[1] = v2.Value;
            }
            else
            {
                Console.Write("Enter first value -> ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int correctInput))
                    values[0] = correctInput;
                else if (input == "save")
                    Save_Load.SaveRoger();
                else
                    Console.WriteLine("Incorrect input!");

                Console.Write("Enter second value -> ");
                input = Console.ReadLine();
                if (int.TryParse(input, out int correctInput2))
                    values[1] = correctInput2;
                else if (input == "save")
                    Save_Load.SaveRoger();
                else
                    Console.WriteLine("Incorrect input!");
            }
            if (Parameters.isDebug)
                Console.Write("Recorded in the initial neurons - ");
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    int mask = 1 << (6 - j);
                    inNeurons[i * 7 + j] = (values[i] & mask) != 0 ? 1.0 : 0.0;
                    if (Parameters.isDebug)
                        Console.Write(inNeurons[i * 7 + j]);
                }
            }

            return values;
        }
    }
}
