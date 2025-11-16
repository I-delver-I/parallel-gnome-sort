using System.Diagnostics;
using GnomeSort.Input;
using GnomeSort.Sorters;

namespace GnomeSort.Tests.Common;

public class CommonSortTests
{
    public static void RunSort()
    {
        var stopwatch = new Stopwatch();
        var inputCatcher = new InputCatcher();
        var sequentialSorter = new SequentialHybridHybridGnomeMergeSorter<int>();
        var parallelSorter = new ParallelHybridHybridGnomeMergeSorter<int>();
        
        var arrayLength = inputCatcher.CatchArrayLength();
        var numberOfThreads = inputCatcher.CatchNumberOfThreads();
        
        long sequentialArraySortingTime = 0;
        long parallelArraySortingTime = 0;
        
        var random = new Random();
        const int minRandomValue = 0;
        const int maxRandomValue = 30;
        var randomArray = ArrayUtils.GenerateRandomArray(arrayLength, () => 
            random.Next(minRandomValue, maxRandomValue));
        
        Console.WriteLine("Sorting array...\n");
        
        var sequentiallySortedArray = Array.Empty<int>();
        stopwatch.Start();

        try
        {
            sequentiallySortedArray = sequentialSorter.Sort(randomArray);
            sequentialArraySortingTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Sequential Gnome Sort took {sequentialArraySortingTime} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sequential Gnome Sort failed:\n{ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
        }

        stopwatch.Reset();
        var parallelSortedArray = Array.Empty<int>();
        stopwatch.Start();

        try
        {
            parallelSortedArray = parallelSorter.Sort(randomArray, numberOfThreads);
            parallelArraySortingTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Parallel Gnome Sort took {parallelArraySortingTime} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Parallel Gnome Sort failed:\n{ex.Message}");
        }
        finally
        {
            stopwatch.Stop();
        }

        var speedUpFactor = (double)sequentialArraySortingTime / parallelArraySortingTime;
        Console.WriteLine($"Speed-up factor: {speedUpFactor}");
        Console.WriteLine();
        
        Console.WriteLine("Sequential Gnome Sort result is sorted " 
                          + (ArrayUtils.IsSortedAscending(sequentiallySortedArray) 
                              ? "correctly." : "incorrectly."));
        Console.WriteLine("Parallel Gnome Sort result is sorted "
                          + (ArrayUtils.IsSortedAscending(parallelSortedArray) 
                              ? "correctly." : "incorrectly."));

        Console.WriteLine();
        Console.WriteLine(ArrayUtils.AreArraysEqual(sequentiallySortedArray, parallelSortedArray) 
            ? "Sorted arrays are equal" : "Sorted arrays are not equal");

        // Console.WriteLine("Initial array:");
        // ArrayUtils.PrintArray(randomArray);
        //
        // Console.WriteLine("Sequentially sorted array:");
        // ArrayUtils.PrintArray(sequentiallySortedArray);
        //
        // Console.WriteLine("Parallel sorted array:");
        // ArrayUtils.PrintArray(parallelSortedArray);
    }
    
    public static void TestObjectsSorting()
    {
        // Create a list of TestObject instances
        var testObjects = new List<TestObject> 
        { 
            new(5), new(3), new(8), new(1), new(9), new(2), new(4), new(7), new(6)
        };

        Console.WriteLine("Initial Array:");
        foreach (var obj in testObjects)
        {
            Console.Write(obj + " ");
        }
        Console.WriteLine();
        
        var sequentialSorter = new SequentialHybridHybridGnomeMergeSorter<TestObject>();
        var sequentialSorted = sequentialSorter.Sort(testObjects.ToArray());

        Console.WriteLine("Sequential Sorted Array:");
        foreach (var obj in sequentialSorted)
        {
            Console.Write(obj + " ");
        }
        Console.WriteLine();

        var parallelSorter = new ParallelHybridHybridGnomeMergeSorter<TestObject>();
        var parallelSorted = parallelSorter.Sort(testObjects.ToArray(), Environment.ProcessorCount);

        Console.WriteLine("Parallel Sorted Array:");
        foreach (var obj in parallelSorted)
        {
            Console.Write(obj + " ");
        }
        Console.WriteLine();
        
        Console.WriteLine("Sequential Gnome Sort result is sorted " 
                          + (ArrayUtils.IsSortedAscending(sequentialSorted) 
                              ? "correctly." : "incorrectly."));
        
        Console.WriteLine("Parallel Gnome Sort result is sorted " 
                          + (ArrayUtils.IsSortedAscending(parallelSorted) 
                              ? "correctly." : "incorrectly."));

        if (sequentialSorted.SequenceEqual(parallelSorted))
        {
            Console.WriteLine("Both arrays are sorted equally!");
        }
        else
        {
            Console.WriteLine("Arrays are not sorted equally!");
        }
    }
}