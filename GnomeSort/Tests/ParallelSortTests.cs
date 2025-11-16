using System.Diagnostics;
using GnomeSort.Input;
using GnomeSort.Sorters;

namespace GnomeSort.Tests;

public class ParallelSortTests
{
    public static void RunSort()
    {
        var stopwatch = new Stopwatch();
        var inputCatcher = new InputCatcher();
        var parallelSorter = new ParallelHybridHybridGnomeMergeSorter<int>();
        
        var arrayLength = inputCatcher.CatchArrayLength();
        var numberOfThreads = inputCatcher.CatchNumberOfThreads();

        var random = new Random();
        const int minRandomValue = 0;
        const int maxRandomValue = 1000;
        var randomArray = ArrayUtils.GenerateRandomArray(arrayLength, () => 
            random.Next(minRandomValue, maxRandomValue));
        
        Console.WriteLine("Sorting array...\n");
        
        var parallelSortedArray = Array.Empty<int>();
        stopwatch.Start();
        
        try
        {
            parallelSortedArray = parallelSorter.Sort(randomArray, numberOfThreads);
            var sequentialArraySortingTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Parallel Gnome Sort took {sequentialArraySortingTime} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Parallel Gnome Sort failed:\n{ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
        }
        
        Console.WriteLine();
        Console.WriteLine("Parallel Gnome Sort result is sorted " 
                          + (ArrayUtils.IsSortedAscending(parallelSortedArray) 
                              ? "correctly." : "incorrectly."));
    }

    public static void RunTests()
    {
        int[] arraySizes = { 50003, 500003, 1500003, 4000003, 7000003, 11000003 };
        int[] threadCounts = { 6, 11, 16 };
        const int numTestsPerSize = 3;

        var stopwatch = new Stopwatch();
        var parallelSorter = new ParallelHybridHybridGnomeMergeSorter<int>();

        var random = new Random();
        const int minRandomValue = 0;
        const int maxRandomValue = 1000;

        foreach (var arraySize in arraySizes)
        {
            foreach (var threadCount in threadCounts)
            {
                Console.WriteLine($"Testing with array size: {arraySize} and {threadCount} threads");

                long totalTime = 0;
                for (int i = 0; i < numTestsPerSize; i++)
                {
                    var randomArray = ArrayUtils.GenerateRandomArray(arraySize, () =>
                        random.Next(minRandomValue, maxRandomValue));

                    stopwatch.Restart();
                    var parallelSortedArray = parallelSorter.Sort(randomArray, threadCount);
                    stopwatch.Stop();

                    totalTime += stopwatch.ElapsedMilliseconds;

                    // You might want to keep this check, but it can add overhead
                    // Console.WriteLine("Parallel Gnome Sort result is sorted " 
                    //                   + (ArrayUtils.IsSortedAscending(parallelSortedArray) 
                    //                       ? "correctly." : "incorrectly."));
                }

                double meanTime = (double)totalTime / numTestsPerSize;
                Console.WriteLine($"Mean Parallel Gnome Sort time: {meanTime:F2} ms");
                Console.WriteLine();
            }
        }
    }
}