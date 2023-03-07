using FluentAssertions;
using Xunit;

namespace Dictionary.Tests;

public sealed class DictionaryManagerTests
{
    private static readonly List<string> Languages = new List<string>
    {
        "english",
        "german",
        "spanish",
        "russian"
    };

    [Fact]
    public void Ctor_NoCSV_Intended()
    {
        var dM = new DictionaryManager();
        dM.AllWords.Should().Be(new MyDictionary<string, MyDictionary<string, WordData>>(), "no words yet");
        dM.AllTranslations.Should().BeEmpty("no translations yet");
    }

    [Fact]
    public void Ctor_CSV_Valid()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.AllWords.Should().BeEquivalentTo(GetValidAllWords(), "Words are loaded in order");
        dM.AllTranslations.Should().BeEquivalentTo(GetAllValidTranslations(), "Translations are loaded in order");
        dM.AllWords["cat"].Should().BeSameAs(dM.AllTranslations[0], "same instance of translations is used for words");
        dM.Languages.Should().Equal(Languages);
    }

    [Fact]
    public void GetWordInformation_Simple()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.GetWordInformation("cat").Should().Be("Language        Word            Usage\n"
                                                 + "English         Cat             often\n"
                                                 + "German          Katze           never\n"
                                                 + "Spanish         Gato            rarely\n"
                                                 + "Russian         Кот             sometimes\n");
    }

    [Fact]
    public void GetWordInformation_DoesNotExist()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.GetWordInformation("sophie").Should().BeNull();
    }

    [Theory]
    [InlineData("cat", true)]
    [InlineData("Cat", true)]
    [InlineData("cAt", true)]
    [InlineData("handgranate", false)]
    [InlineData("sun", true)]
    [InlineData("katze", true)]
    [InlineData("kaTze", true)]
    [InlineData("perro", true)]
    [InlineData("perRo", true)]
    [InlineData("собака", true)]
    public void Contains_Simple(string word, bool expected)
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.Contains(word).Should().Be(expected, "Case is ignored in every language.");
    }

    [Fact]
    public void GetPairs_English_German_Simple()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        var pairs = new List<string[]>
        {
            new[] { "cat", "katze" },
            new[] { "dog", "hund" },
            new[] { "sun", "sonne" }
        };
        dM.GetDictionary("english", "german").Should().Equal(pairs);
    }

    [Fact]
    public void GetPairs_Russian_German_Simple()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        var pairs = new List<string[]>
        {
            new[] { "кот", "katze" },
            new[] { "собака", "hund" },
            new[] { "солнце", "sonne" }
        };
        dM.GetDictionary("russian", "german").Should().Equal(pairs);
    }

    [Fact]
    public void GetPairs_Invalid_Lang()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.GetDictionary("french", "german").Should().BeNull();
    }

    [Fact]
    public void TryAddLanguage_Simple()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.TryAddLanguage("Arabian").Should().BeTrue("Arabian is not yet a language in the dictionary");
        var newLang = new List<string>();
        Languages.ForEach(x => newLang.Add(x));
        newLang.Add("arabian"); //Has to be lower invariant
        dM.Languages.Should().Equal(newLang, "Arabian was added as a language (As lwoer invariant)");
    }

    [Fact]
    public void TryAddLanguage_Contained()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.TryAddLanguage("English").Should().BeFalse("English is already contained in the dictionary");
        dM.Languages.Should().Equal(Languages, "nonthing changed");
    }

    [Fact]
    public void SaveDictionary_Simple()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.SaveDictionary(@"TestData\savedDictionary.csv").Should().BeTrue("File exists");
        var lines = File.ReadAllLines(@"TestData\savedDictionary.csv");
        lines[0].Should().Be("english;german;spanish;russian", "language header required");
        lines[1].Should().Be("cat-often;katze-never;gato-rarely;кот-sometimes", "data saved in order");
        lines[2].Should().Be("dog-sometimes;hund-often;perro-never;собака-often", "data saved in order");
        lines[3].Should().Be("sun-sometimes;sonne-rarely;sol-sometimes;солнце-never", "data saved in order");
        File.WriteAllLines(@"TestData\savedDictionary.csv", Array.Empty<string>());
    }

    [Fact]
    public void SaveDictionary_NoFile()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.SaveDictionary("Dass das Dass das Das ersetzt, das widerstebt mir").Should().BeFalse("file does not exist");
    }

    [Fact]
    public void ToString_Simple()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.ToString().Should().Be(
            "      English         Usage          German          Usage          Spanish         Usage          Russian         Usage\n"
            + "0001. Cat             often          Katze           never          Gato            rarely         Кот             sometimes\n"
            + "0002. Dog             sometimes      Hund            often          Perro           never          Собака          often\n"
            + "0003. Sun             sometimes      Sonne           rarely         Sol             sometimes      Солнце          never\n");
    }

    [Fact]
    public void ToString_Empty()
    {
        var dM = new DictionaryManager();
        dM.ToString().Should()
            .Be(string.Empty, "no languages etc");
    }

    [Fact]
    public void ChangeInformation_Add()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.ChangeInformations("c++-never", "Ungabunga", -1, true).Should()
            .BeTrue("new language \"unagebunga\" was added and word was added to translations");
        var newLangs = new List<string>();
        Languages.ForEach(x => newLangs.Add(x));
        newLangs.Add("ungabunga");
        dM.Languages.Should().Equal(newLangs, "a language was added");
    }

    [Fact]
    public void ChangeInformation_Change()
    {
        var dM = new DictionaryManager(@"TestData\ValidTestData.csv");
        dM.ChangeInformations("Handgranate", "english", 1).Should()
            .BeTrue("word \"cat\" has been changed to \"handgranate\"");
        dM.Languages.Should().Equal(Languages);
        dM.GetWordInformation("HanDGrante").Should().NotBeNull("cat has been exchanged for different word");
    }

    private static MyDictionary<string, MyDictionary<string, WordData>> GetValidAllWords()
    {
        var dic = new MyDictionary<string, MyDictionary<string, WordData>>();
        var translations = GetAllValidTranslations();
        var catTranslations = translations[0];
        var dogTranslations = translations[1];
        var sunTranslations = translations[2];


        dic.Add("cat", catTranslations);
        dic.Add("katze", catTranslations);
        dic.Add("gato", catTranslations);
        dic.Add("кот", catTranslations);
        dic.Add("dog", dogTranslations);
        dic.Add("hund", dogTranslations);
        dic.Add("perro", dogTranslations);
        dic.Add("собака", dogTranslations);
        dic.Add("sun", sunTranslations);
        dic.Add("sonne", sunTranslations);
        dic.Add("sol", sunTranslations);
        dic.Add("солнце", sunTranslations);

        return dic;
    }

    private static List<MyDictionary<string, WordData>> GetAllValidTranslations()
    {
        var catTranslations = new MyDictionary<string, WordData>();
        catTranslations.Add("english", new WordData("cat", Usage.often));
        catTranslations.Add("german", new WordData("katze", Usage.never));
        catTranslations.Add("spanish", new WordData("gato", Usage.rarely));
        catTranslations.Add("russian", new WordData("кот", Usage.sometimes));
        var dogTranslations = new MyDictionary<string, WordData>();
        dogTranslations.Add("english", new WordData("dog", Usage.sometimes));
        dogTranslations.Add("german", new WordData("hund", Usage.often));
        dogTranslations.Add("spanish", new WordData("perro", Usage.never));
        dogTranslations.Add("russian", new WordData("собака", Usage.often));
        var sunTranslations = new MyDictionary<string, WordData>();
        sunTranslations.Add("english", new WordData("sun", Usage.sometimes));
        sunTranslations.Add("german", new WordData("sonne", Usage.rarely));
        sunTranslations.Add("spanish", new WordData("sol", Usage.sometimes));
        sunTranslations.Add("russian", new WordData("солнце", Usage.never));

        var list = new List<MyDictionary<string, WordData>>();
        list.Add(catTranslations);
        list.Add(dogTranslations);
        list.Add(sunTranslations);

        return list;
    }
}