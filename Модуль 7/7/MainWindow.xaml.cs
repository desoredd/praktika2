using System.Windows;
using System.Windows.Input;
namespace MyTC
{
    public partial class MainWindow : Window
    {
        private readonly MainLogic logic;
        public MainWindow()
        {
            InitializeComponent();
            logic = new MainLogic();
            DataContext = logic;
            // Логика не должна напрямую вызывать MessageBox (зависимость от WPF)
            logic.ShowMessageAction = (text, title) =>
                MessageBox.Show(text, title);
            // Подписываем события UI на методы логики
            PwdBox.PasswordChanged += OnPasswordChanged;
        }
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
            => logic.TeacherPassword = PwdBox.Password;
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
            => logic.ProcessUserRegistration();
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
            => logic.ProcessUserLogout();
        private void AddOrUpdateQuestionButton_Click(object sender, RoutedEventArgs e)
            => logic.CommitQuestionToCollection();
        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
            => logic.ClearConstructorInputs();
        private void RemoveQuestionButton_Click(object sender, RoutedEventArgs e)
            => logic.RemoveSelectedQuestion();
        private void ClearTestButton_Click(object sender, RoutedEventArgs e)
            => logic.ClearTest();
        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
            => logic.LoadQuestionImage();
        private void ClearImageButton_Click(object sender, RoutedEventArgs e)
            => logic.ClearImage();
        private void SaveTestButton_Click(object sender, RoutedEventArgs e)
            => logic.SerializeTestToFile();
        private void LoadTestButton_Click(object sender, RoutedEventArgs e)
            => logic.DeserializeTestFromFile();
        private void StartTestingButton_Click(object sender, RoutedEventArgs e)
            => logic.InitiateTestingSession();
        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
            => logic.ProcessUserAnswerAndAdvance();
        private void ClearLogsButton_Click(object sender, RoutedEventArgs e)
            => logic.ClearLogs();
        protected override void OnPreviewKeyDown(KeyEventArgs e)
          {
              if (e.Key == Key.F1) { OpenHelp(); e.Handled = true; return; }
              base.OnPreviewKeyDown(e);
          }
        private void OpenHelp()
        {
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "help.chm");
            if (System.IO.File.Exists(path))
                System.Windows.Forms.Help.ShowHelp(null, path);
            else
                MessageBox.Show("Файл 'help.chm' не найден рядом с исполняемым файлом.",
                    "Справка недоступна", MessageBoxButton.OK, MessageBoxImage.Warning);
        }  
    }
}