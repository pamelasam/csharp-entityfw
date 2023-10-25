using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp
{
    public class UserInput
    {
        [Key]
        public long UserInputId { get; set; }
        public int RoundId { get; set; }
        public string? RawAnswer;
        public string? FormattedAnswer;
        public bool IsValidAnswer { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserInput()
        {
            RawAnswer = GameEngine.GetRawAnswerInput();
            FormattedAnswer = GameEngine.GetNumberOnlyInString(RawAnswer);
            IsValidAnswer = GameEngine.ValidateAnswerInput(GameEngine.ConvertStringToIntArray(FormattedAnswer), GameEngine.NumDigit);
        }
    }
}
