using Quiz.Domain;
using Quiz.Domain.Interfaces;
using Quiz.Persistence;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Quiz.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source =.; Initial Catalog = Quiz; Integrated Security = True; Encrypt = True; Trust Server Certificate = True;";

            IQuizRepository repository = new QuizRepository(connectionString);
            DomainManager manager = new DomainManager(repository);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== Quiz App ===");
                Console.WriteLine("1. Import txt bestand");
                Console.WriteLine("2. Stel een quiz samen");
                Console.WriteLine("3. Voeg een vraag toe");
                Console.WriteLine("4. Schakel een vraag uit");
                Console.WriteLine("5. Voer een test uit");
                Console.WriteLine("6. Afsluiten");
                Console.Write("Keuze: ");

                string keuze = Console.ReadLine();

                switch (keuze)
                {
                    case "1":
                        ImportFile(manager);
                        break;
                    case "2":
                        CreateTest(manager);
                        break;
                    case "3":
                        AddQuestion(manager);
                        break;
                    case "4":
                        DisableQuestion(manager);
                        break;
                    case "5":
                        TakeTest(manager);
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ongeldige keuze.");
                        break;
                }
            }
        }

        static void ImportFile(DomainManager manager)
        {
            Console.Write("Geef het pad naar het txt bestand: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Bestand niet gevonden.");
                return;
            }

            string topicName = Path.GetFileNameWithoutExtension(filePath);
            Topic topic = new Topic(topicName);

            int topicId = manager.AddTopic(topic);
            if (topicId == -1)
                return;

            TxtParser parser = new TxtParser();
            List<Question> questions = parser.Parse(filePath, topic);
            manager.ImportFromFile(questions);

            Console.WriteLine($"{questions.Count} vragen geïmporteerd!");
        }

        static void CreateTest(DomainManager manager)
        {
            Console.Write("Naam van de test: ");
            string name = Console.ReadLine();

            Console.Write("Aantal vragen: ");
            int count = int.Parse(Console.ReadLine());

         
            foreach (Topic topic in manager.GetAllTopics())
            {
                Console.WriteLine($"{topic.Id}. {topic.Name}");
            }

            Console.Write("Kies een topic (Id): ");
            int topicId = int.Parse(Console.ReadLine());

            manager.CreateTest(name, count, topicId);
            Console.WriteLine("Test aangemaakt!");
        }
        static void AddQuestion(DomainManager manager)
        {
         
            foreach (Topic topic in manager.GetAllTopics())
            {
                Console.WriteLine($"{topic.Id}. {topic.Name}");
            }
            Console.Write("Kies een topic (Id): ");
            int topicId = int.Parse(Console.ReadLine());

            Console.Write("Vraag: ");
            string questionText = Console.ReadLine();

            Console.Write("Antwoord A: ");
            string a = Console.ReadLine();
            Console.Write("Antwoord B: ");
            string b = Console.ReadLine();
            Console.Write("Antwoord C: ");
            string c = Console.ReadLine();
            Console.Write("Antwoord D: ");
            string d = Console.ReadLine();

            Console.Write("Juist antwoord (A/B/C/D): ");
            char correct = Console.ReadLine().ToUpper()[0];

            manager.AddQuestion(questionText, topicId, a, b, c, d, correct);
            Console.WriteLine("Vraag toegevoegd!");
        }
        static void DisableQuestion(DomainManager manager)
        {
            foreach (Topic topic in manager.GetAllTopics())
            {
                Console.WriteLine($"{topic.Id}. {topic.Name}");
            }
            Console.Write("Kies een topic (Id): ");
            int topicId = int.Parse(Console.ReadLine());

            foreach (Question question in manager.GetQuestionsByTopic(topicId))
            {
                Console.WriteLine($"{question.Id}. {question.QuestionText}");
            }
            Console.Write("Kies een vraag (Id): ");
            int questionId = int.Parse(Console.ReadLine());

            manager.DisableQuestion(questionId);
            Console.WriteLine("Vraag uitgeschakeld!");
        }
        static void TakeTest(DomainManager manager)
        {
            foreach (Test test in manager.GetAllTests())
            {
                Console.WriteLine($"{test.Id}. {test.Name}");
            }
            Console.Write("Kies een test (Id): ");
            int testId = int.Parse(Console.ReadLine());

            List<Question> questions = manager.GetQuestionsForTest(testId);
            int score = 0;

            foreach (Question question in questions)
            {
                Console.WriteLine($"\n{question.QuestionText}");
                foreach (Answer answer in question.Answers)
                {
                    Console.WriteLine($"{answer.Label}. {answer.AnswerText}");
                }

                Console.Write("Jouw antwoord (A/B/C/D): ");
                char userAnswer = Console.ReadLine().ToUpper()[0];

                Answer correct = question.GetCorrectAnswer();
                if (userAnswer == correct.Label)
                {
                    Console.WriteLine("Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"Fout! Juist antwoord: {correct.Label}. {correct.AnswerText}");
                }
            }

            Console.WriteLine($"\nScore: {score}/{questions.Count}");
            manager.SaveResult(testId, score);
        }

    }
}