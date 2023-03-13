using Dictionary;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine("|           Dictionary          |");
        Console.WriteLine("---------------------------------");
        const string Path = @"Data\words.csv";
        var input = string.Empty;
        var dictionaryManger = new DictionaryManager(Path);
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        /* DictionaryManager.ChangeInformations tests, could be used as unit test data
        Console.WriteLine(dictionaryManger.ToString());
        dictionaryManger.ChangeInformations("katz", "english", 0);
        dictionaryManger.ChangeInformations("gak-often", "lag", 0);
        dictionaryManger.ChangeInformations("gak-often", "lag", -1);
        dictionaryManger.ChangeInformations("never", "german", 0, true);
        dictionaryManger.ChangeInformations("never", "german", -1);
        dictionaryManger.ChangeInformations("dsads", "german", 0, true);
        dictionaryManger.ChangeInformations("dsads", "german", -1);
        dictionaryManger.ChangeInformations(" ", "german", 0);
        Console.WriteLine(dictionaryManger.ToString());
        */

        while (input.ToLowerInvariant() != "exit")
        {
            Console.WriteLine("(1) Play VocabularyTrainer");
            Console.WriteLine("(2) Search for word");
            Console.WriteLine("(3) Get Info to Word");
            Console.WriteLine("(4) Get all words in the Dictionary");
            Console.WriteLine("(5) Change/Add information");
            Console.WriteLine("Type 'Exit' to stop");

            input = string.Empty;
            var inputInt = 0;
            do
            {
                Console.WriteLine("---------------------------------");
                input = Console.ReadLine();
                Console.WriteLine("---------------------------------");
            } while (!int.TryParse(input, out inputInt) && input!.ToLowerInvariant() != "exit");

            switch (inputInt)
            {
                case 1:
                    Console.Write("Enter the language you want to test yourself in[English, German, Spanish, Russian]: ");
                    var lang1 = Console.ReadLine();
                    Console.Write("Enter the language you want to translate into[English, German, Spanish, Russian]: ");
                    var lang2 = Console.ReadLine();

                    var VocabularyTrainer = new VocabularyTrainer(lang1!, lang2!, dictionaryManger);
                    VocabularyTrainer.Run();

                    Console.Clear();
                    break;
                case 2:
                    Console.Write("Enter a word you want to know if it exists: ");

                    var word = Console.ReadLine();
                    var output = dictionaryManger.Contains(word!.ToLowerInvariant()) ? "The word exists" : "The word doesn't exist";
                    Console.WriteLine(output);

                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 3:
                    Console.Write($"Enter a word you want to get info of: ");
                    var wordInfo = Console.ReadLine();
                    var info = dictionaryManger.GetWordInformation(wordInfo!.ToLowerInvariant());

                    if (info != null)
                    {
                        Console.WriteLine(dictionaryManger.GetWordInformation(wordInfo.ToLowerInvariant()));
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Could not find word!");
                        Console.ResetColor();
                    }

                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 4:
                    Console.WriteLine(dictionaryManger.ToString());
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case 5:
                    Console.WriteLine(dictionaryManger.ToString());
                    int index = -1;
                    do
                    {
                        Console.Write("Enter the index of the word or exit (enter a non-existing index if you want to add translations): ");
                    } while (!int.TryParse(Console.ReadLine(), out index));
                    
                    Console.Write("Enter the language of the word or exit: ");
                    var language = Console.ReadLine();
                    
                    bool changeUsage = false;
                    do
                    {
                        Console.Write("Enter if the new information will be usage (true) or a word or both (false): ");
                    } while (!bool.TryParse(Console.ReadLine(), out changeUsage));
                    
                    Console.Write("Enter the new information (if information is word and usage the format is WORD-USAGE) you want to change or add: ");
                    var information = Console.ReadLine();

                    if (dictionaryManger.ChangeInformations(information!, language!, index - 1, changeUsage))
                    {
                        dictionaryManger.SaveDictionary(Path);   
                    }
                    else
                    {
                        Console.WriteLine("Changing/Adding didn't work.");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}