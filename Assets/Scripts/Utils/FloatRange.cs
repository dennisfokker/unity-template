using System;

[Serializable]
public class FloatRange : Range<float>
{
    public FloatRange(float minimum, float maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }
}
