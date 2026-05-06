using Quiz.Domain;
using Quiz.Domain.Interfaces;
using Quiz.Persistence;
using System.IO;

namespace Quiz.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.;Database=QuizApp;Trusted_Connection=True;";

            IQuizRepository repository = new QuizRepository(connectionString);
            DomainManager manager = new DomainManager(repository);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== Quiz App ===");
                Console.WriteLine("1. Import txt bestand");
                Console.WriteLine("2. Afsluiten");
                Console.Write("Keuze: ");

                string keuze = Console.ReadLine();

                switch (keuze)
                {
                    case "1":
                        ImportFile(manager);
                        break;
                    case "2":
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
            topic.Id = manager.AddTopic(topic);

            TxtParser parser = new TxtParser();
            List<Question> questions = parser.Parse(filePath, topic);

            manager.ImportFromFile(questions);

            Console.WriteLine($"{questions.Count} vragen geïmporteerd!");
        }
    }
}