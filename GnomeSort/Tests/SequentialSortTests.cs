using System.Diagnostics;
using GnomeSort.Input;
using GnomeSort.Sorters;

namespace GnomeSort.Tests;

public class SequentialSortTests
{
    public static void RunSort()
    {
        var stopwatch = new Stopwatch();
        var inputCatcher = new InputCatcher();
        var sequentialSorter = new SequentialHybridHybridGnomeMergeSorter<int>();
        
        var arrayLength = inputCatcher.CatchArrayLength();

        var random = new Random();
        const int minRandomValue = 0;
        const int maxRandomValue = 1000;
        var randomArray = ArrayUtils.GenerateRandomArray(arrayLength, () => 
            random.Next(minRandomValue, maxRandomValue));
        
        Console.WriteLine("Sorting array...\n");
        
        var sequentiallySortedArray = Array.Empty<int>();
        stopwatch.Start();
        
        try
        {
            sequentiallySortedArray = sequentialSorter.Sort(randomArray);
            var sequentialArraySortingTime = stopwatch.ElapsedMilliseconds;
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
        
        Console.WriteLine();
        Console.WriteLine("Sequential Gnome Sort result is sorted " 
                          + (ArrayUtils.IsSortedAscending(sequentiallySortedArray) 
                              ? "correctly." : "incorrectly."));
    }

    public static void RunSortTests()
    {
        int[] arraySizes = { 50003, 500003, 1500003, 4000003, 7000003, 11000003 };
        const int numTestsPerSize = 3;

        var stopwatch = new Stopwatch();
        var sequentialSorter = new SequentialHybridHybridGnomeMergeSorter<int>();

        var random = new Random();
        const int minRandomValue = 0;
        const int maxRandomValue = 1000;

        foreach (var arrayLength in arraySizes)
        {
            Console.WriteLine($"Testing with array size: {arrayLength}");
            long totalTime = 0;
            
            for (var i = 0; i < numTestsPerSize; i++)
            {
                var randomArray = ArrayUtils.GenerateRandomArray(arrayLength, () => 
                    random.Next(minRandomValue, maxRandomValue));

                stopwatch.Restart();
                _ = sequentialSorter.Sort(randomArray);
                stopwatch.Stop();

                totalTime += stopwatch.ElapsedMilliseconds;
            }

            var meanTime = (double)totalTime / numTestsPerSize;
            Console.WriteLine($"Mean Sequential Gnome Sort time: {meanTime:F2} ms");
            Console.WriteLine(); 
        }
    }
}