using System;

[Serializable]
public class IntRange : Range<int>
{
    public IntRange(int minimum, int maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }
}
