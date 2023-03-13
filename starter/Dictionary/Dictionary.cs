namespace Dictionary;

/// <summary>
///     Class for a HashMap
/// </summary>
/// <typeparam name="TKey">Key</typeparam>
/// <typeparam name="TValue">Value</typeparam>
public class MyDictionary<TKey, TValue>
{
    private const int InitialBuckets = 5;
    private const int MaxDepth = 4;
    private List<KeyValue>[] _buckets;
    private int BucketCnt => _buckets.Length;

    /// <summary>
    ///     Count of the items in the HashMap
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    ///     Gets the value linked to the key
    /// </summary>
    /// <param name="key">key of the HashMap</param>
    /// <returns>Key of the required value</returns>
    public TValue? this[TKey key]
    {
        get
        {
            var value = default(TValue);
            return TryGetValue(key, out value) ? value : default;
        }
        set => Add(key, value!);
    }

    /// <summary>
    ///     Creates a new instance of the dictionary with default capacity
    /// </summary>
    public MyDictionary() : this(InitialBuckets)
    {

    }
    private MyDictionary(int cnt)
    {
        _buckets = CreateBuckets(cnt);
    }

    /// <summary>
    ///     Gets all Keys from the Dictionary
    /// </summary>
    /// <returns>A List with all keys</returns>
    public List<TKey> GetKeys()
    {
        var keys = new List<TKey>(Count);
        for (var i = 0; i < BucketCnt; i++)
        {
            foreach (var keyValue in _buckets[i])
            {
                keys.Add(keyValue.Key);
            }
        }

        return keys;
    }

    /// <summary>
    ///     Gets all Values in the dictionary
    /// </summary>
    /// <returns>A List with all values</returns>
    public List<TValue> GetValues()
    {
        var values = new List<TValue>(Count);
        for (var i = 0; i < BucketCnt; i++)
        {
            foreach (var keyValue in _buckets[i])
            {
                values.Add(keyValue.Value);
            }
        }

        return values;
    }
    private int GetBucketIndex(TKey key)
    {
        if (key == null)
        {
            return 0;
        }

        var hash = key.GetHashCode();
        var index = hash % _buckets.Length;
        return index >= 0 ? index : -index;
    }

    /// <summary>
    ///     Adds an Item to the Dictionary and 
    ///     Grows if the bucket is too deep
    /// </summary>
    /// <param name="key">Key of the Value</param>
    /// <param name="value">Value to store</param>
    public void Add(TKey key, TValue value)
    {
        if (key == null)
        {
            return;
        }

        var buckets = _buckets[GetBucketIndex(key)];

        foreach (var keyValue in buckets)
        {
            if (keyValue.Key!.Equals(key))
            {
                keyValue.Value = value;
                return;
            }
        }

        buckets.Add(new KeyValue(key, value));

        if (buckets.Count > MaxDepth)
        {
            Grow();
        }

        Count++;
    }

    /// <summary>
    ///     Adds an Bucket to the Dictionary
    /// </summary>
    public void Grow()
    {
        var newBuckets = CreateBuckets(_buckets.Length * 2);

        foreach (var keyValue in GetItems())
        {
            var bucket = newBuckets[GetBucketIndex(keyValue.Key)];
            bucket.Add(keyValue);
        }

        _buckets = newBuckets;
    }

    /// <summary>
    ///     Removes the KeyValue based on the key
    /// </summary>
    /// <param name="key">key to look for</param>
    /// <returns>true if removed, false if couldn't find key</returns>
    public bool Remove(TKey key)
    {
        if (key == null)
        {
            return false;
        }

        var buckets = _buckets[GetBucketIndex(key)];

        for (var i = 0; i < buckets.Count; i++)
        {
            if (!buckets[i].Key!.Equals(key)) continue;
            buckets.RemoveAt(i);
            Count--;
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Gets a Value based on a key
    /// </summary>
    /// <param name="key">key to look for</param>
    /// <param name="value">Out param for storing value if found</param>
    /// <returns>true if it worked and false if not found</returns>
    public bool TryGetValue(TKey key, out TValue? value)
    {
        if (key == null)
        {
            value = default;
            return false;
        }

        if (TryGetValue(key, out KeyValue? existingKeyValue))
        {
            value = existingKeyValue!.Value;
            return true;
        }

        value = default;
        return false;
    }
    private bool TryGetValue(TKey key, out KeyValue? existingKeyValue)
    {
        existingKeyValue = null;
        var bucketIndex = GetBucketIndex(key);
        for (var i = 0; i < _buckets[bucketIndex].Count; i++)
        {
            if (!_buckets[bucketIndex][i].Key!.Equals(key)) continue;
            existingKeyValue = _buckets[bucketIndex][i];
            return true;
        }
        return false;
    }
    public List<KeyValue> GetItems()
    {
        var items = new List<KeyValue>(Count);
        for (var i = 0; i < BucketCnt; i++)
        {
            items.AddRange(_buckets[i]);
        }

        return items;
    }

    /// <summary>
    ///     Checks if the key is contained in the dictionary
    /// </summary>
    /// <param name="key">key to check</param>
    /// <returns>true if the key is contained and false if not</returns>
    public bool ContainsKey(TKey key)
    {
        if (key == null)
        {
            return false;
        }

        var buckets = _buckets[GetBucketIndex(key)];

        foreach (var keyValue in buckets)
        {
            if (keyValue.Key!.Equals(key))
            {
                return true;
            }
        }

        return false;
    }
    private static List<KeyValue>[] CreateBuckets(int amount)
    {
        var buckets = new List<KeyValue>[amount];
        for (var i = 0; i < amount; i++)
        {
            buckets[i] = new(MaxDepth + 1);
        }

        return buckets;
    }
    /// <summary>
    ///     Class for the Key Values
    /// </summary>
    public class KeyValue
    {
        /// <summary>
        ///     Property to get the Key
        /// </summary>
        public TKey Key;

        /// <summary>
        ///     Property to get the Value
        /// </summary>
        public TValue Value;

        /// <summary>
        ///     Constructor for the KeyValue
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}