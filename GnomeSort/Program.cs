using GnomeSort.Tests;
using GnomeSort.Tests.Common;

namespace GnomeSort;

public static class Program
{
    public static void Main()
    {
        // CommonSortTests.RunSort();
        // SequentialSortTests.RunSort();
        ParallelSortTests.RunSort();
        
        // SequentialSortTests.RunSortTests();
        // ParallelSortTests.RunTests();
        // CommonSortTests.TestObjectsSorting();
    }
    
    private static void PrintArrays(int[] initialArray, int[] sequentiallySortedArray, int[] parallelSortedArray)
    {
        Console.WriteLine("Initial array:");
        ArrayUtils.PrintArray(initialArray);
        
        Console.WriteLine("Sequentially sorted array:");
        ArrayUtils.PrintArray(sequentiallySortedArray);
        Console.WriteLine("Parallel sorted array:");
        ArrayUtils.PrintArray(parallelSortedArray);
    }
}