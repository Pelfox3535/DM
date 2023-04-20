using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kruskal
{
    public class Graph
    {
        public List<Vertex> Vertices { get; }
        public List<List<int>> AdjMatrix { get; }

        public Graph(List<Vertex> vertices, List<List<int>> adjMatrix)
        {
            Vertices = vertices;
            AdjMatrix = adjMatrix;
        }
    }

    public class Vertex
    {
        public int Number { get; }
        public List<Edge> Edges { get; }

        public Vertex(int number, List<Edge> edges)
        {
            Number = number;
            Edges = edges;
        }
    }

    public class Edge
    {
        public int Source { get; }
        public int Destination { get; }
        public int Weight { get; }

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
    }

    public static class KruskalExtensions
    {
        public static List<Edge> Kruskal(this Graph graph)
        {
            var edgesByWeight = graph.Vertices.SelectMany(v => v.Edges).OrderBy(e => e.Weight).ToList();
            var result = new List<Edge>();
            var parent = Enumerable.Range(0, graph.Vertices.Count).ToArray();
            foreach (var edge in edgesByWeight)
            {
                var firstVertex = GetVertex(edge.Source, parent);
                var secondVertex = GetVertex(edge.Destination, parent);
                if (firstVertex != secondVertex)
                {
                    result.Add(edge);
                    parent[firstVertex] = secondVertex;
                }
            }
            return result;
        }

        private static int GetVertex(int vertex, int[] parent)
        {
            int temp = vertex;
            while (temp != parent[temp])
            {
                parent[temp] = parent[parent[temp]];
                temp = parent[temp];
            }
            return temp;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var graph = ReadGraph(@"C:\Users\Dmytro\Downloads\Telegram Desktop\file1.txt");
            var result = graph.Kruskal();
            Console.WriteLine("Min spanning tree (Kruskal algorithm)");
            foreach (var edge in result)
            {
                Console.WriteLine($"[{edge.Source + 1}--({edge.Weight})-->{edge.Destination + 1}]");
            }
            Console.WriteLine($"Total weight: {result.Sum(e => e.Weight)}");
        }

        private static Graph ReadGraph(string filePath)
        {
            var file = new FileInfo(filePath);
            Console.WriteLine(file.FullName);
            var lines = File.ReadAllLines(filePath);
            var adjMatrix = lines.Select(line => line.Split(',', ' ').Select(s => s.Trim()).Where(s => int.TryParse(s, out _)).Select(int.Parse).ToList()).ToList();
            var vertices = adjMatrix.Select((row, source) => {
                var edges = new List<Edge>();
                for (int destination = 0; destination < row.Count; destination++)
                {
                    if (row[destination] > 0)
                    {
                        edges.Add(new Edge(source, destination, row[destination]));
                    }
                }
                return new Vertex(source, edges);
            }).ToList();
            return new Graph(vertices, adjMatrix);
        }
    }
}