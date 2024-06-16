using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static int width = 20;
        static int height = 20;
        static int[,] grid;
        static List<(int x, int y)> snake = new List<(int, int)>();
        static (int x, int y) food;
        static (int x, int y) direction = (0, 1); // Di chuyển ban đầu sang phải
        static bool gameOver = false;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            grid = new int[width, height];
            InitializeGame();

            while (!gameOver)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(100); // Chậm lại tốc độ game
            }

            Console.Clear();
            Console.WriteLine("Game Over! Score: " + (snake.Count - 1));
        }

        static void InitializeGame()
        {
            snake.Add((width / 2, height / 2)); // Bắt đầu ở giữa màn hình
            GenerateFood();
        }

        static void Draw()
        {
            Console.Clear();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                    {
                        Console.Write("#"); // Vẽ tường
                    }
                    else if (snake.Any(s => s.x == j && s.y == i))
                    {
                        Console.Write("O"); // Vẽ rắn
                    }
                    else if (food.x == j && food.y == i)
                    {
                        Console.Write("F"); // Vẽ mồi
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction != (1, 0)) direction = (-1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != (-1, 0)) direction = (1, 0);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != (0, 1)) direction = (0, -1);
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != (0, -1)) direction = (0, 1);
                        break;
                }
            }
        }

        static void Logic()
        {
            var head = snake.First();
            var newHead = (head.x + direction.x, head.y + direction.y);

            // Kiểm tra va chạm với tường hoặc chính nó
            if (newHead.x <= 0 || newHead.x >= width - 1 || newHead.y <= 0 || newHead.y >= height - 1 || snake.Any(s => s == newHead))
            {
                gameOver = true;
                return;
            }

            snake.Insert(0, newHead);

            // Kiểm tra nếu ăn mồi
            if (newHead == food)
            {
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void GenerateFood()
        {
            Random rand = new Random();
            do
            {
                food = (rand.Next(1, width - 1), rand.Next(1, height - 1));
            } while (snake.Any(s => s == food));
        }
    }
}
