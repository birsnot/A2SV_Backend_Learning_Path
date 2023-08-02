public class WordFrequencyCount
{
    public static void Main()
    {
        // test the function
        Console.WriteLine("Enter a string to count its words: ");
        string test = Console.ReadLine();
        Dictionary<string, int> testCount = wordCounter(test);

        // display the test output
        Console.WriteLine();
        Console.WriteLine($"{"WORD",-20}COUNT");
        foreach(KeyValuePair<string, int> kvp in testCount)
        {
            Console.WriteLine($" {kvp.Key, -20}{kvp.Value}");
        }
    }

    static Dictionary<string, int> wordCounter(string str)
    {
        Dictionary<string, int> wordCount = new Dictionary<string, int>();
        string word = "";

        foreach(char ch in str)
        {
            if (char.IsLetterOrDigit(ch)) word += char.ToLower(ch);
            else if(word.Length > 0)
            {
                if (wordCount.ContainsKey(word)) wordCount[word]++;
                else wordCount[word] = 1;
                word = "";
            }
        }

        // this is to handle the last word
        if (word.Length > 0)
        {
            if (wordCount.ContainsKey(word)) wordCount[word]++;
            else wordCount[word] = 1;
        }

        return wordCount;
    }
}