using System;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Transactions;
using Yocto_Roger_2._1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Yocto_Roger_v._2._1
// Yocto Roger 2.1 ;)
//
//***************
//*Emotion Corp.*
//***************
//Более умная нейросеть с поддержкой Roger API 1.0!
//Как всегда аксолотль, семечка, хамелеон, грибочек.
//По кирпичикам построим мечту!

{
    public class UI
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Configuring console...");
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            sendMessage(ConsoleColor.Magenta, "Emotion :) 2026    Yocto Roger v.2.1");
            Thread.Sleep(3000);
            if (Parameters.isDebug == false)
                sendMessage(ConsoleColor.DarkMagenta, "Welcome to the RogerHub! v.2.1.2");
            else
                sendMessage(ConsoleColor.DarkMagenta, "Welcome to the RogerHub! v.2.1.2 DEBUG MODE");
            while (true)
            {
                Console.Write(" 1. Start Roger in training mode \n 2. Start Roger from the .roger file \n 3. Options for training mode \n 4. Exit of RogerHub \n >>>");
                int userInput = 0;
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    switch (value)
                    {
                        case 1:
                            Console.WriteLine("Starting Roger...");
                            NeuralNetwork.StartAI(0);
                            break;

                        case 2:
                            Console.Clear();
                            Console.Write("1. Place the .roger2 file in the folder with Yocto Roger 2.1.exe\n2. Enter the file name .roger2 (with extension)...");
                            Parameters.roger2 = Console.ReadLine();
                            if (File.Exists(Parameters.roger2))
                                NeuralNetwork.StartAI(1);
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Roger2 doesn't found!");
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            break;

                        case 3:
                            setUp();
                            break;

                        case 4:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("What?");
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect input >:(");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public static int CountLines(string path)
        {
            int count = 0;
            using var reader = new StreamReader(path);
            while (reader.ReadLine() != null)
                count++;
            return count;
        }

        public static void sendMessage(ConsoleColor color, string message)
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = color;

            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(message);
            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void setUp()
        {
            Console.Clear();

            string userInput = "";
            int i = 0;
            while (i == 0)
            {
                Console.Write($"RogerHub Training Options \n 1. Number of middle neurons...{Parameters.middleNeuronsCount} \n 2. Knowledge file...{Parameters.knowledgeFile} \n 3. DropOut sys percent...{Parameters.DropOutPercent}% \n 4. Learning Rate...{Parameters.learningRate} \n 5. Passes...{Parameters.passes} \n 6. Exit \n >>>");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("*MIDDLE NEURONS PARAMETER*");
                        Console.Write("INT16> Enter new middle neurons number (> 0)...");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int userInputChecked))
                        {
                            if (userInputChecked > 0)
                                Parameters.middleNeuronsCount = userInputChecked;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value out of range.");
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("*KNOWLEDGE PARAMETER*");
                        Console.Write("STRING> Enter new knowledge file...");
                        userInput = Console.ReadLine();
                        if(File.Exists(userInput))
                        {
                            Parameters.knowledgeFile = userInput;
                        }
                        else
                        {
                            Console.ForegroundColor= ConsoleColor.Red;
                            Console.WriteLine("Knowledge file doesn't exists");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                            break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("*DROPOUT PERCENT PARAMETER*");
                        Console.Write("INT16> Enter new DropOut percent (0–70)... ");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int newDrop))
                        {
                            if (newDrop >= 0 && newDrop <= 70)
                                Parameters.DropOutPercent = newDrop;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value out of range.");
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input.");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("*LEARNING RATE PARAMETER*");
                        Console.Write("INT32> Enter new learning rate (0.001 – 1.0)... ");
                        userInput = Console.ReadLine();
                        if (float.TryParse(userInput, out float newLR))
                        {
                            if (newLR > 0 && newLR <= 1.0)
                                Parameters.learningRate = newLR;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Learning rate out of range.");
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input.");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("*PASSES PARAMETER*");
                        Console.Write("INT16> Enter passes count (> 0)... ");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int newPasses))
                        {
                            if (newPasses > 0)
                                Parameters.passes = newPasses;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Passes must be greater than zero.");
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input.");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case "6":
                        i++;
                        break;
                }
            }
        }
    }
}
