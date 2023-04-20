using System;
using System.IO;

public class FordFulkersonAlgorithm
{
    public int MaxFlow(int[][] cap, int s, int t)
    {
        int flow = 0;
        while (true)
        {
            int df = FindPath(cap, new bool[cap.Length], s, t, int.MaxValue);
            if (df == 0)
            {
                return flow;
            }
            flow += df;
        }
    }

    public int FindPath(int[][] cap, bool[] vis, int u, int t, int f)
    {
        if (u == t)
        {
            return f;
        }
        vis[u] = true;
        for (int v = 0; v < vis.Length; v++)
        {
            if (!vis[v] && cap[u][v] > 0)
            {
                int df = FindPath(cap, vis, v, t, Math.Min(f, cap[u][v]));
                if (df > 0)
                {
                    cap[u][v] -= df;
                    cap[v][u] += df;
                    return df;
                }
            }
        }
        return 0;
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

    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the file path:");
        string filepath = @"";
        int[][] matrix = FillAdjMatrixFromFile(filepath); // You'll need to implement this method separately

        int from = 0;
        int to = matrix[0].Length - 1;
        FordFulkersonAlgorithm fordFulkersonAlgorithm = new FordFulkersonAlgorithm();
        int maxFlow = fordFulkersonAlgorithm.MaxFlow(matrix, from, to);
        Console.WriteLine("Maximum flow from source " + from + " to sink " + to + " is " + maxFlow);
    }
}