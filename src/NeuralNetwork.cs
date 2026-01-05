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
    internal class NeuralNetwork
    {
        public static int[,] educationArray;

        public static double[] inputNeurons = new double[14];
        public static double[] middleNeurons = new double[Parameters.middleNeuronsCount];
        public static double[] outputNeurons = new double[8];

        public static double[,] weights1 = new double[inputNeurons.Length, middleNeurons.Length];
        public static double[,] weights2 = new double[middleNeurons.Length, outputNeurons.Length];

        public static double[] bias1 = new double[middleNeurons.Length];
        public static double[] bias2 = new double[outputNeurons.Length];

        public static void StartAI(int mode)
        {
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                Console.WriteLine("SetUp education array...");
                educationArray = new int[UI.CountLines(Parameters.knowledgeFile), 3];
                Console.WriteLine("Read knowledge...");
                Training.writeEducationArray(ref educationArray, Parameters.knowledgeFile);
                Console.WriteLine("Set up Weights... (1/2)");
                Training.setUpWeights(ref weights1);
                Console.WriteLine("Set up Weights... (2/2)");
                Training.setUpWeights(ref weights2);
                Console.WriteLine("Set up Biases... (1/2)");
                Training.setUpBiases(ref bias1);
                Console.WriteLine("Set up Biases... (2/2)");
                Training.setUpBiases(ref bias2);
                Console.WriteLine("Education. This make takes few minutes.");
                UI.sendMessage(ConsoleColor.DarkRed, "Education Roger... This may take a few minutes.");
                Training.educationWithTeacher();
                    Array.Clear(educationArray, 0, educationArray.Length);
                    break;

                case 1:
                    Console.Write("Loading your Roger...");
                    Save_Load.LoadRoger();
                    break;
            }
            Console.WriteLine("Hello! I'm Roger, the MLP AI from Emotion!");
            while (true)
            {
                UI.sendMessage(ConsoleColor.DarkGreen, "Ready.   >>> Enter SAVE for saving Roger to .roger2 file");
                AIMath.WriteInput(ref inputNeurons);
                UI.sendMessage(ConsoleColor.DarkRed, "Calculation neurons... (1/2)");
                sumWeights(ref weights1, ref inputNeurons, ref middleNeurons, bias1);
                UI.sendMessage(ConsoleColor.DarkRed, "Calculation neurons... (2/2)");
                sumWeights(ref weights2, ref middleNeurons, ref outputNeurons, bias2);
                UI.sendMessage(ConsoleColor.DarkRed, "Rounding...");
                AIMath.Rounding(ref outputNeurons);
                UI.sendMessage(ConsoleColor.DarkRed, "Almost ready...");
                Console.WriteLine($"I think it's {AIMath.writeOutput(outputNeurons)}");
                Console.WriteLine("Press any key to continue...");
                UI.sendMessage(ConsoleColor.Magenta, "Waiting.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static float[] generateDropOut()
        {
            float[] masks = new float[Parameters.middleNeuronsCount];
            float keepProb = 1.00f - (Parameters.DropOutPercent * 0.01f);

            if (Parameters.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.Length; i++)
                    masks[i] = 1.0f;
                return masks;
            }
            else
            {
                for (int i = 0; i < masks.Length; i++)
                {
                    if (AIMath.rand.Next(0, 100) < Parameters.DropOutPercent)
                        masks[i] = 0;
                    else
                        masks[i] = 1.0f / keepProb;
                }
            }    
            return masks;
        }

        public static void sumWeights(ref double[,] oldweights, ref double[] oldNeurons, ref double[] newNeurons, double[] biases) //нахождение новых нейронов
        {
            if (Parameters.isDebug)
                Console.Write("Sum of weights - ");
            for (int i = 0; i < newNeurons.Length; i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[i];
                newNeurons[i] = AIMath.sigmoida(temp);
                if (Parameters.isDebug)
                    Console.Write(newNeurons[i] + " ");
            }
            if (Parameters.isDebug)
                Console.WriteLine();
        }
    }
}
