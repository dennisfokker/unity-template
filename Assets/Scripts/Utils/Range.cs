using System;
using System.Collections.Generic;

/// <summary>
/// Defines the max/min of a range of values.
/// </summary>
/// <typeparam name="T">Type of range. Must be IComparable</typeparam>
[Serializable]
public abstract class Range<T> where T : IComparable<T>
{
    public T Minimum
    {
        get
        {
            return minimum;
        }
        set
        {
            minimum = value;

            if (!IsValid())
            {
                T temp = minimum;
                minimum = maximum;
                maximum = temp;
            }
        }
    }
    public T Maximum {
        get
        {
            return maximum;
        }
        set
        {
            maximum = value;

            if (!IsValid())
            {
                T temp = maximum;
                maximum = minimum;
                minimum = temp;
            }
        }
    }

    private T minimum;
    private T maximum;

    /// <summary>
    /// Initializes a new instance of the Range structure to the specified maximum and minimum.
    /// Automatically switches the values around if invalid.
    /// </summary>
    /// <param name="minimum">Minimum value.</param>
    /// <param name="maximum">Maximum value.</param>
    public Range(T minimum, T maximum)
    {
        if (IsValid())
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }
        else
        {
            this.minimum = maximum;
            this.maximum = minimum;
        }
    }

    /// <summary>
    /// Check if value is contained withing the range.
    /// </summary>
    /// <param name="value">Value to check agains.</param>
    /// <returns>Whether or not the value is inside the range.</returns>
    public bool ContainsValue(T value)
    {
        return (minimum.CompareTo(value) <= 0) && (value.CompareTo(maximum) <= 0);
    }

    private bool IsValid()
    {
        return minimum.CompareTo(maximum) <= 0;
    }

    public static bool operator ==(Range<T> x, Range<T> y)
    {
        if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.minimum.CompareTo(y.minimum) == 0 && x.maximum.CompareTo(y.maximum) == 0;
    }

    public static bool operator !=(Range<T> x, Range<T> y)
    {
        if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            return false;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return true;

        return x.minimum.CompareTo(y.minimum) != 0 || x.maximum.CompareTo(y.maximum) != 0;
    }

    public override string ToString()
    {
        return string.Format("[{0} - {1}]", minimum, maximum);
    }

    public override bool Equals(object obj)
    {
        var range = obj as Range<T>;
        return range != null &&
               EqualityComparer<T>.Default.Equals(minimum, range.minimum) &&
               EqualityComparer<T>.Default.Equals(maximum, range.maximum);
    }

    public override int GetHashCode()
    {
        var hashCode = -1421831408;
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(minimum);
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(maximum);
        return hashCode;
    }
}

#region Non-generic implementations to allow serialization
[Serializable]
public class DoubleRange : Range<double>
{
    public DoubleRange(double minimum, double maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }

    public static implicit operator DoubleRange(FloatRange range)
    {
        return new DoubleRange(range.Minimum, range.Maximum);
    }

    public static implicit operator DoubleRange(IntRange range)
    {
        return new DoubleRange(range.Minimum, range.Maximum);
    }

    public static implicit operator DoubleRange(LongRange range)
    {
        return new DoubleRange(range.Minimum, range.Maximum);
    }
}

[Serializable]
public class FloatRange : Range<float>
{
    public FloatRange(float minimum, float maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }

    public static implicit operator FloatRange(DoubleRange range)
    {
        return new FloatRange((float) range.Minimum, (float) range.Maximum);
    }

    public static implicit operator FloatRange(IntRange range)
    {
        return new FloatRange(range.Minimum, range.Maximum);
    }

    public static implicit operator FloatRange(LongRange range)
    {
        return new FloatRange(range.Minimum, range.Maximum);
    }
}

[Serializable]
public class IntRange : Range<int>
{
    public IntRange(int minimum, int maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }

    public static implicit operator IntRange(DoubleRange range)
    {
        return new IntRange((int) range.Minimum, (int) range.Maximum);
    }

    public static implicit operator IntRange(FloatRange range)
    {
        return new IntRange((int) range.Minimum, (int) range.Maximum);
    }

    public static implicit operator IntRange(LongRange range)
    {
        return new IntRange((int) range.Minimum, (int) range.Maximum);
    }
}

[Serializable]
public class LongRange : Range<long>
{
    public LongRange(long minimum, long maximum) : base(minimum, maximum)
    {
        // Nothing special to do
    }

    public static implicit operator LongRange(DoubleRange range)
    {
        return new LongRange((long) range.Minimum, (long) range.Maximum);
    }

    public static implicit operator LongRange(FloatRange range)
    {
        return new LongRange((long) range.Minimum, (long) range.Maximum);
    }

    public static implicit operator LongRange(IntRange range)
    {
        return new LongRange(range.Minimum, range.Maximum);
    }
}
#endregion