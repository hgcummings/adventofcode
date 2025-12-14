using System.Collections;
using System.Collections.Concurrent;

namespace HGC.AOC.Common;

public class Tally<TKey> : IEnumerable<KeyValuePair<TKey, long>> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, long> _table; 

    public Tally()
    {
        _table = new ConcurrentDictionary<TKey, long>();
    }

    public TKey Highest => _table.MaxBy(entry => entry.Value).Key;
    public TKey Lowest => _table.MinBy(entry => entry.Value).Key;

    public long Total => _table.Sum(entry => entry.Value);
    public long HighestValue => _table.Max(entry => entry.Value);

    public void Increase(TKey key, long amount)
    {
        _table.AddOrUpdate(key, _ => amount, (_, x) => x + amount);
    }
    
    public void Increment(TKey key)
    {
        _table.AddOrUpdate(key, _ => 1, (_, x) => x + 1);
    }

    public IEnumerator<KeyValuePair<TKey, long>> GetEnumerator()
    {
        return _table.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}