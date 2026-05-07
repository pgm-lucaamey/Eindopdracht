using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain.Exceptions
{
    public class InvalidQuestionException : Exception
    {
        public InvalidQuestionException(string message) : base(message)
        {

        }

    }
}
