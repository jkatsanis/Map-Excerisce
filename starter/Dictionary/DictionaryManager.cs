using System.Runtime.InteropServices.JavaScript;

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
        //TODO
    }

    /// <summary>
    ///     Constructs a <see cref="DictionaryManager"/> which contains the information from the csv file.
    /// </summary>
    /// <param name="wordsDataPath">the path to the csv file with the information</param>
    public DictionaryManager(string wordsDataPath) : this()
    {
        //TODO
    }

    public bool SaveDictionary(string path)
    {
        //TODO
        return false;
    }

    /// <summary>
    ///     Changes or add the information of a word.
    ///     Every word needs a <see cref="Usage"/>.
    ///     If the <see cref="language"/> doesn't exist a new one will be added.
    ///     If the information contains the word and the usage it can be added/changed every time.
    /// </summary>
    /// <param name="information">the word or the <see cref="Usage"/> or <see cref="WordData"/></param>
    /// <param name="language">the language of the added <see cref="information"/></param>
    /// <param name="index">the index of the translation in the <see cref="AllTranslations"/> list</param>
    /// <param name="changeUsage">if true, the information should be the <see cref="Usage"/></param>
    /// <returns>if the changing/adding of information worked</returns>
    public bool ChangeInformations(string information, string language, int index = -1, bool changeUsage = false)
    {
        //TODO
        return false;
    }

    /// <summary>
    ///     Adds a language to <see cref="Languages"/>, if it doesn't already exist.
    /// </summary>
    /// <param name="language">the language that will be added</param>
    /// <returns>if adding the language worked</returns>
    public bool TryAddLanguage(string language)
    {
        //TODO
        return false;
    }

    /// <summary>
    ///     Writes the whole information of the dictionary to the console.
    /// </summary>
    /// <returns>the information of the dictionary</returns>
    public override string ToString()
    {
        //TODO
        return $"";
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
        //TODO
        return null;
    }

    /// <summary>
    ///     Tests if the <see cref="DictionaryManager"/> contains the <see cref="word"/>.
    /// </summary>
    /// <param name="word">the word to test if it is contained</param>
    /// <returns>if the <see cref="DictionaryManager"/> contains the <see cref="word"/></returns>
    public bool Contains(string word)
    {
        //TODO
        return false;
    }

    /// <summary>
    ///     Gets the saved information about a <see cref="word"/>.
    /// </summary>
    /// <param name="word">the word to get the information from</param>
    /// <returns>a message with the information about the <see cref="word"/>, if the word doesn't exist null gets returned</returns>
    public string? GetWordInformation(string word)
    {
        //TODO
        return "";
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
        //TODO
        return false;
    }

    /// <summary>
    ///     Reads the languages from the <see cref="line"/> and saves it in the field <see cref="Languages"/>.
    /// </summary>
    /// <param name="line">the string with the information</param>
    private void InitializeLanguages(string line)
    {
        //TODO
    }

    /// <summary>
    ///     Tries to get the <see cref="Usage"/> and Word from the <see cref="wordDataString"/>.
    /// </summary>
    /// <param name="wordDataString">the data string</param>
    /// <param name="wordData">the output if getting the data worked</param>
    /// <returns>if getting the data worked</returns>
    private bool TryGetWordData(string wordDataString, out WordData? wordData)
    {
        //TODO
        wordData = null;
        return false;
    }

    /// <summary>
    ///     Converts the string to lowercase and the first char to uppercase.
    /// </summary>
    /// <param name="word">the string that will be converted</param>
    /// <returns>the converted word</returns>
    private string CapitaliseString(string? word)
    {
        //TODO
        return "";
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
        //TODO
    }
}