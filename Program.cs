using System.Configuration;
using System.Collections.Specialized;
using Microsoft.IdentityModel.Protocols;
using System.Collections.Generic;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string userInput = GameEngine.GetRawAnswerInput();
            int[] secretCode = GameEngine.GenerateSecretCode(4);
            string userInput = "1207";
            string formattedInput = GameEngine.GetNumberOnlyInString(userInput);
            int[] arr_input = GameEngine.ConvertStringToIntArray(formattedInput);
            bool isvalid = GameEngine.ValidateAnswerInput(arr_input, 4);
            int h = 0;
            int m = 0;
            GameEngine.GetClue(secretCode, arr_input, ref h, ref m);
            //using (GameContext gameContext = new GameContext())
            //{                
                //IEnumerable<Game> gameQuery = gameContext.Games.Where(g => g.GameId == 1);
                //foreach (Game game in gameQuery)
                //{
                //    game.IsEnd = true;
                //    game.EndDt = DateTime.Now;
                //    context.Update<Game>(game);
                //}

                //Game game = gameContext.Games.Find(11);
                //if (game != null)
                //{
                //    GameEngine.EndGame(game);
                //}
                //gameContext.SaveChanges();
            //}
        }
    }
}