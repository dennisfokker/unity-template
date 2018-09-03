using System;

[Serializable]
public class LongRange : Range<long>
{
    public LongRange(long minimum, long maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }
}
