using Quiz.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain
{
    public class Topic
    {
        private string _name;
        private int _id;

        public string Name { get { return _name; } }
      
        public int Id { get { return _id; } set { _id = value; } }
        

        public Topic(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidQuestionException("Topic name cannot be null or empty.");
            }
            _name = name; 
        }
    }
}
