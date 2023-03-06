using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab02
{

    public class PatternMatching : MarshalByRefObject
    {
        /// <summary>
        /// Etap 1 - wyznaczenie trasy, zgodnie z którą robot przemieści się z pozycji poczatkowej (0,0) na pozycję docelową (-n-1, m-1)
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="obstacles">tablica ze współrzędnymi przeszkód</param>
        /// <returns>krotka (bool result, string path) - result ma wartość true jeżeli trasa istnieje, false wpp., path to wynikowa trasa</returns>
        public (bool result, string path) Lab02Stage1(int n, int m, (int, int)[] obstacles)
        {
            char[,] map = new char[n, m];
            //string path = new string('X',m+n - 2);
            char[] pathC = new char[m + n - 2];


            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    map[i, j] = 'o';
                }
            }

            for (int i = 0; i < obstacles.Length; i++)
            {
                map[obstacles[i].Item1, obstacles[i].Item2] = 'b';
            }

            for (int i = 1; i < n; i++)
            {
                if (map[i - 1, 0] != 'b' && map[i, 0] != 'b')
                {
                    map[i, 0] = 'D';
                }
                else
                {
                    map[i, 0] = 'b';
                }
            }
            for (int i = 1; i < m; i++)
            {
                if (map[0, i - 1] != 'b' && map[0, i] != 'b')
                {
                    map[0, i] = 'R';
                }
                else
                {
                    map[0, i] = 'b';
                }
            }



            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    if (map[i, j] != 'b')
                    {
                        if (map[i - 1, j] != 'b')
                        {
                            map[i, j] = 'D';
                        }
                        else if (map[i, j - 1] != 'b')
                        {
                            map[i, j] = 'R';
                        }
                        else
                        {
                            map[i, j] = 'b';
                        }
                    }
                }
            }
           // path[9] = 'c';




            bool result = map[n - 1, m - 1] != 'b';
            if (!result) return (false, "");

            int c_i = n - 1;
            int c_j = m - 1;
            int ind = m + n - 3;
            while (c_i != 0 || c_j != 0)
            {
                pathC[ind--] = map[c_i, c_j];
                if (map[c_i, c_j] == 'D') c_i--;
                else c_j--;

            }

            string path = new string(pathC) ;

            return (result, path);
        }

        /// <summary>
        /// Etap 2 - wyznaczenie trasy realizującej zadany wzorzec, zgodnie z którą robot przemieści się z pozycji poczatkowej (0,0) na pozycję docelową (-n-1, m-1)
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="pattern">zadany wzorzec</param>
        /// <param name="obstacles">tablica ze współrzędnymi przeszkód</param>
        /// <returns>krotka (bool result, string path) - result ma wartość true jeżeli trasa istnieje, false wpp., path to wynikowa trasa</returns>
        public (bool result, string path) Lab02Stage2(int n, int m, string pattern, (int, int)[] obstacles)
        {
            int p = pattern.Length;
            char[,,] map = new char[p+1, n, m];
            char[] pathC = new char[m + n - 2];

            for (int k = 0; k <= p; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        map[k, i, j] = 'o';
                    }
                }
            }

            for (int k = 0; k <= p; k++)
            {
                for (int i = 0; i < obstacles.Length; i++)
                {
                    map[k, obstacles[i].Item1, obstacles[i].Item2] = 'b';
                }
            }


            // k == 0
            map[0, 0, 0] = 'D';

            // k > 0
            for (int k = 1; k <= p; k++)
            {
                if (pattern[k - 1] == '*')
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < m; j++)
                        {
                            if (map[k - 1, i, j] != 'b' && map[k - 1, i, j] != 'o') map[k, i, j] = map[k - 1, i, j];
                            else if (map[k - 1, i, j] == 'o')
                            {
                                if (i > 0)
                                {
                                    if (map[k, i - 1, j] == 'D' || map[k, i - 1, j] == 'R') map[k, i, j] = 'D';
                                }

                                if (j > 0)
                                {
                                    if (map[k, i, j - 1] == 'D' || map[k, i, j - 1] == 'R') map[k, i, j] = 'R';
                                }

                            }
                        }
                    }
                    continue;
                }
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (map[k - 1, i, j] != 'o' && map[k - 1, i, j] != 'b')
                        {
                            if (pattern[k - 1] == 'D')
                            {
                                if (i + 1 < n && map[k, i + 1, j] != 'b')
                                {
                                    map[k, i + 1, j] = 'D';
                                }

                            }
                            else if (pattern[k - 1] == 'R')
                            {
                                if (j + 1 < m && map[k, i, j + 1] != 'b')
                                {
                                    map[k, i, j + 1] = 'R';
                                }

                            }
                            else if (pattern[k - 1] == '?')
                            {
                                if (j + 1 < m && map[k, i, j + 1] != 'b')
                                {
                                    map[k, i, j + 1] = 'R';
                                }
                                if (i + 1 < n && map[k, i + 1, j] != 'b')
                                {
                                    map[k, i + 1, j] = 'D';
                                }

                            }
                        }
                    }
                }
            }


            bool result = map[p, n - 1, m - 1] != 'b' && map[p, n - 1, m - 1] != 'o';
            if (!result) return (false, "");

            
            int c_i = n - 1;
            int c_j = m - 1;
            int it = 0;
            int ind = m + n - 3;
            while (c_i != 0 || c_j != 0)
            {
                if (map[p - it, c_i, c_j] == 'o') it++;
                else if (pattern[p - 1 - it] != '*')
                {
                    pathC[ind--] = map[p - it, c_i, c_j];
                    if (map[p - it, c_i, c_j] == 'D') c_i--;
                    else c_j--;
                    it++;
                }
                else
                {
                    while ((map[p - it, c_i, c_j] == 'D' || map[p - it, c_i, c_j] == 'R') && (c_j != 0 || c_i !=0))
                    {
                        pathC[ind--] += map[p - it, c_i, c_j];
                        if (map[p - it, c_i, c_j] == 'D') c_i--;
                        else c_j--;
                    }
                }

            }

            string path = new string(pathC);
            return (result, path);
           
        }
    }
}