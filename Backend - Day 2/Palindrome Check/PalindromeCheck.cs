public class PalindromeCheck
{
    public static void Main()
    {
        // test the function
        Console.WriteLine("Enter a string to check if it's palindrome: ");
        string test = Console.ReadLine();

        if(isPalindrome(test))
            Console.WriteLine($"{test} is palindrome");
        else 
            Console.WriteLine($"{test} is not palindrome");
    }

    static bool isPalindrome(string str)
    {
        string cleanedStr = "";

        foreach (char ch in str)
        {
            if (char.IsLetterOrDigit(ch)) cleanedStr += char.ToLower(ch);
        }

        int len = cleanedStr.Length;
        for(int i = 0; i < len/2; i++)
        {
            if (cleanedStr[i] != cleanedStr[len - 1 - i])
            {
                return false;
            }
        }
        return true;
    }
}