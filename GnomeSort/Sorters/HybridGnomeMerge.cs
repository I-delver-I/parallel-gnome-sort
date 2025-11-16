namespace GnomeSort.Sorters;

public abstract class HybridGnomeMerge<T> where T : IComparable<T>
{
    protected void GnomeSort(T[] array, int startIndex, int endIndex)
    {
        var currentIndex = startIndex + 1;

        while (currentIndex <= endIndex)
        {
            if (currentIndex == startIndex
                || array[currentIndex].CompareTo(array[currentIndex - 1]) >= 0)
            {
                currentIndex++;
            }
            else
            {
                Swap(array, currentIndex, currentIndex - 1);
                currentIndex--;
            }
        }
    }

    protected T[] MergeSegments(ArraySegment<T>[] segments)
    {
        if (segments.Length == 1)
        {
            return segments[0].Array;
        }
        
        var mergedArray = new T[segments[0].Array!.Length];
        var minHeap = new SortedSet<(T Value, int SegmentIndex, int IndexInSegment)>
        (Comparer<(T, int, int)>.Create((a, b) =>
        {
            var comparison = a.Item1.CompareTo(b.Item1);
            return comparison != 0 
                ? comparison : a.Item2.CompareTo(b.Item2);
        }));

        for (var i = 0; i < segments.Length; i++)
        {
            if (segments[i].Count > 0)
            {
                minHeap.Add((segments[i].Array![segments[i].Offset], i, 0));
            }
        }

        var mergedIndex = 0;

        while (minHeap.Count > 0)
        {
            var (smallestValue, segmentIndex, indexInSegment) = minHeap.Min;
            minHeap.Remove(minHeap.Min);
            mergedArray[mergedIndex++] = smallestValue;
            var nextIndexInSegment = indexInSegment + 1;
            
            if (nextIndexInSegment < segments[segmentIndex].Count)
            {
                minHeap.Add((segments[segmentIndex].Array![segments[segmentIndex]
                    .Offset + nextIndexInSegment], segmentIndex, nextIndexInSegment));
            }
        }

        return mergedArray;
    }

    protected int CalculateOptimalSegmentSize(T[] array)
    {
        const int minimumSegmentSize = 64;

        if (array.Length <= minimumSegmentSize)
        {
            return array.Length;
        }

        var baseSegmentSize = array.Length switch
        { 
            <= 1000 => Math.Min(minimumSegmentSize, array.Length / 8),     
            <= 100000 => Math.Max(minimumSegmentSize, array.Length / 500), 
            <= 1000000 => Math.Max(minimumSegmentSize, array.Length / 1300),
            _ => Math.Max(minimumSegmentSize, array.Length / 2000)         
        };

        return baseSegmentSize;
    }

    protected void Swap(T[] array, int index1, int index2)
    {
        (array[index1], array[index2]) = (array[index2], array[index1]);
    }
}