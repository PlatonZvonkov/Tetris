using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Tetris
{
    class Tetrimino
    {

        public static int[,] I = new int[1, 4] { { 1, 1, 1, 1 } };
        public static int[,] O = new int[2, 2] { { 1, 1 }, { 1, 1 } };
        public static int[,] T = new int[2, 3] { { 0, 1, 0 }, { 1, 1, 1 } };
        public static int[,] S = new int[2, 3] { { 0, 1, 1 }, { 1, 1, 0 } };
        public static int[,] Z = new int[2, 3] { { 1, 1, 0 }, { 0, 1, 1 } };
        public static int[,] J = new int[2, 3] { { 1, 0, 0 }, { 1, 1, 1 } };
        public static int[,] L = new int[2, 3] { { 0, 0, 1 }, { 1, 1, 1 } };
        public static List<int[,]> tetrimino = new List<int[,]>() { I, O, T, S, Z, J, L };

        private bool isRight = false;
        public int[,] figure;
        public List<int[]> position = new List<int[]>();

        public Tetrimino()
        {

            Random random = new Random();
            figure = tetrimino[random.Next(0, 7)];
            for (int i = 23; i < 33; ++i)
            {
                for (int j = 3; j < 10; j++)
                {
                    Console.SetCursorPosition(i, j);
                    Console.Write("  ");
                }

            }

            Program.GameArea();

            for (int i = 0; i < figure.GetLength(0); i++)
            {
                for (int j = 0; j < figure.GetLength(1); j++)
                {
                    if (figure[i, j] == 1)
                    {
                        Console.SetCursorPosition(((10 - figure.GetLength(1)) / 2 + j) * 2 + 20, i + 5);
                        Console.Write(Program.block);
                    }
                }
            }
        }
        public void Appeareance()
        {
            for (int i = 0; i < figure.GetLength(0); i++)
            {
                for (int j = 0; j < figure.GetLength(1); j++)
                {
                    if (figure[i, j] == 1)
                    {
                        position.Add(new int[] { i, (10 - figure.GetLength(1)) / 2 + j });
                    }
                }
            }
            Update();
        }

        public void Solve()
        {
            if (IsSomthBelow())
            {
                for (int i = 0; i < 4; i++)
                {
                    Program.positionOnGrid[position[i][0], position[i][1]] = 1;
                }
                Program.letFall = true;

            }
            else
            {
                for (int numCount = 0; numCount < 4; numCount++)
                {
                    position[numCount][0] += 1;
                }
                Update();
            }
        }
        public void Rotate()
        {
            List<int[]> posicionTemporal = new List<int[]>();
            for (int i = 0; i < figure.GetLength(0); i++)
            {
                for (int j = 0; j < figure.GetLength(1); j++)
                {
                    if (figure[i, j] == 1)
                        posicionTemporal.Add(new int[] { i, (10 - figure.GetLength(1)) / 2 + j });

                }
            }

            if (figure == tetrimino[0])
            {
                if (isRight == false)
                {
                    for (int i = 0; i < position.Count; i++)
                    {
                        posicionTemporal[i] = TransformMatrix(position[i], position[2], "Clockwise");
                    }
                }
                else
                {
                    for (int i = 0; i < position.Count; i++)
                    {
                        posicionTemporal[i] = TransformMatrix(position[i], position[2], "Counterclockwise");
                    }
                }
            }

            else if (figure == tetrimino[3])
            {
                for (int i = 0; i < position.Count; i++)
                {
                    posicionTemporal[i] = TransformMatrix(position[i], position[3], "Clockwise");
                }
            }

            else if (figure == tetrimino[1]) return;
            else
            {
                for (int i = 0; i < position.Count; i++)
                {
                    posicionTemporal[i] = TransformMatrix(position[i], position[2], "Clockwise");
                }
            }


            for (int count = 0; OverlapLeft(posicionTemporal) != false | OverlapRight(posicionTemporal) != false | Overlap(posicionTemporal) != false; count++)
            {
                if (OverlapLeft(posicionTemporal) == true)
                {
                    for (int i = 0; i < position.Count; i++)
                    {
                        posicionTemporal[i][1] += 1;
                    }
                }

                if (OverlapRight(posicionTemporal) == true)
                {
                    for (int i = 0; i < position.Count; i++)
                    {
                        posicionTemporal[i][1] -= 1;
                    }
                }
                if (Overlap(posicionTemporal) == true)
                {
                    for (int i = 0; i < position.Count; i++)
                    {
                        posicionTemporal[i][0] -= 1;
                    }
                }
                if (count == 3)
                {
                    return;
                }
            }

            position = posicionTemporal;

        }
        public bool IsSomthBelow()
        {
            for (int i = 0; i < 4; i++)
            {
                if (position[i][0] + 1 >= 23)
                    return true;
                if (position[i][0] + 1 < 23)
                {
                    if (Program.positionOnGrid[position[i][0] + 1, position[i][1]] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool IsSomethRight()
        {
            {
                for (int i = 0; i < 4; i++)
                {
                    if (position[i][1] == 9)
                    {
                        return true;
                    }
                    else if (Program.positionOnGrid[position[i][0], position[i][1] + 1] == 1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool IsSomethLeft()
        {
            for (int i = 0; i < 4; i++)
            {
                if (position[i][1] == 0)
                {
                    return true;
                }
                else if (Program.positionOnGrid[position[i][0], position[i][1] - 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public int[] TransformMatrix(int[] coord, int[] axis, string dir)
        {
            int[] pcoord = { coord[0] - axis[0], coord[1] - axis[1] };
            if (dir == "Counterclockwise")
            {
                pcoord = new int[] { -pcoord[1], pcoord[0] };
            }
            else if (dir == "Clockwise")
            {
                pcoord = new int[] { pcoord[1], -pcoord[0] };
            }

            return new int[] { pcoord[0] + axis[0], pcoord[1] + axis[1] };
        }

        public bool? OverlapLeft(List<int[]> location)
        {
            List<int> coordinates = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                coordinates.Add(location[i][1]);
                if (location[i][1] < 0)
                {
                    return true;
                }
                if (location[i][1] > 9)
                {
                    return false;
                }
                if (location[i][0] >= 23)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (coordinates.Max() - coordinates.Min() == 3)
                {
                    if (coordinates.Min() == location[i][1] | coordinates.Min() + 1 == location[i][1])
                    {
                        if (Program.positionOnGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (coordinates.Min() == location[i][1])
                    {
                        if (Program.positionOnGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool? OverlapRight(List<int[]> location)
        {
            List<int> coordinates = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                coordinates.Add(location[i][1]);
                if (location[i][1] > 9)
                {
                    return true;
                }
                if (location[i][1] < 0)
                {
                    return false;
                }
                if (location[i][0] >= 23)
                    return null;
                if (location[i][0] < 0)
                    return null;
            }
            for (int i = 0; i < 4; i++)
            {
                if (coordinates.Max() - coordinates.Min() == 3)
                {
                    if (coordinates.Max() == location[i][1] | coordinates.Max() - 1 == location[i][1])
                    {
                        if (Program.positionOnGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (coordinates.Max() == location[i][1])
                    {
                        if (Program.positionOnGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool? Overlap(List<int[]> location)
        {
            List<int> coordinates = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                coordinates.Add(location[i][0]);
                if (location[i][0] >= 23)
                    return true;
                if (location[i][0] < 0)
                    return null;
                if (location[i][1] < 0)
                    return null;
                if (location[i][1] > 9)
                    return null;

            }
            for (int i = 0; i < 4; i++)
            {
                if (coordinates.Max() - coordinates.Min() == 3)
                {
                    if (coordinates.Max() == location[i][0] | coordinates.Max() - 1 == location[i][0])
                    {
                        if (Program.positionOnGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    if (coordinates.Max() == location[i][0])
                    {
                        if (Program.positionOnGrid[location[i][0], location[i][1]] == 1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Update()
        {
            for (int i = 0; i < 23; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Program.area[i, j] = 0;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Program.area[position[i][0], position[i][1]] = 1;
            }
            Program.Draw();

        }
    }
}
