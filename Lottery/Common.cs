using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lottery
{
    public struct PrizeNumber
    {
        public int FirstRank;
        public int SecondRank;
        public int ThirdRank;
        public int Total;
        public PrizeNumber(int firstRank, int secondRank, int thirdRank)
        {
            this.FirstRank = firstRank;
            this.SecondRank = secondRank;
            this.ThirdRank = thirdRank;
            this.Total = firstRank + secondRank + thirdRank;
        }
    }
    public static class Common
    {


        public static PrizeNumber ThePrizeNumber;
        public static string Title;
        public static string[] NameList;
        public static string[] AvailableName;

        public static void Init()
        {
            string config = "config.txt";
            string patternTitle = "^Title:\\s\\S+";
            string PatternFirst = "^FirstPrize:\\s\\d{1,2}";
            string PatternSecond = "^SecondPrize:\\s\\d{1,2}";
            string PatternThird = "^ThirdPrize:\\s\\d{1,2}";
            if (!File.Exists(config))
                throw new FileNotFoundException($"文件{config}未找到!");

            Common.Title = "";
            string first = "", second = "", third = "";
            foreach (string line in File.ReadAllLines(config))
            {
                if (Regex.IsMatch(line, patternTitle))
                    Common.Title = Regex.Match(line, patternTitle).Value.Replace("Title: ", "");
                if (Regex.IsMatch(line, PatternFirst))
                    first = Regex.Match(line, "\\d{1,2}").Value;
                if (Regex.IsMatch(line, PatternSecond))
                    second = Regex.Match(line, "\\d{1,2}").Value;
                if (Regex.IsMatch(line, PatternThird))
                    third = Regex.Match(line, "\\d{1,2}").Value;
            }
            if (Common.Title.Length <= 1)
                throw new FormatException("参数Tittle格式有误,请检查!");
            if (first == "")
                throw new FormatException("参数FirstPrize格式有误,请检查!");
            if (second == "")
                throw new FormatException("参数SecondPrize格式有误,请检查!");
            if (third == "")
                throw new FormatException("参数ThirdPrize格式有误,请检查!");

            Common.ThePrizeNumber = new PrizeNumber(int.Parse(first), int.Parse(second), int.Parse(third));

            // Read the NameList file
            string fileName = "NameList.txt";
            if (!File.Exists(fileName))
                throw new FileNotFoundException($"文件{fileName}未找到!");

            string[] replace = new string[] { "\n", "\t", "\b", " ", "."};
            string nameText = File.ReadAllText(fileName);
            foreach(var c in replace)
                nameText = nameText.Replace(c, "");
            Common.NameList = nameText.Split(',');

            Common.AvailableName = Common.NameList;
        }
    }
}
