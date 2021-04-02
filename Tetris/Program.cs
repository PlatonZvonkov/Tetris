using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Media;

namespace Tetris
{
     class Program
    {    
        
            public static int[,] area = new int[23, 10];
            public static int[,] positionOnGrid = new int[23, 10];
            public static string block = "■";
            public static bool letFall = false;
            public static Stopwatch timer = new Stopwatch();
            public static Stopwatch timerC = new Stopwatch();
            public static Stopwatch timerE = new Stopwatch();
            public static int time, speed = 300;
            static Tetrimino tetrimino;
            static Tetrimino nextTetrimino;
            public static ConsoleKeyInfo key;
            public static bool keyPulse = false;
            public static int lineScore = 0, points = 0, level = 1;
        
         static void Main()
        {
            
            GameArea();
            Console.SetCursorPosition(4, 5);
            Console.WriteLine("Press Any Key");
            Console.ReadKey(true);
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = Environment.CurrentDirectory + "\\Tetris.wav";
            sp.PlayLooping();
            timer.Start();
            timerC.Start();
            _ = timer.ElapsedMilliseconds;
            Console.SetCursorPosition(25, 0);
            Console.WriteLine("Level: " + level);
            Console.SetCursorPosition(25, 1);
            Console.WriteLine("Points: " + points);
            Console.SetCursorPosition(25, 2);
            Console.WriteLine("Score: " + lineScore);
            nextTetrimino = new Tetrimino();
            tetrimino = nextTetrimino;
            tetrimino.Appeareance();
            nextTetrimino = new Tetrimino();

            Update();
            sp.Stop();
            sp.SoundLocation = Environment.CurrentDirectory + "\\Retro-game-over-sound-effect.wav";
            sp.Play();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game Over \n Start New Game? (Y/N)");
            string teclaPulsada = Console.ReadLine();

            if (teclaPulsada.ToUpper() == "Y")
            {
                int[,] area = new int[23, 10];
                positionOnGrid = new int[23, 10];
                timer = new Stopwatch();
                timerC = new Stopwatch();
                timerE = new Stopwatch();
                speed = 300;
                letFall = false;
                keyPulse = false;
                lineScore = 0;
                points = 0;
                level = 1;
                GC.Collect();
                Console.Clear();
                Main();
            }
            else return;

        }
        private static void BlockMover() {
            int combo = 0;
            for (int i = 0; i < 23; i++)
            {
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (positionOnGrid[i, j] == 0)
                        break;
                }
                if (j == 10)
                {
                    lineScore++;
                    combo++;
                    for (j = 0; j < 10; j++)
                    {
                        positionOnGrid[i, j] = 0;
                    }
                    int[,] newdropLocationGrid = new int[23, 10];
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            newdropLocationGrid[k + 1, l] = positionOnGrid[k, l];
                        }
                    }
                    for (int k = 1; k < i; k++)
                    {
                        for (int l = 0; l < 10; l++)
                        {
                            positionOnGrid[k, l] = 0;
                        }
                    }
                    for (int k = 0; k < 23; k++)
                        for (int l = 0; l < 10; l++)
                            if (newdropLocationGrid[k, l] == 1)
                                positionOnGrid[k, l] = 1;
                    Draw();
                }
            }
            if (combo == 1)
                points += 40 * level;
            else if (combo == 2)
                points += 100 * level;
            else if (combo == 3)
                points += 300 * level;
            else if (combo > 3)
                points += 300 * combo * level;

            if (lineScore < 5) level = 1;
            else if (lineScore < 10) level = 2;
            else if (lineScore < 15) level = 3;
            else if (lineScore < 25) level = 4;
            else if (lineScore < 35) level = 5;
            else if (lineScore < 50) level = 6;
            else if (lineScore < 70) level = 7;
            else if (lineScore < 90) level = 8;
            else if (lineScore < 110) level = 9;
            else if (lineScore < 150) level = 10;


            if (combo > 0)
            {
                Console.SetCursorPosition(25, 0);
                Console.WriteLine("Level: " + level);
                Console.SetCursorPosition(25, 1);
                Console.WriteLine("Points: " + points);
                Console.SetCursorPosition(25, 2);
                Console.WriteLine("Score: " + lineScore);
            }

            speed = 300 - 22 * level;
        }
        private static void Pulse() {
            
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey();
                keyPulse = true;
            }
            else
                keyPulse = false;

            if (Program.key.Key == ConsoleKey.LeftArrow & !tetrimino.IsSomethLeft() & keyPulse)
            {
                for (int i = 0; i < 4; i++)
                {
                    tetrimino.position[i][1] -= 1;
                }
                Update();

            }
            else if (Program.key.Key == ConsoleKey.RightArrow & !tetrimino.IsSomethRight() & keyPulse)
            {
                for (int i = 0; i < 4; i++)
                {
                    tetrimino.position[i][1] += 1;
                }
                Update();
            }
            if (Program.key.Key == ConsoleKey.DownArrow & keyPulse)
            {
                tetrimino.Solve();
            }
            if (Program.key.Key == ConsoleKey.DownArrow & keyPulse)
            {
                for (; tetrimino.IsSomthBelow() != true;)
                {
                    tetrimino.Solve();
                }
            }
            if (Program.key.Key == ConsoleKey.UpArrow & keyPulse)
            {
                tetrimino.Rotate();
                tetrimino.Update();
            }
        }
    
        public static void Draw() {
            for (int i = 0; i < 23; ++i)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.SetCursorPosition(1 + 2 * j, i);
                    if (area[i, j] == 1 | positionOnGrid[i, j] == 1)
                    {
                        Console.SetCursorPosition(1 + 2 * j, i);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(block);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }

            }
        }
        public static void GameArea()
        {            
            Console.ForegroundColor = ConsoleColor.Blue;
            for (int lengthCount = 0; lengthCount <= 22; lengthCount++)
            {
                Console.SetCursorPosition(0, lengthCount);
                Console.Write("†");
                Console.SetCursorPosition(21, lengthCount);
                Console.Write("†");
            }
            Console.SetCursorPosition(0, 23);
            for (int widthCount = 0; widthCount <= 10; widthCount++)
            {
                Console.Write("--");
            }
            Console.ForegroundColor = ConsoleColor.Green;
        }
        public static void Update() 
        {
            while (true)
            {
                time = (int)timerC.ElapsedMilliseconds;
                if (time > speed)
                {
                    time = 0;
                    timerC.Restart();
                    tetrimino.Solve();
                }
                if (letFall == true)
                {
                    tetrimino = nextTetrimino;
                    nextTetrimino = new Tetrimino();
                    tetrimino.Appeareance();

                    letFall = false;
                }
                int j;
                for (j = 0; j < 10; j++)
                {
                    if (positionOnGrid[0, j] == 1)
                        return;
                }

                Pulse();
                BlockMover();
            }
        }
        //public static int Rotate(int posX, int posY, int rot)
        //{
        //    switch (rot % 4)
        //    {
        //        case 0:
        //            return posY * 4 + posX;
        //        case 1:
        //            return 12 + posY - (posX * 4);
        //        case 2:
        //            return 15 - (posY * 4) - posX;
        //        case 3:
        //            return 3 - posY + (posX * 4);
        //        default:
        //            break;
        //    }
        //    return 0;
        //}
        //private static void Appeareance()
        //{            
        //    Tetrimino tetrimino = new Tetrimino();
        //    for (int i = 0; i < tetrimino.figure.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < tetrimino.figure.GetLength(1); j++)
        //        {
        //            if (tetrimino.figure[i, j] == 1)
        //            {
        //                tetrimino.position.Add(new int[] { i, (10 - tetrimino.figure.GetLength(1)) / 2 + j });
        //            }
        //        }
        //    }
        //    Update();
        //}
       
    }
}

