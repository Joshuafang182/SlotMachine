using UnityEngine;

public static class PublicFunction
{
    public static string RandomLetters(int numberOfLetters)
    {
        string res = string.Empty;
        for (int i = 0; i < numberOfLetters; i++)
        {
            char randomLetter = (char)('a' + Random.Range(0, 17));
            res += randomLetter.ToString();
        }
        return res;
    }
}
