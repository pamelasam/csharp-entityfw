using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public string GameStrId { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmssf");
        public string SecretCode { get; private set; }
        public int NumDigit { get; private set; }
        public ICollection<Round>? Round { get; set; } = new List<Round>();
        public bool IsEnd { get; set; } = false;
        public DateTime StartDt { get; set; } = DateTime.Now;
        public DateTime? EndDt { get; set; }
        public DateTime? UpdateDt { get; set; }
        public Game() 
        {
            NumDigit = GameEngine.NumDigit;
            int[] secretCode = GameEngine.GenerateSecretCode(NumDigit);
            SecretCode = GameEngine.ConvertArrayToString(secretCode);
        }
    }
}
