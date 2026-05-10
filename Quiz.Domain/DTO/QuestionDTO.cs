using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerDTO> Answers { get; set; }

        public QuestionDTO()
        {
            Answers = new List<AnswerDTO>();
        }
    }
}
