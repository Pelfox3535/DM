public class Isomorphism
{
    public bool IsIsomorphic(int[][] graph1, int[][] graph2)
    {
        int n1 = graph1.Length;
        int n2 = graph2.Length;

        if (n1 != n2 || graph1[0].Length != graph2[0].Length)
        {
            return false;
        }

        int[] degree1 = new int[n1];
        int[] degree2 = new int[n2];
        for (int i = 0; i < n1; i++)
        {
            for (int j = 0; j < graph1[0].Length; j++)
            {
                if (graph1[i][j] > 0)
                {
                    degree1[i]++;
                }

                if (graph2[i][j] > 0)
                {
                    degree2[i]++;
                }
            }
        }

        return EqualArrays(degree1, degree2);
    }

    // return how often n appears in an array
    public static int Count(int n, int[] array)
    {
        int appearedNTimes = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == n)
            {
                appearedNTimes++;
            }
        }
        return appearedNTimes;
    }

    public static bool EqualArrays(int[] arr1, int[] arr2)
    {
        foreach (int i in arr1)
        {
            if (Count(i, arr1) != Count(i, arr2)) return false;
        }
        return arr1.Length == arr2.Length;
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
        string filepath1 = @"";
        string filepath2 = @"";

        int[][] matrix1 = FillAdjMatrixFromFile(filepath1);
        int[][] matrix2 = FillAdjMatrixFromFile(filepath2);

        Isomorphism isomorphism = new Isomorphism();
        bool isTrue = isomorphism.IsIsomorphic(matrix1, matrix2);
        if (isTrue)
        {
            Console.WriteLine("These graphs are isomorphic");
        }
        else
        {
            Console.WriteLine("These graphs aren't isomorphic");
        }
    }
}