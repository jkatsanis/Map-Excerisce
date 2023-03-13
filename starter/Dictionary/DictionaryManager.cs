using Microsoft.VisualBasic;
using System.ComponentModel;

namespace Dictionary;

public sealed class DictionaryManager
{
    private const char Seperator = ';';
    private const char WordDataSeperator = '-';
    /// <summary>
    /// The key is the word you want the translations of
    /// The value therefore is a dictionary of all the translations each translation is associated
    /// with the corresponding language 
    /// </summary>
    public readonly MyDictionary<string, MyDictionary<string, WordData>> AllWords;
    /// <summary>
    /// Contains a dictionary of all translations for every word
    /// The contained dictionaries ARE THE SAME as the ones of _allWords
    /// 
    /// IMPORTANT: The same refers to them being the SAME INSTANCE
    /// </summary>
    public readonly List<MyDictionary<string, WordData>> AllTranslations;

    public List<string> Languages { get; }

    /// <summary>
    ///     Constructs a <see cref="DictionaryManager"/> which doesn't contain any information.
    /// </summary>
    public DictionaryManager()
    {
        this.AllWords = new MyDictionary<string, MyDictionary<string, WordData>>();
        this.AllTranslations = new List<MyDictionary<string, WordData>>();
        this.Languages = new List<string>();
    }

    /// <summary>
    ///     Constructs a <see cref="DictionaryManager"/> which contains the information from the csv file.
    /// </summary>
    /// <param name="wordsDataPath">the path to the csv file with the information</param>
    public DictionaryManager(string wordsDataPath) : this()
    {
        InitializeWordsFromFile(wordsDataPath);
        GenerateAllWordsFromTranslations();
    }

    public bool SaveDictionary(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        string[] lines = new string[AllTranslations.Count + 1];
        lines[0] = string.Join(';', Languages);

        for (int i = 0; i < AllTranslations.Count; i++)
        {
            string line = string.Empty;
            for (int j = 0; j < Languages.Count; j++)
            {
                if (j != 0)
                {
                    line += ";";
                }
                line += $"{AllTranslations[i][Languages[j]]?.Word}" +
                        $"{WordDataSeperator}" +
                        $"{AllTranslations[i][Languages[j]]?.Usage}";
            }

            lines[i + 1] = line;
        }

        File.WriteAllLines(path, lines);

        return true;
    }

    /// <summary>
    ///     Changes or add the information of a word.
    ///     Every word needs a <see cref="Usage"/>.
    ///     If the <see cref="language"/> doesn't exist a new one will be added.
    ///     If the information contains the word and the usage it can be added/changed every time.
    /// </summary>
    /// <param name="information">the word or the <see cref="Usage"/> or <see cref="WordData"/></param>
    /// <param name="language">the language of the added <see cref="information"/></param>
    /// <param name="idx">the index of the translation in the <see cref="AllTranslations"/> list</param>
    /// <param name="changeUsage">if true, the information should be the <see cref="Usage"/></param>
    /// <returns>if the changing/adding of information worked</returns>
    public bool ChangeInformations(string information, string language, int idx = -1, bool changeUsage = false)
    {
        information = information.ToLowerInvariant();
        language = language.ToLowerInvariant();
        if (IsInValid(information, changeUsage, out Usage usage, out WordData? newWordData) || newWordData == null)
        {
            return false;
        }
        if (idx < 0 || idx >= AllTranslations.Count)
        {   
            idx = AllTranslations.Count;
            AllTranslations.Add(new MyDictionary<string, WordData>());
        }
        TryAddLanguage(language);

        AllWords.Remove(AllTranslations[idx][language]?.Word ?? string.Empty);
        ChangeTranslation(AllTranslations[idx], language, newWordData, information, usage, changeUsage);
        AllWords.Add(AllTranslations[idx][language]!.Word, AllTranslations[idx]);

        return true;
    }

    private bool IsInValid(string information, bool changeUsage, out Usage usage, out WordData? wordData)
    {
        wordData = null;
        usage = Usage.never;
        return !TryGetWordData(information, out wordData)
            && (string.IsNullOrWhiteSpace(information)
            || (changeUsage && !Enum.TryParse(information, out usage))
            || (!changeUsage && AllWords.ContainsKey(information)));
    }

    /// <summary>
    ///     Adds a language to <see cref="Languages"/>, if it doesn't already exist.
    /// </summary>
    /// <param name="language">the language that will be added</param>
    /// <returns>if adding the language worked</returns>
    public bool TryAddLanguage(string language)
    {
        if (Languages.Contains(language.ToLowerInvariant()))
        {
            return false;
        }
        Languages.Add(language.ToLowerInvariant());
        return true;
    }

    /// <summary>
    ///     Writes the whole information of the dictionary to the console.
    /// </summary>
    /// <returns>the information of the dictionary</returns>
    public override string ToString()
    {
        if(Languages.Count== 0)
        {
            return string.Empty;
        }
        return "";
    }

    /// <summary>
    ///     Gets a list of all word pairs in the languages of <see cref="language1"/> and <see cref="language2"/>,
    ///     but only if a version of the word in both languages exists.
    /// </summary>
    /// <param name="language1">the first language</param>
    /// <param name="language2">the second language</param>
    /// <returns>a list of all word pairs in the languages of <see cref="language1"/> and <see cref="language2"/></returns>
    public List<string[]>? GetDictionary(string language1, string language2)
    {
  
       List<string[]> wp = new List<string[]>();

        foreach (var translation in AllTranslations)
        {
            if (translation[language1] != null
                && translation[language2] != null)
            {
                wp.Add(
                    new[] 
                    { 
                        translation[language1]!.Word, 
                        translation[language2]!.Word 
                    });
            }
        }

        return (wp.Count <= 0) ? null
            : wp;
    }

    /// <summary>
    ///     Tests if the <see cref="DictionaryManager"/> contains the <see cref="word"/>.
    /// </summary>
    /// <param name="word">the word to test if it is contained</param>
    /// <returns>if the <see cref="DictionaryManager"/> contains the <see cref="word"/></returns>
    public bool Contains(string word)
    {
        return AllWords.ContainsKey(word.ToLowerInvariant());
    }

    /// <summary>
    ///     Gets the saved information about a <see cref="word"/>.
    /// </summary>
    /// <param name="word">the word to get the information from</param>
    /// <returns>a message with the information about the <see cref="word"/>, if the word doesn't exist null gets returned</returns>
    public string? GetWordInformation(string word)
    {
        return this.ToString();
    }

    /// <summary>
    /// <summary>
    ///     Generates a the <see cref="AllWords"/> dictionary which contains a key value pair for every single
    ///     word in the <see cref="AllTranslations"/>.
    ///     The keys are the single words.
    ///     The values are the Dictionaries with the translations of the word and the word itself. 
    /// </summary>
    private void GenerateAllWordsFromTranslations()
    {
        foreach (var translations in AllTranslations)
        {
            List<WordData> translatedWordData = translations.GetValues();
            foreach (var data in translatedWordData)
            {
                if (!AllWords.ContainsKey(data.Word.ToLowerInvariant()))
                {
                    AllWords.Add(data.Word.ToLowerInvariant(), translations);
                }
            }
        }
    }

    /// <summary>
    ///     Reads the words and the data of the word in the csv file and saves them in the field <see cref="AllTranslations"/>.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>if everything worked properly</returns>
    private bool InitializeWordsFromFile(string path)
    {
        bool inited = true;
        string[] lines = File.ReadAllLines(path);

        if (!File.Exists(path)
            || lines.Length <= 0)
        {
            return false;
        }

        string[] languages = lines[0].ToLowerInvariant().Split(';');
        foreach (string language in languages)
        {
            Languages.Add(language.ToLowerInvariant());
        }
        for (int i = 1; i < lines.Length; i++)
        {
            string[] words = lines[i].ToLowerInvariant().Split(';');
            if (words.Length >= Languages.Count)
            {
                AddTolist(words);
                inited = true;
            }
            else
            {
                inited = false;
            }
        }
        return inited;
    }

    private void AddTolist(string[] words)
    {
        MyDictionary<string, WordData> translations = new MyDictionary<string, WordData>();
        for (int j = 0; j < words.Length; j++)
        {
            if (TryGetWordData(words[j], out WordData? wordData))
            {
                translations.Add(Languages[j], wordData!);
            }
    
        }
        if (translations.Count > 0)
        {
            AllTranslations.Add(translations);
        }
    }


    /// <summary>
    ///     Tries to get the <see cref="Usage"/> and Word from the <see cref="wordDataString"/>.
    /// </summary>
    /// <param name="wordDataString">the data string</param>
    /// <param name="wordData">the output if getting the data worked</param>
    /// <returns>if getting the data worked</returns>
    private bool TryGetWordData(string wordDataString, out WordData? wordData)
    {
        string[] props = wordDataString.Split('-');
        if (!string.IsNullOrWhiteSpace(wordDataString)
            && !string.IsNullOrWhiteSpace(props[0])
            && Enum.TryParse(props[1], out Usage usage)
            && props.Length >= 2)
        {
            wordData = new WordData(props[0], usage);
            return true;
        }

        wordData = null;
        return false;
    }

    /// <summary>
    ///     Adds the translation depending on the existing data.
    /// </summary>
    /// <param name="translations">the list of all translations</param>
    /// <param name="language">the of the information</param>
    /// <param name="newWordData">the <see cref="WordData"/></param>
    /// <param name="word">the word</param>
    /// <param name="usage">the usage</param>
    /// <param name="changeUsage">if true the usage should be changed</param>
    private void ChangeTranslation(MyDictionary<string, WordData> translations, string language, WordData? newWordData, string word, Usage usage, bool changeUsage)
    {
        WordData? oldWordData = translations[language];
        translations.Remove(language);

        if (newWordData != null)
        {
            translations.Add(language, newWordData);
        }
        else if (changeUsage)
        {
            translations.Add(language, new WordData(oldWordData!.Word, usage));
        }
        else
        {
            translations.Add(language, new WordData(word, oldWordData!.Usage));
        }
    }
}