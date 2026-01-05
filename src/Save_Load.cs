using System;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Transactions;
using Yocto_Roger_v._2._1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Yocto_Roger_2._1
{
    internal class Save_Load
    {
        static bool fileExists = true;
        static string name = "";
        static int temp = 0;
        public static void SaveRoger()
        {
            fileExists = true;
            while (fileExists)
            {
                name = "roger" + temp + ".roger2";
                if (File.Exists(name))
                    temp++;
                else
                    fileExists = false;
            }

            using StreamWriter writer = new StreamWriter(name);
            writer.WriteLine($"[roger]\nAIversion = {Convert.ToString(Parameters.version)}\n");

            //нейроны
            writer.WriteLine($"[neurons]\ninputNeurons = {NeuralNetwork.inputNeurons.Length}\nmiddleNeurons = {NeuralNetwork.middleNeurons.Length}\noutputNeurons = {NeuralNetwork.outputNeurons.Length}");

            //веса
            writer.WriteLine("\n[weights]\nweights1 = ");
            for(int i = 0; i < NeuralNetwork.weights1.GetLength(1); i++)
                for (int j = 0; j < NeuralNetwork.weights1.GetLength(0); j++)
                    writer.Write(NeuralNetwork.weights1[j,i] + "/ ");
            writer.WriteLine("\nweights2 = ");
            for (int i = 0; i < NeuralNetwork.weights2.GetLength(1); i++)
                for (int j = 0; j < NeuralNetwork.weights2.GetLength(0); j++)
                    writer.Write(NeuralNetwork.weights2[j, i] + "/ ");
            //сдвиги
            writer.WriteLine("\n[biases]\nbiases1 = ");
            for (int i = 0; i < NeuralNetwork.bias1.Length; i++)
                writer.Write(NeuralNetwork.bias1[i] + "/ ");
            writer.WriteLine("\nbiases2 = ");
            for (int i = 0; i < NeuralNetwork.bias2.Length; i++)
                writer.Write(NeuralNetwork.bias2[i] + "/ ");
        }

        public static void LoadRoger()
        {
            if (!File.Exists(Parameters.roger2))
            {
                Console.WriteLine("Error loading neural network: file not found");
                return;
            }

            string[] lines = File.ReadAllLines(Parameters.roger2);

            string section = "";

            List<double> w1 = new List<double>();
            List<double> w2 = new List<double>();
            List<double> b1 = new List<double>();
            List<double> b2 = new List<double>();

            int input = 0, middle = 0, output = 0;

            foreach (string raw in lines)
            {
                string line = raw.Trim();
                if (line == "" || line.StartsWith(";"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line;
                    continue;
                }

                if (section == "[roger]")
                {
                    if (line.StartsWith("AIversion"))
                    {
                        string v = line.Split('=')[1].Trim();
                        if (v != Convert.ToString(Parameters.version))
                        {
                            Console.WriteLine("Version not supported");
                            return;
                        }
                    }
                }

                if (section == "[neurons]")
                {
                    if (line.StartsWith("inputNeurons"))
                        input = int.Parse(line.Split('=')[1]);
                    if (line.StartsWith("middleNeurons"))
                        middle = int.Parse(line.Split('=')[1]);
                    if (line.StartsWith("outputNeurons"))
                        output = int.Parse(line.Split('=')[1]);
                }

                if (section == "[weights]")
                {
                    if (line.StartsWith("weights1"))
                        continue;
                    if (line.StartsWith("weights2"))
                        continue;

                    string[] nums = line.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string n in nums)
                    {
                        if (section == "[weights]" && w1.Count < input * middle)
                            w1.Add(double.Parse(n));
                        else
                            w2.Add(double.Parse(n));
                    }
                }

                if (section == "[biases]")
                {
                    if (line.StartsWith("biases1"))
                        continue;
                    if (line.StartsWith("biases2"))
                        continue;

                    string[] nums = line.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string n in nums)
                    {
                        if (b1.Count < middle)
                            b1.Add(double.Parse(n));
                        else
                            b2.Add(double.Parse(n));
                    }
                }
            }

            int index = 0;
            for (int j = 0; j < middle; j++)
                for (int i = 0; i < input; i++)
                    NeuralNetwork.weights1[i, j] = w1[index++];

            index = 0;
            for (int j = 0; j < output; j++)
                for (int i = 0; i < middle; i++)
                    NeuralNetwork.weights2[i, j] = w2[index++];

            for (int i = 0; i < middle; i++)
                NeuralNetwork.bias1[i] = b1[i];

            for (int i = 0; i < output; i++)
                NeuralNetwork.bias2[i] = b2[i];

            Console.Write("Done.\n");
        }
    }
}
