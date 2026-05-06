using System;
using System.Collections.Generic;
using System.Text;

namespace Quiz.Domain
{
    public class Test
    {
        private string _name;
        private int _numberOfQuestions;
        private int _id;
        private int _topicId;
        private DateTime _createdAt; 

        public string Name
        {
            get { return _name; }
        }
        public int NumberOfQuestions
        {
            get { return _numberOfQuestions; }
        }
        public int Id
        {
            get { return _id; }
        }
        public int TopicId
        {
            get { return _topicId; }
        }
        public DateTime CreatedAt {  get { return _createdAt; } }

        public Test(string name, int numberofQuestions, int TopicId)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name cannot be empty or null");
            _name = name;
            _numberOfQuestions = numberofQuestions;
            _topicId = TopicId;
            _createdAt = DateTime.Now;

        }
    }
}
