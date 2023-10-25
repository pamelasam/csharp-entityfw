using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Round
    {
        [Key]
        public int RoundId { get; set; }
        public int GameId { get; set; }
        public long UserInputId { get; set; }
        public Game Game { get; set; }
        public int RoundNo { get; set; }
        public int Hit { get; set; } = 0;
        public int Miss { get; set; } = 0;
        public DateTime StartDt { get; set; } = DateTime.Now;
        public DateTime EndDt { get; set; }
        public Round(int roundNo)
        {
            RoundNo = roundNo;
        }
    }
}
