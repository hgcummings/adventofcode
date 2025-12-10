namespace HGC.AOC.Common;

public struct LongRange
{
    public static LongRange FromLength(long from, long length)
    {
        return new LongRange(from, from + length);
    }

    public static LongRange FromToInclusive(long from, long toInc)
    {
        return new LongRange(from, toInc + 1);
    }
        
    private LongRange(long from, long toExc)
    {
        From = from;
        ToExc = toExc;
    }

    public long From { get; }
    public long ToExc { get; }

    public bool Contains(long value)
    {
        return value >= this.From && value < this.ToExc;
    }

    public bool Intersects(LongRange other)
    {
        return this.From < other.ToExc && this.ToExc > other.From;
    }

    public LongRange Shift(long shift)
    {
        return new LongRange(From + shift, ToExc + shift);
    }

    public LongRange Intersect(LongRange other, out List<LongRange> remainder)
    {
        if (!this.Intersects(other))
        {
            throw new Exception("Ranges don't intersect");
        }

        var intFrom = Math.Max(From, other.From);
        var intersection = new LongRange(intFrom, Math.Min(ToExc, other.ToExc));

        remainder = new List<LongRange>();
            
        if (From < intFrom)
        {
            remainder.Add(new LongRange(From, intFrom));
        }

        if (ToExc > intersection.ToExc)
        {
            remainder.Add(new LongRange(intersection.ToExc, ToExc));
        }

        return intersection;
    }

    public override string ToString()
    {
        return String.Format("[{0},{1})", From, ToExc);
    }
}