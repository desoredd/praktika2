using System.Windows;
using ProjectManagerWpf.ViewModels;
namespace ProjectManagerWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var db = new Database("project_manager.db");
            db.Initialize();
            var services = new Services(db);
            DataContext = new MainViewModel(services);
        }
    }
}