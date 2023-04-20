using System;
using System.IO;
using System.Linq;

namespace BranchAndBoundTSP
{
    class Program
    {
        static int N = 0;
        static int[] final_path;
        static bool[] visited;
        static int final_res = int.MaxValue;

        static void CopyToFinal(int[] curr_path)
        {
            for (int i = 0; i < N; i++)
                final_path[i] = curr_path[i];
            final_path[N] = curr_path[0];
        }

        static int FirstMin(int[][] adj, int i)
        {
            int min = int.MaxValue;
            for (int k = 0; k < N; k++)
                if (adj[i][k] < min && i != k)
                    min = adj[i][k];
            return min;
        }

        static int SecondMin(int[][] adj, int i)
        {
            int first = int.MaxValue, second = int.MaxValue;
            for (int j = 0; j < N; j++)
            {
                if (i == j)
                    continue;

                if (adj[i][j] <= first)
                {
                    second = first;
                    first = adj[i][j];
                }
                else if (adj[i][j] <= second &&
                        adj[i][j] != first)
                    second = adj[i][j];
            }
            return second;
        }

        static void TSPRec(int[][] adj, int curr_bound, int curr_weight,
                           int level, int[] curr_path)
        {
            if (level == N)
            {
                if (adj[curr_path[level - 1]][curr_path[0]] != 0)
                {
                    int curr_res = curr_weight +
                            adj[curr_path[level - 1]][curr_path[0]];

                    if (curr_res < final_res)
                    {
                        CopyToFinal(curr_path);
                        final_res = curr_res;
                    }
                }
                return;
            }

            for (int i = 0; i < N; i++)
            {
                if (adj[curr_path[level - 1]][i] != 0 && !visited[i])
                {
                    int temp = curr_bound;
                    curr_weight += adj[curr_path[level - 1]][i];

                    if (level == 1)
                        curr_bound -= ((FirstMin(adj, curr_path[level - 1]) +
                                FirstMin(adj, i)) / 2);
                    else
                        curr_bound -= ((SecondMin(adj, curr_path[level - 1]) +
                                FirstMin(adj, i)) / 2);

                    if (curr_bound + curr_weight < final_res)
                    {
                        curr_path[level] = i;
                        visited[i] = true;

                        TSPRec(adj, curr_bound, curr_weight, level + 1,
                                curr_path);
                    }

                    curr_weight -= adj[curr_path[level - 1]][i];
                    curr_bound = temp;

                    Array.Fill(visited, false);
                    for (int j = 0; j <= level - 1; j++)
                        visited[curr_path[j]] = true;
                }
            }
        }

        static void TSP(int[][] adj, int verticesCount)
        {
            N = verticesCount;

            final_path = new int[N + 1];
            visited = new bool[N];

            int[] curr_path = new int[N + 1];

            int curr_bound = 0;
            Array.Fill(curr_path, -1);
            Array.Fill(visited, false);

            for (int i = 0; i < N; i++)
                curr_bound += (FirstMin(adj, i) +
                         SecondMin(adj, i));

            curr_bound = (curr_bound % 2 == 1) ? curr_bound / 2 + 1 :
                    curr_bound / 2;

            visited[0] = true;
            curr_path[0] = 0;

            TSPRec(adj, curr_bound, 0, 1, curr_path);
        }

        public static int[][] FillAdjMatrixFromFile(string filepath)
        {
            int[][] matrix;

            string[] lines = File.ReadAllLines(filepath);

            int verticesCount = int.Parse(lines[0]);
            matrix = new int[verticesCount][];

            for (int i = 0; i < verticesCount; i++)
            {
                matrix[i] = lines[i + 1].Split(' ').Select(int.Parse).ToArray();
            }

            return matrix;
        }

        static void Main(string[] args)
        {
            string filepath = @"";
            int[][] matrix = FillAdjMatrixFromFile(filepath);
            TSP(matrix, matrix.Length);

            Console.WriteLine($"Minimum path: {final_res}");
            Console.Write("Path: ");
            for (int i = 0; i <= N; i++)
            {
                Console.Write($"{final_path[i]} ");
            }
        }
    }
}