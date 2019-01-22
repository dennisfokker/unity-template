using System;

[Serializable]
public class Position
{
    public int X;
    public int Y;

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Position x, Position y)
    {
        if (object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null)) return true;

        if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) return false;

        return x.X.CompareTo(y.X) == 0 && x.Y.CompareTo(y.Y) == 0;
    }

    public static bool operator !=(Position x, Position y)
    {
        if (object.ReferenceEquals(x, null) && object.ReferenceEquals(y, null)) return false;

        if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null)) return true;

        return x.X.CompareTo(y.X) != 0 || x.Y.CompareTo(y.Y) != 0;
    }

    public override string ToString()
    {
        return string.Format("[{0} - {1}]", this.X, this.Y);
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Position))
        {
            return false;
        }

        return X.CompareTo(((Position) obj).X) == 0 && Y.CompareTo(((Position) obj).Y) == 0;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }

    public bool ContainsValue(int value)
    {
        return (this.X.CompareTo(value) <= 0) && (value.CompareTo(this.Y) <= 0);
    }
}