using System;
using System.Collections.Generic;
using System.Linq;

namespace BrickWork
{
    class StartUp
    {
        static void Main(string[] args)
        {
            // Input for the width and length of the brick area
            var input = Console.ReadLine()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            // Brick area width
            var width = int.Parse(input[0]);

            // Brick area length
            var length = int.Parse(input[1]);

            // Validation of the width and length of the brick area
            if (width % 2 != 0 || length % 2 != 0
                || width > 100 || length > 100 || width <= 0 || length <= 0)
            {
                Console.WriteLine("Invalid input!");
                Console.WriteLine("The width and length should be between 1 and 100 and should be even numbers!");
                return;
            }

            // First layer of the Brick Work
            var firstLayer = new int[width, length];


            // Using try catch to check the input for possible errors
            try
            {
                for (int col = 0; col < width; col++)
                {
                    // Input for the lines/cols of the layer
                    var inputLine = Console.ReadLine()
                        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();

                    // Making the rows of the layer
                    for (int row = 0; row < inputLine.Length; row++)
                    {
                        firstLayer[col, row] = inputLine[row];
                    }
                }
                // Checking for empty space in input
                var isInputLayerFilled = IsLayerFilled(firstLayer);
                if (!isInputLayerFilled)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong input! Try again.");
                return;
            }

            //Validating for Triple bricks.
            var validation = TripleBricksValidator(firstLayer);
            if (validation)
            {
                Console.WriteLine("Invalid input! Triple brick detected.");
                Console.WriteLine("End of program!");
                return;
            }

            // Making second layer
            // List that will contain all bricks.
            var numberOfBricks = new List<int>();

            // Filling the list with the bricks.
            foreach (var number in firstLayer)
            {
                numberOfBricks.Add(number);
            }
            numberOfBricks = numberOfBricks.Distinct().ToList();

            // Creating second layer of the brick work.
            int[,] secondLayer = CreateSecondLayer(width, length, numberOfBricks, firstLayer);

            // Checking the new layer for possible errors.
            var validationForTriples = TripleBricksValidator(secondLayer);
            var validationIfEmpty = IsLayerFilled(secondLayer);
            if (validationForTriples || !validationIfEmpty)
            {
                Console.WriteLine("-1. Solution not found!");
                return;
            }

            // Printing the new layer
            PrintLayer(secondLayer, width, length);
        }

        // Method for filling the second layer.
        private static int[,] CreateSecondLayer(int width, int length, List<int> bricks, int[,] firstLayer)
        {
            // Second layer of the brick work.
            int[,] layer = new int[width, length];

            // Filling the second layer.
            // We start from the last index to assure that the first brick is not the same as in the first layer.
            for (int i = width - 1; i > 0; i--)
            {
                // Checking the current place for brick. If its taken continue with the next one.
                if (layer[i, length - 1] != 0)
                {
                    continue;
                }
                for (int j = length - 1; j > 0; j -= 2)
                {
                    // Checking if the brick is horizontal or vertical.
                    if (firstLayer[i, j] == firstLayer[i, j - 1])
                    {
                        // If horizontal!
                        // Filling 2 vertical bricks to assure that the place of the old one is filled with different bricks.
                        layer[i, j] = bricks[0];
                        layer[i - 1, j] = bricks[0];
                        layer[i, j - 1] = bricks[1];
                        layer[i - 1, j - 1] = bricks[1];

                        // Removing them from our list.
                        bricks.RemoveAt(0);
                        bricks.RemoveAt(0);
                    }
                    else
                    {
                        // If vertical!
                        // Filling 2 horizontal bricks to assure that the place of the old one is filled with different bricks.
                        layer[i, j] = bricks[0];
                        layer[i, j - 1] = bricks[0];
                        layer[i - 1, j] = bricks[1];
                        layer[i - 1, j - 1] = bricks[1];

                        // Removing them from our list.
                        bricks.RemoveAt(0);
                        bricks.RemoveAt(0);
                    }
                }
            }

            return layer;
        }

        // Method for printing the layer
        private static void PrintLayer(int[,] layer, int width, int length)
        {
            for (int line = 0; line < width; line++)
            {
                for (int row = 0; row < length; row++)
                {
                    Console.Write($"{layer[line, row]}");
                    if (row != length - 1)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine(Environment.NewLine);
            }
        }

        // Validator for triple bricks.
        private static bool TripleBricksValidator(int[,] layer)
        {
            // Boolean to determine whetever there are triple bricks or not.
            // True for error.
            var areTriple = false;

            // Converting the matrix into IEnumberable<int> so we can use LINQ
            var layerQuery = from int item in layer
                             select item;

            foreach (var item in layer)
            {
                // The length of the brick
                var count = layerQuery.Where(x => x == item).ToList().Count();
                // Checking if it extends the given limitation.
                if (count > 2)
                {
                    areTriple = true;
                    break;
                }
            }

            return areTriple;
        }

        // Validator for empty spaces.
        private static bool IsLayerFilled(int[,] layer)
        {
            // Boolean to determine whetever the layer contains empty spaces or not.
            // False for error.
            var isFilled = true;

            // Converting the matrix into IEnumberable<int> so we can use LINQ
            var layerQuery = from int item in layer
                             select item;

            //Checking for empty spaces.
            if (layerQuery.Contains(0))
            {
                isFilled = false;
            }

            return isFilled;
        }
    }
}
