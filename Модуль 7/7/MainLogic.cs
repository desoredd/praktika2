using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
namespace MyTC
{
    public enum UserRole { Guest, Teacher, Student }
    public class TQuestion
    {
        public string QuestionText { get; set; } = "";
        public string[] Answers { get; set; } = Array.Empty<string>();
        public int CorrectIndex { get; set; }
        public int Weight { get; set; }
        public string ImagePath { get; set; } = "";
    }
    public class TTestResult
    {
        public string StudentName { get; set; } = "";
        public string GroupName { get; set; } = "";
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public double Percentage { get; set; }
        public int Mark { get; set; }
        public DateTime DateTimeStamp { get; set; }
    }
    public class AnswerOption : INotifyPropertyChanged
    {
        private string _text = "";
        private bool _isCorrect;
        public string Text { get => _text; set { _text = value; OnChanged(); } }
        public bool IsCorrect { get => _isCorrect; set { _isCorrect = value; OnChanged(); } }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
    // Вся бизнес-логика приложения «MyTC»
    public class MainLogic : INotifyPropertyChanged
    {
        // Регистрация в БД-подобных файлах
        private const string TeacherAccessKey = "1";
        public MainLogic()
        {
            CreateDefaultTest();
            RebuildAnswerOptions();
        }
        // КОЛЛЕКЦИИ
        public ObservableCollection<TQuestion> TestQuestions { get; } = new();
        public ObservableCollection<TTestResult> SessionResults { get; } = new();
        public ObservableCollection<TTestResult> VisibleResults { get; } = new();
        public ObservableCollection<AnswerOption> AnswerOptions { get; } = new();
        public ObservableCollection<string> TestingAnswers { get; } = new();
        // АВТОРИЗАЦИЯ
        public UserRole CurrentRole { get; private set; } = UserRole.Guest;
        public string StudentName { get; private set; } = "";
        public string StudentGroup { get; private set; } = "";
        private int _regRoleIndex;
        public int RegRoleIndex
        {
            get => _regRoleIndex;
            set { _regRoleIndex = value; OnChanged(); OnChanged(nameof(IsTeacherRegistration)); }
        }
        public bool IsTeacherRegistration => RegRoleIndex == 1;
        private string _regName = "";
        public string RegName { get => _regName; set { _regName = value; OnChanged(); } }
        private string _regGroup = "";
        public string RegGroup { get => _regGroup; set { _regGroup = value; OnChanged(); } }
        private string _teacherPassword = "";
        public string TeacherPassword { get => _teacherPassword; set { _teacherPassword = value; OnChanged(); } }
        private string _statusBanner = "Текущий статус: Авторизация не пройдена (Доступ ограничен)";
        public string StatusBanner { get => _statusBanner; set { _statusBanner = value; OnChanged(); } }
        private string _statusColor = "MistyRose";
        public string StatusColor { get => _statusColor; set { _statusColor = value; OnChanged(); } }
        private bool _isLoggedIn;
        public bool IsLoggedIn { get => _isLoggedIn; set { _isLoggedIn = value; OnChanged(); } }
        public bool IsTeacher => CurrentRole == UserRole.Teacher;
        public bool IsStudent => CurrentRole == UserRole.Student;
        public bool CanSeeStatistics => IsTeacher;
        public bool CanClearLogs => IsTeacher;
        // КОНСТРУКТОР
        private string _testTitle = "Общий технический тест";
        public string TestTitle { get => _testTitle; set { _testTitle = value; OnChanged(); } }
        private string _questionText = "";
        public string QuestionText { get => _questionText; set { _questionText = value; OnChanged(); } }
        private int _questionWeight = 1;
        public int QuestionWeight { get => _questionWeight; set { _questionWeight = value; OnChanged(); } }
        private int _answersCount = 4;
        public int AnswersCount
        {
            get => _answersCount;
            set
            {
                int v = value < 2 ? 2 : (value > 5 ? 5 : value);
                if (_answersCount == v) return;
                _answersCount = v; OnChanged(); RebuildAnswerOptions();
            }
        }
        private string _loadedImagePath = "";
        public string LoadedImagePath { get => _loadedImagePath; set { _loadedImagePath = value; OnChanged(); } }
        private TQuestion? _selectedQuestion;
        public TQuestion? SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value; OnChanged();
                if (value != null) LoadQuestionToInspector(value);
            }
        }
        // ТЕСТИРОВАНИЕ
        private int _testingQuestionIndex;
        private int _totalUserScore;
        private int _maxPossibleScore;
        private bool _isTesting;
        public bool IsTesting { get => _isTesting; set { _isTesting = value; OnChanged(); } }
        private string _testProgress = "Вопрос 0 из 0.";
        public string TestProgress { get => _testProgress; set { _testProgress = value; OnChanged(); } }
        private string _currentQuestionText = "";
        public string CurrentQuestionText { get => _currentQuestionText; set { _currentQuestionText = value; OnChanged(); } }
        private string _currentQuestionImagePath = "";
        public string CurrentQuestionImagePath { get => _currentQuestionImagePath; set { _currentQuestionImagePath = value; OnChanged(); } }
        private string? _selectedAnswer;
        public string? SelectedAnswer { get => _selectedAnswer; set { _selectedAnswer = value; OnChanged(); } }
        private string _preTestInstructions = "Для начала выполнения теста нажмите на кнопку ниже.";
        public string PreTestInstructions { get => _preTestInstructions; set { _preTestInstructions = value; OnChanged(); } }
        // ЖУРНАЛ
        private string _globalStatistics = "Статистические данные недоступны.";
        public string GlobalStatistics { get => _globalStatistics; set { _globalStatistics = value; OnChanged(); } }
        // МЕТОДЫ: АВТОРИЗАЦИЯ
        public void ProcessUserRegistration()
        {
            if (IsTeacherRegistration)
            {
                if (TeacherPassword != TeacherAccessKey)
                {
                    ShowMessage("Ошибка авторизации: неверный пароль доступа.", "Ошибка");
                    return;
                }
                CurrentRole = UserRole.Teacher;
                StudentName = "Преподаватель";
                StudentGroup = "Управление";
                StatusBanner = "Роль: Преподаватель (Полный доступ)";
                StatusColor = "LightGreen";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(RegName) || string.IsNullOrWhiteSpace(RegGroup))
                {
                    ShowMessage("Для регистрации учащегося необходимо указать ФИО и группу.", "Валидация");
                    return;
                }
                CurrentRole = UserRole.Student;
                StudentName = RegName;
                StudentGroup = RegGroup;
                StatusBanner = $"Роль: Учащийся ({RegName})";
                StatusColor = "LightSkyBlue";
            }
            IsLoggedIn = true;
            EnforceRoleSecurityPolicies();
            RenderResults();
            RaiseRoleChanged();
        }
        public void ProcessUserLogout()
        {
            CurrentRole = UserRole.Guest;
            StudentName = ""; StudentGroup = "";
            RegName = ""; RegGroup = ""; TeacherPassword = ""; RegRoleIndex = 0;
            StatusBanner = "Текущий статус: Авторизация не пройдена (Доступ ограничен)";
            StatusColor = "MistyRose";
            IsLoggedIn = false;
            EnforceRoleSecurityPolicies();
            RenderResults();
            RaiseRoleChanged();
        }
        private void EnforceRoleSecurityPolicies()
        {
            PreTestInstructions = CurrentRole switch
            {
                UserRole.Teacher => "Вы авторизованы как преподаватель. Имитация прохождения теста доступна для отладки.",
                UserRole.Student => $"Студент: {StudentName}, Группа: {StudentGroup}.\n" +
                                    "После запуска начнется сессия контроля знаний. Будьте внимательны.",
                _ => "Внимание: Прохождение теста недоступно без предварительной регистрации."
            };
        }
        private void RaiseRoleChanged()
        {
            OnChanged(nameof(IsTeacher));
            OnChanged(nameof(IsStudent));
            OnChanged(nameof(CanSeeStatistics));
            OnChanged(nameof(CanClearLogs));
        }
        // МЕТОДЫ: КОНСТРУКТОР
        public void RebuildAnswerOptions()
        {
            AnswerOptions.Clear();
            for (int i = 0; i < AnswersCount; i++)
                AnswerOptions.Add(new AnswerOption { Text = $"Вариант {i + 1}", IsCorrect = i == 0 });
        }
        public void CommitQuestionToCollection()
        {
            if (string.IsNullOrWhiteSpace(QuestionText))
            {
                ShowMessage("Валидация отклонена: формулировка вопроса пуста.", "Ошибка");
                return;
            }
            var answers = new string[AnswerOptions.Count];
            int correctIndex = -1;
            for (int i = 0; i < AnswerOptions.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(AnswerOptions[i].Text))
                {
                    ShowMessage($"Валидация отклонена: текст варианта №{i + 1} пуст.", "Ошибка");
                    return;
                }
                answers[i] = AnswerOptions[i].Text;
                if (AnswerOptions[i].IsCorrect) correctIndex = i;
            }
            if (correctIndex < 0)
            {
                ShowMessage("Не выбран правильный ответ.", "Ошибка");
                return;
            }
            var q = new TQuestion
            {
                QuestionText = QuestionText,
                Answers = answers,
                CorrectIndex = correctIndex,
                Weight = QuestionWeight,
                ImagePath = LoadedImagePath
            };
            if (SelectedQuestion != null)
            {
                int idx = TestQuestions.IndexOf(SelectedQuestion);
                if (idx >= 0) TestQuestions[idx] = q;
            }
            else TestQuestions.Add(q);
            ClearConstructorInputs();
        }
        private void LoadQuestionToInspector(TQuestion q)
        {
            QuestionText = q.QuestionText;
            QuestionWeight = q.Weight;
            AnswersCount = q.Answers.Length;
            for (int i = 0; i < q.Answers.Length && i < AnswerOptions.Count; i++)
            {
                AnswerOptions[i].Text = q.Answers[i];
                AnswerOptions[i].IsCorrect = i == q.CorrectIndex;
            }
            LoadedImagePath = q.ImagePath;
        }
        public void ClearConstructorInputs()
        {
            SelectedQuestion = null;
            QuestionText = "";
            QuestionWeight = 1;
            LoadedImagePath = "";
            AnswersCount = 4;
            RebuildAnswerOptions();
        }
        public void RemoveSelectedQuestion()
        {
            if (SelectedQuestion != null) TestQuestions.Remove(SelectedQuestion);
            ClearConstructorInputs();
        }
        public void ClearTest() => TestQuestions.Clear();
        public void LoadQuestionImage()
        {
            var ofd = new OpenFileDialog { Filter = "Изображения|*.png;*.jpg;*.jpeg;*.bmp" };
            if (ofd.ShowDialog() == true) LoadedImagePath = ofd.FileName;
        }
        public void ClearImage() => LoadedImagePath = "";
        public void SerializeTestToFile()
        {
            if (TestQuestions.Count == 0) return;
            var sfd = new SaveFileDialog { Filter = "Файлы тестов MyTC (*.mtc)|*.mtc" };
            if (sfd.ShowDialog() != true) return;
            try
            {
                using var bw = new BinaryWriter(File.Open(sfd.FileName, FileMode.Create));
                bw.Write("MTCF");
                bw.Write(TestTitle);
                bw.Write(TestQuestions.Count);
                foreach (var q in TestQuestions)
                {
                    bw.Write(q.QuestionText);
                    bw.Write(q.Weight);
                    bw.Write(q.ImagePath);
                    bw.Write(q.Answers.Length);
                    foreach (var a in q.Answers) bw.Write(a);
                    bw.Write(q.CorrectIndex);
                }
                ShowMessage("Файловая сериализация успешно завершена.", "Экспорт");
            }
            catch (Exception ex) { ShowMessage($"Ошибка сохранения: {ex.Message}", "Ошибка"); }
        }
        public void DeserializeTestFromFile()
        {
            var ofd = new OpenFileDialog { Filter = "Файлы тестов MyTC (*.mtc)|*.mtc" };
            if (ofd.ShowDialog() != true) return;
            try
            {
                using var br = new BinaryReader(File.Open(ofd.FileName, FileMode.Open));
                if (br.ReadString() != "MTCF") return;
                TestTitle = br.ReadString();
                int count = br.ReadInt32();
                TestQuestions.Clear();
                for (int i = 0; i < count; i++)
                {
                    var q = new TQuestion
                    {
                        QuestionText = br.ReadString(),
                        Weight = br.ReadInt32(),
                        ImagePath = br.ReadString()
                    };
                    int len = br.ReadInt32();
                    q.Answers = new string[len];
                    for (int j = 0; j < len; j++) q.Answers[j] = br.ReadString();
                    q.CorrectIndex = br.ReadInt32();
                    TestQuestions.Add(q);
                }
                ClearConstructorInputs();
                ShowMessage("Импорт завершен.", "Успех");
            }
            catch (Exception ex) { ShowMessage($"Ошибка импорта: {ex.Message}", "Ошибка"); }
        }
        // МЕТОДЫ: ТЕСТИРОВАНИЕ
        public void InitiateTestingSession()
        {
            if (CurrentRole == UserRole.Guest) return;
            if (TestQuestions.Count == 0)
            {
                ShowMessage("Тестовые задания не обнаружены. Обратитесь к преподавателю.", "Внимание");
                return;
            }
            _testingQuestionIndex = 0;
            _totalUserScore = 0;
            _maxPossibleScore = TestQuestions.Sum(q => q.Weight);
            IsTesting = true;
            DisplayCurrentTestingQuestion();
        }
        private void DisplayCurrentTestingQuestion()
        {
            TestingAnswers.Clear();
            SelectedAnswer = null;
            if (_testingQuestionIndex >= TestQuestions.Count) return;
            var q = TestQuestions[_testingQuestionIndex];
            TestProgress = $"Вопрос {_testingQuestionIndex + 1} из {TestQuestions.Count}. " +
                           $"Ценность теста: {_maxPossibleScore} баллов.";
            CurrentQuestionText = q.QuestionText;
            CurrentQuestionImagePath =
                !string.IsNullOrEmpty(q.ImagePath) && File.Exists(q.ImagePath) ? q.ImagePath : "";
            foreach (var a in q.Answers) TestingAnswers.Add(a);
            if (TestingAnswers.Count > 0) SelectedAnswer = TestingAnswers[0];
        }
        public void ProcessUserAnswerAndAdvance()
        {
            if (_testingQuestionIndex >= TestQuestions.Count) return;
            var q = TestQuestions[_testingQuestionIndex];
            int selectedIndex = SelectedAnswer != null ? Array.IndexOf(q.Answers, SelectedAnswer) : -1;
            if (selectedIndex == q.CorrectIndex) _totalUserScore += q.Weight;
            _testingQuestionIndex++;
            if (_testingQuestionIndex < TestQuestions.Count) DisplayCurrentTestingQuestion();
            else FinalizeTestingSession();
        }
        private void FinalizeTestingSession()
        {
            IsTesting = false;
            double percentage = _maxPossibleScore > 0
                ? Math.Round((double)_totalUserScore / _maxPossibleScore * 100.0, 1) : 0;
            var result = new TTestResult
            {
                StudentName = StudentName,
                GroupName = StudentGroup,
                Score = _totalUserScore,
                MaxScore = _maxPossibleScore,
                Percentage = percentage,
                Mark = ConvertToMark(percentage),
                DateTimeStamp = DateTime.Now
            };
            SessionResults.Add(result);
            RenderResults();
            ShowMessage(
                $"Сессия контроля знаний завершена!\n\n" +
                $"Результат: {result.Score} из {result.MaxScore} баллов ({result.Percentage}%).\n" +
                $"Ваша оценка: {result.Mark} баллов.", "Тестирование окончено");
        }
        private static int ConvertToMark(double percentage)
        {
            if (percentage >= 95.0) return 10;
            if (percentage >= 85.0) return 9;
            if (percentage >= 75.0) return 8;
            if (percentage >= 65.0) return 7;
            if (percentage >= 55.0) return 6;
            if (percentage >= 45.0) return 5;
            if (percentage >= 35.0) return 4;
            if (percentage >= 25.0) return 3;
            if (percentage >= 15.0) return 2;
            return 1;
        }
        // МЕТОДЫ: ЖУРНАЛ
        public void RenderResults()
        {
            VisibleResults.Clear();
            foreach (var r in SessionResults)
            {
                if (CurrentRole == UserRole.Teacher ||
                   (CurrentRole == UserRole.Student && r.StudentName == StudentName))
                {
                    VisibleResults.Add(r);
                }
            }
            RecalculateGlobalStatistics();
        }
        public void ClearLogs()
        {
            SessionResults.Clear();
            RenderResults();
        }
        private void RecalculateGlobalStatistics()
        {
            if (SessionResults.Count == 0 || CurrentRole != UserRole.Teacher)
            {
                GlobalStatistics = "Статистические данные для вашей роли недоступны либо журнал пуст.";
                return;
            }
            double totalPerc = 0, totalGrades = 0;
            int passCount = 0;
            foreach (var r in SessionResults)
            {
                totalPerc += r.Percentage;
                totalGrades += r.Mark;
                if (r.Mark >= 4) passCount++;
            }
            GlobalStatistics =
                $"Сводные статистические параметры текущей сессии контроля:\n" +
                $"• Всего попыток сдачи: {SessionResults.Count}\n" +
                $"• Средний процент успеваемости: {Math.Round(totalPerc / SessionResults.Count, 1)}%\n" +
                $"• Средний академический балл: {Math.Round(totalGrades / SessionResults.Count, 2)} из 10.0\n" +
                $"• Процент успеваемости (оценки 4-10): " +
                $"{Math.Round((double)passCount / SessionResults.Count * 100.0, 1)}%";
        }
        // ВСПОМОГАТЕЛЬНОЕ
        private void CreateDefaultTest()
        {
            TestQuestions.Add(new TQuestion
            {
                QuestionText = "Какой тип данных в C# используется для хранения целых чисел со знаком в диапазоне int?",
                Answers = new[] { "short", "int", "long", "uint" },
                CorrectIndex = 1,
                Weight = 1
            });
            TestQuestions.Add(new TQuestion
            {
                QuestionText = "Какое ключевое слово предотвращает возможность наследования от класса?",
                Answers = new[] { "static", "abstract", "sealed", "readonly" },
                CorrectIndex = 2,
                Weight = 2
            });
        }
        // Делегат для показа сообщений — чтобы логика не зависела напрямую от WPF
        public Action<string, string>? ShowMessageAction { get; set; }
        private void ShowMessage(string text, string title)
            => ShowMessageAction?.Invoke(text, title);
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
   }
}