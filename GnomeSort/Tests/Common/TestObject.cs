namespace GnomeSort.Tests.Common;

public class TestObject(int value) : IComparable<TestObject>
{
    public int Value { get; set; } = value;
    
    public int CompareTo(TestObject? other)
    {
        return other == null 
            ? 1 : Value.CompareTo(other.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}