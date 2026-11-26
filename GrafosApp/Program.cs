// Program.cs - Tema 7: Grafos
using System;

class Grafo
{
    public int NumVertices;
    public string[] Vertices;
    public double[,] MatrizAdyacencia;

    private int contador = 0;

    public Grafo(int numVertices)
    {
        if (numVertices <= 0)
            throw new ArgumentException("El número de vértices debe ser mayor que cero.");

        NumVertices = numVertices;
        Vertices = new string[numVertices];
        MatrizAdyacencia = new double[numVertices, numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            for (int j = 0; j < numVertices; j++)
            {
                MatrizAdyacencia[i, j] =
                    (i == j) ? 0 : double.PositiveInfinity;
            }
        }
    }

    // Agregar vértices
    public void AgregarVertice(string nombre)
    {
        if (contador >= NumVertices)
            throw new InvalidOperationException("No hay espacio para más vértices.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del vértice no es válido.");

        Vertices[contador] = nombre;
        contador++;
    }

    // Agregar aristas
    public void AgregarArista(int origen, int destino, double peso, bool dirigido = false)
    {
        if (origen < 0 || origen >= contador ||
            destino < 0 || destino >= contador)
            throw new ArgumentOutOfRangeException("Índice de vértice inválido.");

        if (double.IsNaN(peso) || double.IsInfinity(peso))
            throw new ArgumentException("El peso debe ser un número finito.");

        MatrizAdyacencia[origen, destino] = peso;

        if (!dirigido)
            MatrizAdyacencia[destino, origen] = peso;
    }

    // Obtener vértices
    public string[] ObtenerVertices()
    {
        return (string[])Vertices.Clone();
    }

    // Mostrar matriz
    public void MostrarMatriz(double[,] matriz)
    {
        Console.Write("      ");

        foreach (string v in Vertices)
            Console.Write($"{v,6}");

        Console.WriteLine();

        for (int i = 0; i < NumVertices; i++)
        {
            Console.Write($"{Vertices[i],6}");

            for (int j = 0; j < NumVertices; j++)
            {
                string valor = double.IsPositiveInfinity(matriz[i, j])
                    ? "INF"
                    : matriz[i, j].ToString("0.##");

                Console.Write($"{valor,6}");
            }

            Console.WriteLine();
        }
    }

    // Algoritmo de Floyd-Warshall
    public double[,] FloydWarshall(double[,] matrizOriginal)
    {
        if (matrizOriginal.GetLength(0) != NumVertices ||
            matrizOriginal.GetLength(1) != NumVertices)
            throw new ArgumentException("Las dimensiones de la matriz no son válidas.");

        double[,] dist = (double[,])matrizOriginal.Clone();

        for (int k = 0; k < NumVertices; k++)
        {
            for (int i = 0; i < NumVertices; i++)
            {
                for (int j = 0; j < NumVertices; j++)
                {
                    if (double.IsPositiveInfinity(dist[i, k]) ||
                        double.IsPositiveInfinity(dist[k, j]))
                        continue;

                    double nuevaDistancia = dist[i, k] + dist[k, j];

                    if (nuevaDistancia < dist[i, j])
                    {
                        dist[i, j] = nuevaDistancia;
                    }
                }
            }
        }

        return dist;
    }

    // Detectar ciclos negativos
    public bool TieneCicloNegativo(double[,] matriz)
    {
        for (int i = 0; i < NumVertices; i++)
        {
            if (matriz[i, i] < 0)
                return true;
        }

        return false;
    }
}

class Program
{
    static void Main()
    {
        Grafo grafo = new Grafo(4);

        // Agregar vértices
        grafo.AgregarVertice("A");
        grafo.AgregarVertice("B");
        grafo.AgregarVertice("C");
        grafo.AgregarVertice("D");

        // Agregar aristas
        grafo.AgregarArista(0, 1, 5);
        grafo.AgregarArista(0, 2, 10);
        grafo.AgregarArista(1, 2, 3);
        grafo.AgregarArista(1, 3, 8);
        grafo.AgregarArista(2, 3, 2);

        Console.WriteLine("MATRIZ DE ADYACENCIA:");
        grafo.MostrarMatriz(grafo.MatrizAdyacencia);

        // Aplicar Floyd-Warshall
        double[,] resultado =
            grafo.FloydWarshall(grafo.MatrizAdyacencia);

        Console.WriteLine("\nMATRIZ DE DISTANCIAS MÍNIMAS:");
        grafo.MostrarMatriz(resultado);

        // Verificar ciclos negativos
        if (grafo.TieneCicloNegativo(resultado))
            Console.WriteLine("\nEl grafo contiene un ciclo negativo.");
        else
            Console.WriteLine("\nEl grafo no contiene ciclos negativos.");
    }
}