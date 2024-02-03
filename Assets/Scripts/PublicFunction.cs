using UnityEngine;

namespace Joshua.Publlic
{
    public static class PublicFunction
    {
        /// <summary>
        /// 產生隨機小寫字母字串
        /// </summary>
        /// <param name="numberOfLetters">字串長度</param>
        /// <returns>字串</returns>
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

}
