using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Отключаем курсор.
            Console.CursorVisible = false;

            // Инициализируем шаблон карты.
            char[,] map = ReadMap("C:\\Users\\Пикачу\\source\\repos\\Game\\map\\map.txt");

            // Создаем переменную pressedKey для записи и обработки нажатой клавиши.
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();

            // Запускаем второй поток для записи в переменную pressedKey значение нажатой клавиши.
            Task.Run(() => { while(true)pressedKey = Console.ReadKey(); });

            // Задаем defolt координаты игрока по X,Y.
            int packmanX = 1;
            int packmanY = 1;

            // Задаем defolt значение очков.
            int score = 0;

            // Цикл покадровой отрисовки логики игры.
            Kvadrat(map);
            while (true)
            {
                Console.Clear();
                StepAndScore(pressedKey, ref packmanX, ref packmanY, map, ref score);
                DrowMap(map, ConsoleColor.Blue);
                if (score == 11)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("YOU WIN!");
                    break;
                }
                Console.SetCursorPosition(packmanX, packmanY);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("@");
                Console.SetCursorPosition(35, 0);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Score: {score}");
                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// Метод который: Изменяет координаты игрока, проверяет на столкновение с объектами, записывает счёт при столкновении с точкой.
        /// </summary>
        /// <param name="pressedKey">Параметр нажатой клавиши.</param>
        /// <param name="packmanX">Ссылка на хранение значения координаты игрока по оси X.</param>
        /// <param name="packmanY">Ссылка на хранение значения координаты игрока по оси Y.</param>
        /// <param name="map">Инициализация карты.</param>
        /// <param name="score">Ссылка на запись счёта.</param>
        private static void StepAndScore(ConsoleKeyInfo pressedKey, ref int packmanX, ref int packmanY, char[,] map, ref int score)
        {
            int[] direction = GetDirection(pressedKey);

            int nextPositionX = packmanX + direction[0];
            int nextPositionY = packmanY + direction[1];

            char nextStep = map[nextPositionX, nextPositionY];

            if (nextStep == ' ' || nextStep == '.')
            {
                packmanX = nextPositionX;
                packmanY = nextPositionY;

                if(nextStep == '.')
                {
                    score++;
                    map[nextPositionX, nextPositionY] = ' ';
                }
            }
        }

        /// <summary>
        /// Метод для установки направления игрока.
        /// </summary>
        /// <param name="pressedKey">Параметр нажатой клавиши.</param>
        /// <returns>Массив с данными о направлении.</returns>
        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0,0 };

            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                direction[1] = -1;
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                direction[1] = 1;
            }
            else if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                direction[0] = 1;
            }
            else if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                direction[0] = -1;
            }

            return direction;
        }

        /// <summary>
        /// Метод отрисовки карты по полученному массиву символов и их координат на плоскости X,Y.
        /// </summary>
        /// <param name="map">Массив записанной карты.</param>
        /// <param name="color">Цвет отрисовки стен.</param>
        private static void DrowMap(char[,] map, ConsoleColor color)
        {

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.ForegroundColor = color;
                    if (map[x, y] == '.')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(map[x, y]);
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Метод чтения карты. Записывает полученный массив символов и перезаписывает его в новый двумерный массив для смены координатов в нужную форму.
        /// </summary>
        /// <param name="path">Ссылка на файл формата .txt.</param>
        /// <returns>Двумерный массив с точными координатами максимального размера карты.</returns>
        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);
            char[,] map = new char[InizialyzSizeMap(file), file.Length];

            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = file[y][x];
                }
            }

            return map;
        }
        private static void Kvadrat(char[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == '#')
                    {
                        map[x, y] = '█';
                    }
                }
            }
        }

        /// <summary>
        /// Алгоритм определения истинного размера карты.
        /// </summary>
        /// <param name="lines">Массив строки.</param>
        /// <returns>Максимальный размер карты.</returns>
        private static int InizialyzSizeMap(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (var line in lines)
            {
                if(line.Length > maxLength)
                {
                    maxLength = line.Length;
                }
            }

            return maxLength;
        }
    }
}
