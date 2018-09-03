using System;

[Serializable]
public class DoubleRange : Range<double>
{
    public DoubleRange(double minimum, double maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }
}
