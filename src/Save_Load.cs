using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Yocto_Roger_v._2._1;

namespace Yocto_Roger_2._1
{
    internal static class Save_Load
    {
        public static void SaveRoger()
        {
            string fileName;
            int index = 0;

            do
            {
                fileName = $"roger{index}.roger2";
                index++;
            }
            while (File.Exists(fileName));

            using StreamWriter writer = new StreamWriter(fileName);

            writer.WriteLine("[roger]");
            writer.WriteLine($"AIversion = {Parameters.version}");
            writer.WriteLine();

            writer.WriteLine("[neurons]");
            writer.WriteLine($"inputNeurons = {NeuralNetwork.inputNeurons.Length}");
            writer.WriteLine($"middleNeurons = {NeuralNetwork.middleNeurons.Length}");
            writer.WriteLine($"outputNeurons = {NeuralNetwork.outputNeurons.Length}");
            writer.WriteLine();

            writer.WriteLine("[weights]");
            writer.WriteLine("weights1 =");
            WriteMatrix(writer, NeuralNetwork.weights1);

            writer.WriteLine("weights2 =");
            WriteMatrix(writer, NeuralNetwork.weights2);

            writer.WriteLine("[biases]");
            writer.WriteLine("biases1 =");
            WriteArray(writer, NeuralNetwork.bias1);

            writer.WriteLine("biases2 =");
            WriteArray(writer, NeuralNetwork.bias2);
        }

        private static void WriteMatrix(StreamWriter writer, double[,] matrix)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                    writer.Write(matrix[i, j].ToString(CultureInfo.InvariantCulture) + "/ ");
                writer.WriteLine();
            }
        }

        private static void WriteArray(StreamWriter writer, double[] array)
        {
            foreach (double v in array)
                writer.Write(v.ToString(CultureInfo.InvariantCulture) + "/ ");
            writer.WriteLine();
        }

        public static void LoadRoger()
        {
            if (!File.Exists(Parameters.roger2))
                throw new FileNotFoundException("Roger file not found");

            int input = 0, middle = 0, output = 0;
            int aiVersion = -1;

            List<double> w1 = new();
            List<double> w2 = new();
            List<double> b1 = new();
            List<double> b2 = new();

            string section = "";
            string target = "";

            foreach (string raw in File.ReadAllLines(Parameters.roger2))
            {
                string line = raw.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                if (line.StartsWith("["))
                {
                    section = line;
                    continue;
                }

                if (line.Contains("="))
                {
                    var parts = line.Split('=', 2);
                    target = parts[0].Trim();
                    line = parts[1].Trim();
                }

                if (section == "[roger]" && target == "AIversion")
                {
                    aiVersion = int.Parse(line, CultureInfo.InvariantCulture);
                    continue;
                }

                if (section == "[neurons]")
                {
                    int val = int.Parse(line, CultureInfo.InvariantCulture);

                    if (target == "inputNeurons") input = val;
                    if (target == "middleNeurons") middle = val;
                    if (target == "outputNeurons") output = val;

                    continue;
                }

                string[] nums = line.Split('/', StringSplitOptions.RemoveEmptyEntries);
                foreach (string n in nums)
                {
                    double v = double.Parse(n.Trim(), CultureInfo.InvariantCulture);

                    switch (target)
                    {
                        case "weights1": w1.Add(v); break;
                        case "weights2": w2.Add(v); break;
                        case "biases1": b1.Add(v); break;
                        case "biases2": b2.Add(v); break;
                    }
                }
            }

            if (aiVersion != 2)
                throw new Exception($"Unsupported AI version: {aiVersion}");

            NeuralNetwork.inputNeurons = new double[input];
            NeuralNetwork.middleNeurons = new double[middle];
            NeuralNetwork.outputNeurons = new double[output];

            NeuralNetwork.weights1 = new double[input, middle];
            NeuralNetwork.weights2 = new double[middle, output];

            NeuralNetwork.bias1 = new double[middle];
            NeuralNetwork.bias2 = new double[output];

            int idx = 0;
            for (int j = 0; j < middle; j++)
                for (int i = 0; i < input; i++)
                    NeuralNetwork.weights1[i, j] = w1[idx++];

            idx = 0;
            for (int j = 0; j < output; j++)
                for (int i = 0; i < middle; i++)
                    NeuralNetwork.weights2[i, j] = w2[idx++];

            for (int i = 0; i < middle; i++)
                NeuralNetwork.bias1[i] = b1[i];

            for (int i = 0; i < output; i++)
                NeuralNetwork.bias2[i] = b2[i];

            Console.WriteLine("Roger load successful.");
        }

    }
}
