using System;
using DungeonGeneration.Generator.Domain;

public class XTestUtils {

    public static bool areEquals(int[,] expected, int[,] result) {
        if (expected.GetLength(0) != result.GetLength(0)) return false;
        if (expected.GetLength(1) != result.GetLength(1)) return false;
        for (int i = 0; i < expected.GetLength(0); i++) {
            for (int j = 0; j < expected.GetLength(1); j++) {
                if (expected[i, j] != result[i, j]) return false;
            }
        }
        return true;
    }

    public static bool areEquals(Object[] expected, Object[] result) {
        if (expected.GetLength(0) != result.GetLength(0)) return false;
        for(int i=0; i<expected.GetLength(0); i++) {
            if (!expected[i].Equals(result[i])) return false;
        }
        return true;
    }

    public static void print(int[,] matrix) {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        String result = rows + " x " + cols + "\n";
        //Console.WriteLine(rows + " x " + cols);
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (j == 0) //Console.Write(" {");
                    result += " {";
                //Console.Write(matrix[i, j]);
                result += matrix[i, j];
                if (j != cols - 1) //Console.Write(", "); 
                    result += ", ";
            }
            //Console.Write("}");
            result += "}";
            if (i != rows - 1) //Console.WriteLine(",");
                result += ",\n";
        }
        Console.WriteLine(result);
    }

    public static void print(Object[] list) {
        int size = list.GetLength(0);

        String result = "[" + size + "] ";

        for (int i = 0; i < size; i++) {
            if (i == 0) result += " {";
            result += list[i].ToString();
            if (i != size - 1) result += ", ";
        }
        result += "}";
        Console.WriteLine(result);
    }
}