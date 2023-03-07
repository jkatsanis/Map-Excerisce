namespace Dictionary;

public class MyDictionary<TKey, TValue>
{
    private const int InitialBuckets = 5;
    private const int MaxDepth = 4;
    private List<KeyValue>[] _buckets;
    private int _bucketCnt => _buckets.Length;
    public int Count { get; private set; }

    /// <summary>
    /// Gets the value linked to the key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValue? this[TKey key]
    {
        //TODO
        get => default;
    }

    /// <summary>
    /// Creates a Dictionary with the cnt buckets
    /// </summary>
    /// <param name="cnt"></param>
    public MyDictionary(int cnt)
    {
        //TODO
    }

    /// <summary>
    /// Gets all Keys from the Dictionary
    /// </summary>
    /// <returns>A List<Key> with all keys</returns>
    public List<TKey> GetKeys()
    {
        //TODO
        return null;
    }

    /// <summary>
    /// Gets all Values in the dictionary
    /// </summary>
    /// <returns>A List<TValue> with all values</returns>
    public List<TValue> GetValues()
    {
        //TODO
        return null;
    }

    /// <summary>
    /// Gets the Index from the Bucket the key should be in
    /// </summary>
    /// <param name="key"></param>
    /// <returns>The index of the bucket</returns>
    private int GetBucketIndex(TKey key)
    {
        //TODO
        return -1;
    }

    /// <summary>
    /// Adds an Item to the Dictionary and Grows if the bucket is to deep
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TKey key, TValue value)
    {
        //TODO
    }

    /// <summary>
    /// Adds an Bucket to the Dictionary
    /// </summary>
    public void Grow()
    {
        //TODO
    }

    /// <summary>
    /// Removes the KeyValue based on the key
    /// </summary>
    /// <param name="key"></param>
    /// <returns>true if removed, false if couldn't find key</returns>
    public bool Remove(TKey key)
    {
        //TODO
        return false;
    }

    /// <summary>
    /// Gets a Value based on a key
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>true if it worked and false if not found</returns>
    public bool TryGetValue(TKey key, out TValue? value)
    {
        //TODO
        value = default;
        return false;
    }

    /// <summary>
    /// Checks if the key is contained in the dictionary
    /// </summary>
    /// <param name="key"></param>
    /// <returns>true if the key is contained and false if not</returns>
    public bool ContainsKey(TKey key)
    {
        foreach (var bucket in _buckets)
        {
            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public class KeyValue
    {
        public TKey Key;
        public TValue Value;
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value  = value;
        }
    }
}