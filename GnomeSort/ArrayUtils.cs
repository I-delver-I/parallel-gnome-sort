namespace GnomeSort;

public static class ArrayUtils
{
    public static void PrintArray<T>(T[] array)
    {
        foreach (var element in array)
        {
            Console.Write($"{element} ");
        }
        
        Console.WriteLine();
    }
        
    public static bool AreArraysEqual<T>(T[] array1, T[] array2)
    {
        if (array1.Length != array2.Length)
        {
            return false;
        }

        return !array1.Where((t, i) => !t.Equals(array2[i])).Any();
    }
        
    public static bool IsSortedAscending<T>(T[] array) where T : IComparable<T>
    {
        for (var i = 1; i < array.Length; i++)
        {
            if (array[i - 1].CompareTo(array[i]) > 0)
            {
                return false;
            }
        }
            
        return true;
    }
    
    public static T[] GenerateRandomArray<T>(int length, Func<T> valueGenerator)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Size must be greater than zero.");
        }
            
        var randomArray = new T[length];

        for (var i = 0; i < length; i++) 
        {
            randomArray[i] = valueGenerator();
        }

        return randomArray;
    }
}