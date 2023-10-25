using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class GameManager
    {
        Game Game { get; set; }
        int RoundRunningNo { get; set; } = 1;
        public GameManager() 
        { 
            Game = GameEngine.CreateNewGame();
        }

    }
    public class GameEngine
    {
        public static int NumDigit = 4;
        public static Round CreateNewRound(int roundRunningNo) {
            using (GameContext context = new GameContext())
            {
                Round round = new Round(roundRunningNo);
                context.Rounds.Add(round);
                context.SaveChanges();
                return round;
            }
        }
        public static Game CreateNewGame() 
        {
            using (GameContext context = new GameContext())
            {
                Game game = new Game();
                context.Games.Add(game);
                context.SaveChanges();
                return game;
            }
        }
        public static Game EndGame(Game game)
        {
            using (GameContext context = new GameContext())
            {
                game.IsEnd = true;
                game.EndDt = DateTime.Now;
                game.UpdateDt = DateTime.Now;
                context.Update(game);
                context.SaveChanges();
                return game;
            }
        }
        public static string GetRawAnswerInput()
        {
            Console.WriteLine("Guess the Secret Code: ");
            string input_answer = Console.ReadLine();
            return input_answer;
        }
        public static int[] GenerateSecretCode(int numDigit)
        {
            int[] secretCode = new int[numDigit];
            Random random = new Random();
            for (int i = 0; i < numDigit;)
            {
                int tempRandom = random.Next(0, 10);
                if (secretCode.Contains(tempRandom) == false)
                {
                    secretCode[i] = tempRandom;
                    i++;
                }
                else
                {
                    //Console.WriteLine("Duplicated random digit");
                    continue;
                }
            }
            return secretCode;
        }
        public static string ConvertArrayToString(int[] array)
        {
            return string.Join("", array);
        }
        public static string GetNumberOnlyInString(string _string) 
        {
            string strInput = "";
            string pattern = @"\d";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(_string);
            if (matches.Count > 0 )
            {
                foreach(Match match in matches)
                {
                    strInput = string.Concat(strInput, match.Value);
                }
            }
            return strInput;
        }
        public static int[] ConvertStringToIntArray(string _string)
        {
            int[] array = new int[_string.Length];
            for (int i = 0; i < _string.Length; i++)
            {
                array[i] = _string.ElementAt(i) - '0'; //convert char to int
            }
            return array;
        }
        public static bool ValidateAnswerInput(int[] answer, int numDigit) 
        {
            return answer.Length == numDigit;
        }
        public static void GetClue(int[] secretCode, int[] answer, ref int roundHit, ref int roundMiss)
        {
            int hit = 0, miss = 0;
            for (int i = 0; i < answer.Length; i++)
            {
                int tempAnswerDigit = answer[i];

                if (secretCode[i] == tempAnswerDigit)
                {
                    hit++;
                    continue;
                }
                if (secretCode.Contains(tempAnswerDigit))
                {
                    miss++;
                }
            }
            roundHit = hit;
            roundMiss = miss;
        }
        public static bool IsGameEnd(int hit)
        {
            return hit == NumDigit;
        }
    }
}
