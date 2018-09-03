using System;

[Serializable]
public class Range<T> where T : IComparable<T>
{
    public T Minimum;
    public T Maximum;

    public Range(T minimum, T maximum)
    {
        Minimum = minimum;
        Maximum = maximum;

        if (!IsValid())
        {
            Minimum = maximum;
            Maximum = minimum;
        }
    }

    public static bool operator ==(Range<T> x, Range<T> y)
    {
        if (object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null)) return true;

        if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) return false;

        return x.Minimum.CompareTo(y.Minimum) == 0 && x.Maximum.CompareTo(y.Maximum) == 0;
    }

    public static bool operator !=(Range<T> x, Range<T> y)
    {
        if (object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null)) return false;

        if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) return true;

        return x.Minimum.CompareTo(y.Minimum) != 0 || x.Maximum.CompareTo(y.Maximum) != 0;
    }

    public override string ToString()
    {
        return string.Format("[{0} - {1}]", this.Minimum, this.Maximum);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Range<T>))
        {
            return false;
        }

        return Minimum.CompareTo(((Range<T>) obj).Minimum) == 0 && Maximum.CompareTo(((Range<T>) obj).Maximum) == 0;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + Minimum.GetHashCode();
            hash = hash * 23 + Maximum.GetHashCode();
            return hash;
        }
    }

    public bool ContainsValue(T value)
    {
        return (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);
    }

    private bool IsValid()
    {
        return this.Minimum.CompareTo(this.Maximum) <= 0;
    }
}