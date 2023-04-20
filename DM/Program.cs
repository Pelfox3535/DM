using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DM
{
    public class EulerPath
    {
        private readonly int[][] adjacencyMatrix;
        private readonly int numberOfNodes;
        private readonly Dictionary<string, int> edgeWeights;
        private int tourCost = 0;

        public EulerPath(int numberOfNodes, int[][] adjacencyMatrix, Dictionary<string, int> edgeWeights)
        {
            this.numberOfNodes = numberOfNodes;
            this.adjacencyMatrix = new int[numberOfNodes + 1][];
            for (int i = 0; i < this.adjacencyMatrix.Length; i++)
            {
                this.adjacencyMatrix[i] = new int[numberOfNodes + 1];
            }

            for (int sourceVertex = 1; sourceVertex <= numberOfNodes; sourceVertex++)
            {
                for (int destinationVertex = 1; destinationVertex <= numberOfNodes; destinationVertex++)
                {
                    this.adjacencyMatrix[sourceVertex][destinationVertex] = adjacencyMatrix[sourceVertex][destinationVertex];
                }
            }
            this.edgeWeights = edgeWeights;
        }

        public static void Main(string[] args)
        {
            Dictionary<string, int> weights = new Dictionary<string, int>();

            try
            {
                string filepath = @"";
                string[] lines = File.ReadAllLines(filepath);
                List<string> strings = lines.Where(str => str.Length > 2).ToList();

                int[][] adjacencyMatrix = new int[strings.Count + 1][];

                for (int i = 0; i < adjacencyMatrix.Length; i++)
                {
                    adjacencyMatrix[i] = new int[strings.Count + 1];
                }

                for (int i = 0; i < strings.Count; i++)
                {
                    string[] valuesForNode = strings[i].Split(" ");
                    for (int j = 0; j < valuesForNode.Length; j++)
                    {
                        int indexI = i + 1;
                        int indexJ = j + 1;
                        int value;
                        if (int.TryParse(valuesForNode[j], out value) && value > 0)
                        {
                            weights[$"{indexI}{indexJ}"] = value;
                            adjacencyMatrix[indexI][indexJ] = 1;
                        }
                        else
                        {
                            adjacencyMatrix[indexI][indexJ] = 0;
                        }
                    }
                }

                for (int i = 1; i < strings.Count + 1; i++)
                {
                    int nodeDegree = 0;
                    for (int j = 0; j < adjacencyMatrix[i].Length; j++)
                    {
                        int num = adjacencyMatrix[i][j];
                        if (num > 0)
                        {
                            nodeDegree++;
                        }
                    }
                    if (nodeDegree % 2 != 0)
                    {
                        Console.WriteLine($"Степiнь вершини {i} не парна, отже ейлерового циклу не iснує");
                        Environment.Exit(1);
                    }
                }

                EulerPath circuit = new EulerPath(strings.Count, adjacencyMatrix, weights);
                circuit.PrintEulerTour();
                // Rest of the code
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        public int Degree(int vertex)
        {
            int degree = 0;
            for (int destinationVertex = 1; destinationVertex <= numberOfNodes; destinationVertex++)
            {
                if (adjacencyMatrix[vertex][destinationVertex] == 1 || adjacencyMatrix[destinationVertex][vertex] == 1)
                {
                    degree++;
                }
            }
            return degree;
        }

        public int OddDegreeVertex()
        {
            int vertex = -1;
            for (int node = 1; node <= numberOfNodes; node++)
            {
                if (Degree(node) % 2 != 0)
                {
                    vertex = node;
                    break;
                }
            }
            return vertex;
        }

        public void PrintEulerTourUtil(int vertex)
        {
            for (int destination = 1; destination <= numberOfNodes; destination++)
            {
                if (adjacencyMatrix[vertex][destination] == 1 && IsValidNextEdge(vertex, destination))
                {
                    string edgeWeightKey = vertex.ToString() + destination.ToString();
                    int weight = edgeWeights[edgeWeightKey];
                    tourCost += weight;
                    Console.WriteLine($"source : {vertex} destination {destination} edgeWeight: {weight}");
                    RemoveEdge(vertex, destination);
                    PrintEulerTourUtil(destination);
                }
            }
        }

        public void PrintEulerTour()
        {
            int vertex = 1;
            if (OddDegreeVertex() != -1)
            {
                vertex = OddDegreeVertex();
            }
            PrintEulerTourUtil(vertex);
            Console.WriteLine($"Euler path costs: {tourCost}");
        }

        public bool IsValidNextEdge(int source, int destination)
        {
            int count = 0;
            for (int vertex = 1; vertex <= numberOfNodes; vertex++)
            {
                if (adjacencyMatrix[source][vertex] == 1)
                {
                    count++;
                }
            }

            if (count == 1)
            {
                return true;
            }

            int[] visited = new int[numberOfNodes + 1];
            int count1 = DFSCount(source, visited);

            RemoveEdge(source, destination);
            for (int vertex = 1; vertex <= numberOfNodes; vertex++)
            {
                visited[vertex] = 0;
            }

            int count2 = DFSCount(source, visited);
            AddEdge(source, destination);

            return count1 <= count2;
        }

        public int DFSCount(int source, int[] visited)
        {
            visited[source] = 1;
            int count = 1;
            int destination = 1;

            while (destination <= numberOfNodes)
            {
                if (adjacencyMatrix[source][destination] == 1 && visited[destination] == 0)
                {
                    count += DFSCount(destination, visited);
                }
                destination++;
            }
            return count;
        }

        public void RemoveEdge(int source, int destination)
        {
            adjacencyMatrix[source][destination] = 0;
            adjacencyMatrix[destination][source] = 0;
        }

        public void AddEdge(int source, int destination)
        {
            adjacencyMatrix[source][destination] = 1;
            adjacencyMatrix[destination][source] = 1;
        }

    }
}
