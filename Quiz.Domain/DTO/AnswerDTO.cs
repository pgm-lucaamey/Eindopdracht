using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public char Label { get; set; }
        public bool IsCorrect { get; set; }
    }
}
