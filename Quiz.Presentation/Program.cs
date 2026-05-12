using Quiz.Domain;
using Quiz.Domain.DTO;
using Quiz.Domain.Interfaces;
using Quiz.Persistence;
using Spectre.Console;
using System.IO;

namespace Quiz.Presentation
{
    internal class Program
    {
        static DomainManager manager;

        static void Main(string[] args)
        {
            string connectionString = "Data Source=.;Initial Catalog=Quiz;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            IQuizRepository repository = new QuizRepository(connectionString);
            manager = new DomainManager(repository);

            bool running = true;
            while (running)
            {
                ShowMenu();
                string keuze = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                        .AddChoices(
                            ">> Import txt bestand",
                            ">> Stel een quiz samen",
                            ">> Voeg een vraag toe",
                            ">> Schakel een vraag uit",
                            ">> Voer een test uit",
                            ">> Afsluiten"
                             ));

                AnsiConsole.Clear();

                switch (keuze)
                {
                    case ">> Import txt bestand":
                        ImportFile();
                        break;
                    case ">> Stel een quiz samen":
                        CreateTest();
                        break;
                    case ">> Voeg een vraag toe":
                        AddQuestion();
                        break;
                    case ">> Schakel een vraag uit":
                        DisableQuestion();
                        break;
                    case ">> Voer een test uit":
                        TakeTest();
                        break;
                    case ">> Afsluiten":
                        running = false;
                        break;
                }
            }

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[blue]Tot ziens![/]").Centered());
        }
        static void ShowMenu()
        {
            AnsiConsole.Clear();

            AnsiConsole.Write(new FigletText("Quiz App")
                .Centered()
                .Color(Color.Blue));

            AnsiConsole.Write(new Rule("[bold blue]Hoofdmenu[/]").Centered());
            AnsiConsole.WriteLine();

            var stats = new Table()
                .BorderColor(Color.Grey)
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[grey]Info[/]").Centered())
                .AddColumn(new TableColumn("[grey]Waarde[/]").Centered());

            var topics = manager.GetAllTopics().ToList();
            var tests = manager.GetAllTests().ToList();

            stats.AddRow("[grey]Geladen topics[/]", $"[white]{topics.Count}[/]");
            stats.AddRow("[grey]Beschikbare tests[/]", $"[white]{tests.Count}[/]");

            AnsiConsole.Write(stats);
            AnsiConsole.WriteLine();

            AnsiConsole.Write(new Rule("[grey]↑ ↓ navigeren  |  Enter kiezen[/]")
                .RuleStyle("grey")
                .Centered());
            AnsiConsole.WriteLine();
        }

        static void ShowSuccess(string message)
        {
            AnsiConsole.Write(new Panel($"[green]{message}[/]")
                .BorderColor(Color.Green));
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Druk op Enter om terug te gaan...[/]");
            Console.ReadLine();
        }

        static void ShowError(string message)
        {
            AnsiConsole.Write(new Panel($"[red]{message}[/]")
                .BorderColor(Color.Red));
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Druk op Enter om terug te gaan...[/]");
            Console.ReadLine();
        }

        static int ReadInt(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<int>($"[blue]{prompt}[/]")
                    .ValidationErrorMessage("[red]Ongeldige invoer, geef een getal in.[/]")
                    .Validate(n => n > 0
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]Getal moet groter zijn dan 0.[/]")));
        }

        static void ImportFile()
        {
            AnsiConsole.Write(new Rule("[blue]Import txt bestand[/]").Centered());
            AnsiConsole.WriteLine();

            string filePath = AnsiConsole.Prompt(
                new TextPrompt<string>("[blue]Geef het pad naar het txt bestand:[/]"));

            if (!File.Exists(filePath))
            {
                ShowError("Bestand niet gevonden.");
                return;
            }

            string topicName = Path.GetFileNameWithoutExtension(filePath);

            if (manager.TopicExists(topicName))
            {
                ShowError($"Topic '{topicName}' bestaat al.");
                return;
            }

            Topic topic = new Topic(topicName);
            TxtParser parser = new TxtParser();

            List<Question> questions = null;
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse("blue"))
                .Start("Bestand inlezen...", ctx =>
                {
                    questions = parser.Parse(filePath, topic);
                    ctx.Status("Opslaan in database...");
                    manager.ImportFromFile(topic, questions);
                });

            ShowSuccess($"{questions.Count} vragen succesvol geïmporteerd!");
        }

        static void CreateTest()
        {
            AnsiConsole.Write(new Rule("[blue]Stel een quiz samen[/]").Centered());
            AnsiConsole.WriteLine();

            string name = AnsiConsole.Prompt(
                new TextPrompt<string>("[blue]Naam van de test:[/]"));

            int count = ReadInt("Aantal vragen: ");

            var topics = manager.GetAllTopics().ToList();
            var topicKeuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Kies een topic:[/]")
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                    .AddChoices(topics.Select(t => $"{t.Id}. {t.Name}")));

            int topicId = int.Parse(topicKeuze.Split('.')[0]);
            manager.CreateTest(name, count, topicId);

            ShowSuccess($"Test '{name}' succesvol aangemaakt!");
        }

        static void AddQuestion()
        {
            AnsiConsole.Write(new Rule("[blue]Voeg een vraag toe[/]").Centered());
            AnsiConsole.WriteLine();

            var topics = manager.GetAllTopics().ToList();
            var topicKeuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Kies een topic:[/]")
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                    .AddChoices(topics.Select(t => $"{t.Id}. {t.Name}")));

            int topicId = int.Parse(topicKeuze.Split('.')[0]);

            string questionText = AnsiConsole.Prompt(
                new TextPrompt<string>("[blue]Vraag:[/]"));

            string a = AnsiConsole.Prompt(new TextPrompt<string>("[blue]Antwoord A:[/]"));
            string b = AnsiConsole.Prompt(new TextPrompt<string>("[blue]Antwoord B:[/]"));
            string c = AnsiConsole.Prompt(new TextPrompt<string>("[blue]Antwoord C:[/]"));
            string d = AnsiConsole.Prompt(new TextPrompt<string>("[blue]Antwoord D:[/]"));

            string correct = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Juist antwoord:[/]")
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                    .AddChoices("A", "B", "C", "D"));

            manager.AddQuestion(questionText, topicId, a, b, c, d, correct[0]);
            ShowSuccess("Vraag succesvol toegevoegd!");
        }

        static void DisableQuestion()
        {
            AnsiConsole.Write(new Rule("[blue]Schakel een vraag uit[/]").Centered());
            AnsiConsole.WriteLine();

            var topics = manager.GetAllTopics().ToList();
            var topicKeuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Kies een topic:[/]")
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                    .AddChoices(topics.Select(t => $"{t.Id}. {t.Name}")));

            int topicId = int.Parse(topicKeuze.Split('.')[0]);

            var questions = manager.GetQuestionsByTopicDTO(topicId).ToList();
            var questionKeuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Kies een vraag om uit te schakelen:[/]")
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                    .AddChoices(questions.Select(q => $"{q.Id}. {q.QuestionText}")));

            int questionId = int.Parse(questionKeuze.Split('.')[0]);
            manager.DisableQuestion(questionId);

            ShowSuccess("Vraag succesvol uitgeschakeld!");
        }

        static void TakeTest()
        {
            AnsiConsole.Write(new Rule("[blue]Voer een test uit[/]").Centered());
            AnsiConsole.WriteLine();

            var tests = manager.GetAllTests().ToList();
            var testKeuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]Kies een test:[/]")
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                    .AddChoices(tests.Select(t => $"{t.Id}. {t.Name}")));

            int testId = int.Parse(testKeuze.Split('.')[0]);

            var questions = manager.GetQuestionsForTestDTO(testId);
            int score = 0;
            int questionNumber = 1;

            foreach (QuestionDTO question in questions)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule($"[blue]Vraag {questionNumber}/{questions.Count}[/]").Centered());
                AnsiConsole.WriteLine();

                AnsiConsole.MarkupLine($"[white]{question.QuestionText}[/]");
                AnsiConsole.WriteLine();

                var antwoordKeuze = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[grey]Kies je antwoord:[/]")
                        .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
                        .AddChoices(question.Answers.Select(a => $"{a.Label}. {a.AnswerText}")));

                char userAnswer = antwoordKeuze[0];
                AnswerDTO correct = question.Answers.FirstOrDefault(a => a.IsCorrect);

                if (userAnswer == correct.Label)
                {
                    AnsiConsole.MarkupLine("[green]Correct![/]");
                    score++;
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]Fout! Juist antwoord: {correct.Label}. {correct.AnswerText}[/]");
                }

                questionNumber++;
                AnsiConsole.MarkupLine("[grey]Druk op Enter om verder te gaan...[/]");
                Console.ReadLine();
            }

            manager.SaveResult(testId, score);

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[blue]Resultaat[/]").Centered());
            AnsiConsole.WriteLine();

            var resultTable = new Table()
                .BorderColor(Color.Blue)
                .Border(TableBorder.Rounded)
                .AddColumn(new TableColumn("[blue]Info[/]").Centered())
                .AddColumn(new TableColumn("[blue]Waarde[/]").Centered());

            resultTable.AddRow("Score", $"{score}/{questions.Count}");
            resultTable.AddRow("Percentage", $"{(score * 100 / questions.Count)}%");

            AnsiConsole.Write(resultTable);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[grey]Druk op Enter om terug te gaan...[/]");
            Console.ReadLine();
        }
    }
}