using System.Collections.Concurrent;

namespace GnomeSort.Sorters;

class ParallelHybridHybridGnomeMergeSorter<T> : HybridGnomeMerge<T> where T : IComparable<T>
{
    public T[] Sort(T[] array, int numberOfThreads)
    {
        ValidateParameters(array, ref numberOfThreads);
        var segmentSize = CalculateOptimalSegmentSize(array);
        var numberOfSegments = (int)Math.Ceiling((double)array.Length / segmentSize);

        var segmentData = new T[array.Length];
        var segments = new ArraySegment<T>[numberOfSegments];

        for (var i = 0; i < numberOfSegments; i++)
        {
            var start = i * segmentSize;
            var end = Math.Min(start + segmentSize, array.Length);

            segments[i] = new ArraySegment<T>(segmentData, start, end - start);
            Array.Copy(array, start, segmentData, start,
                end - start);
        }

        var rangePartitioner = Partitioner.Create(0, segments.Length,
            Math.Max(1, segments.Length / numberOfThreads));

        Parallel.ForEach(rangePartitioner, (range, _) =>
        {
            for (var i = range.Item1; i < range.Item2; i++)
            {
                GnomeSort(segments[i].Array, segments[i].Offset,
                    segments[i].Offset + segments[i].Count - 1);
            }
        });

        var result = MergeSegments(segments);

        return result;
    }

    private void ValidateParameters(T[] array, ref int numberOfThreads)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), "Input array cannot be null.");
        }

        if (numberOfThreads <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfThreads),
                "Number of threads must be greater than zero.");
        }

        if (array.Length < numberOfThreads)
        {
            numberOfThreads = array.Length;
        }
    }
}