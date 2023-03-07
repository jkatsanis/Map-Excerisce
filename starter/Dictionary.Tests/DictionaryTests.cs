using FluentAssertions;
using Xunit;

namespace Dictionary.Tests;

public sealed class DictionaryTests
{
    [Fact]
    public void Construction()
    {
        var dic = new Dictionary<int, string>();
        dic.Should().NotBeNull().And.HaveCount(0);
    }

    [Fact]
    public void Add_Single()
    {
        var dic = new Dictionary<int, string>();

        const string word = "hello";
        const int key = 2;

        dic.Add(key, word);
        dic.Count.Should().Be(1, "word got added to the dictionary");
        dic.ContainsKey(key).Should().BeTrue();
        dic.TryGetValue(key, out var value).Should().BeTrue();
        value.Should().BeSameAs(word);
    }

    [Fact]
    public void Add_Multiple()
    {
        var dic = new Dictionary<int, string>();

        string[] words = { "hello", "World", "program" };
        int[] keys = { 5, 4, 3 };

        for (int i = 0; i < words.Length; i++)
        {
            dic.Add(keys[i], words[i]);
        }

        dic.Count.Should().Be(3, "3 words were added to the dictionary");

        for (int i = 0; i < words.Length; i++)
        {
            dic.ContainsKey(keys[i]).Should().BeTrue();
            dic.TryGetValue(keys[i], out var value).Should().BeTrue();
            value.Should().Be(words[i]);
        }
    }

    [Fact]
    public void Add_SameKey()
    {
        var dic = new MyDictionary<int, string>();

        const string word = "hello";
        const string word1 = "world";
        const int Key = 2;

        dic.Add(Key, word);
        dic.Add(Key, word1);

        dic.Count.Should().Be(1, "because of the same key, old word was removed");
        dic.ContainsKey(Key).Should().BeTrue();
        dic.TryGetValue(Key, out var value).Should().BeTrue();
        value.Should().Be(word1, "old word was replaced with new word");
    }

    [Fact]
    public void Remove_Single()
    {
        var dic = new MyDictionary<int, bool>();

        const int Key = 12;

        dic.Add(Key, true);

        dic.Remove(Key).Should().BeTrue("key exists => could be removed");
        dic.Count.Should().Be(0, "Count reduced because of deletion");
    }

    [Fact]
    public void Remove_InvalidKey()
    {
        var dic = new MyDictionary<int, bool>();

        dic.Add(12, true);

        dic.Remove(21).Should().BeFalse("no such key in the dictionary");
        dic.Count.Should().Be(1, "count stays the same");
    }

    [Fact]
    public void ContainsKey_NotExists()
    {
        var dic = new MyDictionary<int, long>();

        dic.Add(12, 74374623528);

        dic.ContainsKey(21).Should().BeFalse("dictionary does not contain such a key");
    }

    [Fact]
    public void ContainsKey_Exists()
    {
        var dic = new MyDictionary<int, long>();

        dic.Add(12, 74374623528);

        dic.ContainsKey(12).Should().BeTrue();
    }

    [Fact]
    public void GetKeys()
    {
        var dic = new MyDictionary<int, int>();

        var keys = new[] { 1, 2, 3 };
        var values = new[] { 2, 7, 13 };

        for (var i = 0; i < keys.Length; i++)
        {
            dic.Add(keys[i], values[i]);
        }

        dic.GetKeys().Should().Contain(keys, "contains all keys").And.HaveCount(keys.Length);
    }

    [Fact]
    public void GetValues()
    {
        var keys = new[] { 1, 2, 3 };
        var values = new[] { 2, 7, 13 };

        var dic = new MyDictionary<int, int>();

        for (var i = 0; i < keys.Length; i++)
        {
            dic.Add(keys[i], values[i]);
        }

        dic.GetValues().Should().Contain(values, "contains all values").And.HaveCount(values.Length);
    }

    [Fact]
    public void TryGetValue()
    {
        var dic = new MyDictionary<string, decimal>();
        dic.Add("A", 14.33M);

        dic.TryGetValue("A", out var value).Should().BeTrue("value was found");
        value.Should().Be(14.33M);
    }

    [Fact]
    public void TryGetValue_NotExists()
    {
        var dic = new MyDictionary<string, decimal>();
        dic.Add("Z", 70.21M);

        dic.TryGetValue("L", out var value).Should().BeFalse("no value to the given key was found");
        value.Should().Be(default, "returns default if no value to the key was found");
    }

    [Fact]
    public void Indexer_NotExists_Value()
    {
        var dic = new MyDictionary<int, string>();

        dic[1].Should().Be(default, "not found => default string returned");
    }
}