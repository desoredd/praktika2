using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
namespace ProjectManagerWpf.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object p)
        {
            return _canExecute == null || _canExecute(p);
        }
        public void Execute(object p)
        {
            _execute(p);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Services _services;
        public ObservableCollection<Employee> Employees { get; private set; }
        public ObservableCollection<Project> Projects { get; private set; }
        public ObservableCollection<TaskItem> Tasks { get; private set; }
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set { _selectedEmployee = value; OnPropertyChanged(); FillEmployeeForm(); }
        }
        private Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value; OnPropertyChanged(); FillProjectForm(); }
        }
        private TaskItem _selectedTask;
        public TaskItem SelectedTask
        {
            get { return _selectedTask; }
            set { _selectedTask = value; OnPropertyChanged(); FillTaskForm(); }
        }
        // поля формы сотрудника
        private string _employeeFullName = "";
        public string EmployeeFullName
        {
            get { return _employeeFullName; }
            set { _employeeFullName = value; OnPropertyChanged(); }
        }
        private string _employeePosition = "";
        public string EmployeePosition
        {
            get { return _employeePosition; }
            set { _employeePosition = value; OnPropertyChanged(); }
        }
        private string _employeeEmail = "";
        public string EmployeeEmail
        {
            get { return _employeeEmail; }
            set { _employeeEmail = value; OnPropertyChanged(); }
        }
        // поля формы проекта
        private string _projectName = "";
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; OnPropertyChanged(); }
        }
        private string _projectDescription = "";
        public string ProjectDescription
        {
            get { return _projectDescription; }
            set { _projectDescription = value; OnPropertyChanged(); }
        }
        private DateTime _projectStartDate = DateTime.Today;
        public DateTime ProjectStartDate
        {
            get { return _projectStartDate; }
            set { _projectStartDate = value; OnPropertyChanged(); }
        }
        private DateTime? _projectEndDate;
        public DateTime? ProjectEndDate
        {
            get { return _projectEndDate; }
            set { _projectEndDate = value; OnPropertyChanged(); }
        }
        // поля формы задачи
        private string _taskTitle = "";
        public string TaskTitle
        {
            get { return _taskTitle; }
            set { _taskTitle = value; OnPropertyChanged(); }
        }
        private string _taskDescription = "";
        public string TaskDescription
        {
            get { return _taskDescription; }
            set { _taskDescription = value; OnPropertyChanged(); }
        }
        private int _taskProgress;
        public int TaskProgress
        {
            get { return _taskProgress; }
            set { _taskProgress = value; OnPropertyChanged(); }
        }
        private TaskStatus _taskStatus;
        public TaskStatus TaskStatus
        {
            get { return _taskStatus; }
            set { _taskStatus = value; OnPropertyChanged(); }
        }
        // отчёт
        private string _reportText = "Здесь будет отчёт.";
        public string ReportText
        {
            get { return _reportText; }
            set { _reportText = value; OnPropertyChanged(); }
        }
        // команды
        public ICommand AddEmployeeCommand { get; private set; }
        public ICommand UpdateEmployeeCommand { get; private set; }
        public ICommand DeleteEmployeeCommand { get; private set; }
        public ICommand AddProjectCommand { get; private set; }
        public ICommand UpdateProjectCommand { get; private set; }
        public ICommand DeleteProjectCommand { get; private set; }
        public ICommand AddTaskCommand { get; private set; }
        public ICommand UpdateTaskCommand { get; private set; }
        public ICommand DeleteTaskCommand { get; private set; }
        public ICommand AssignTaskCommand { get; private set; }
        public ICommand UpdateProgressCommand { get; private set; }
        public ICommand ReportProjectCommand { get; private set; }
        public ICommand ReportAllProjectsCommand { get; private set; }
        public ICommand ReportEmployeeCommand { get; private set; }
        public MainViewModel(Services services)
        {
            _services = services;
            Employees = new ObservableCollection<Employee>();
            Projects = new ObservableCollection<Project>();
            Tasks = new ObservableCollection<TaskItem>();
            AddEmployeeCommand = new RelayCommand(_ => AddEmployee());
            UpdateEmployeeCommand = new RelayCommand(_ => UpdateEmployee(), _ => SelectedEmployee != null);
            DeleteEmployeeCommand = new RelayCommand(_ => DeleteEmployee(), _ => SelectedEmployee != null);
            AddProjectCommand = new RelayCommand(_ => AddProject());
            UpdateProjectCommand = new RelayCommand(_ => UpdateProject(), _ => SelectedProject != null);
            DeleteProjectCommand = new RelayCommand(_ => DeleteProject(), _ => SelectedProject != null);
            AddTaskCommand = new RelayCommand(_ => AddTask());
            UpdateTaskCommand = new RelayCommand(_ => UpdateTask(), _ => SelectedTask != null);
            DeleteTaskCommand = new RelayCommand(_ => DeleteTask(), _ => SelectedTask != null);
            AssignTaskCommand = new RelayCommand(_ => AssignTask(), _ => SelectedTask != null);
            UpdateProgressCommand = new RelayCommand(_ => UpdateProgress(), _ => SelectedTask != null);
            ReportProjectCommand = new RelayCommand(_ => ReportProject(), _ => SelectedProject != null);
            ReportAllProjectsCommand = new RelayCommand(_ => ReportAllProjects());
            ReportEmployeeCommand = new RelayCommand(_ => ReportEmployee(), _ => SelectedEmployee != null);
            ReloadEmployees();
            ReloadProjects();
            ReloadTasks();
        }
        private void ReloadEmployees()
        {
            Employees.Clear();
            foreach (var e in _services.Employees.GetAll()) Employees.Add(e);
        }
        private void ReloadProjects()
        {
            Projects.Clear();
            foreach (var p in _services.Projects.GetAll()) Projects.Add(p);
        }
        private void ReloadTasks()
        {
            Tasks.Clear();
            foreach (var t in _services.Tasks.GetAll()) Tasks.Add(t);
        }
        // сотрудники
        private void AddEmployee()
        {
            if (string.IsNullOrWhiteSpace(EmployeeFullName) ||
                string.IsNullOrWhiteSpace(EmployeeEmail))
            {
                MessageBox.Show("Заполните ФИО и Email.");
                return;
            }
            _services.Employees.Add(new Employee
            {
                FullName = EmployeeFullName,
                Position = EmployeePosition,
                Email = EmployeeEmail
            });
            ReloadEmployees();
            ClearEmployeeForm();
        }
        private void UpdateEmployee()
        {
            if (SelectedEmployee == null) return;
            SelectedEmployee.FullName = EmployeeFullName;
            SelectedEmployee.Position = EmployeePosition;
            SelectedEmployee.Email = EmployeeEmail;
            _services.Employees.Update(SelectedEmployee);
            ReloadEmployees();
        }
        private void DeleteEmployee()
        {
            if (SelectedEmployee == null) return;
            if (MessageBox.Show("Удалить '" + SelectedEmployee.FullName + "'?",
                    "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

            _services.Employees.Delete(SelectedEmployee.Id);
            ReloadEmployees();
            ReloadTasks();
            ClearEmployeeForm();
        }
        // проекты
        private void AddProject()
        {
            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                MessageBox.Show("Заполните название проекта.");
                return;
            }
            _services.Projects.Add(new Project
            {
                Name = ProjectName,
                Description = ProjectDescription,
                StartDate = ProjectStartDate,
                EndDate = ProjectEndDate
            });
            ReloadProjects();
            ClearProjectForm();
        }
        private void UpdateProject()
        {
            if (SelectedProject == null) return;
            SelectedProject.Name = ProjectName;
            SelectedProject.Description = ProjectDescription;
            SelectedProject.StartDate = ProjectStartDate;
            SelectedProject.EndDate = ProjectEndDate;
            _services.Projects.Update(SelectedProject);
            ReloadProjects();
            ReloadTasks();
        }
        private void DeleteProject()
        {
            if (SelectedProject == null) return;
            if (MessageBox.Show("Удалить проект '" + SelectedProject.Name + "' со всеми задачами?",
                    "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            _services.Projects.Delete(SelectedProject.Id);
            ReloadProjects();
            ReloadTasks();
            ClearProjectForm();
        }
        // задачи
        private void AddTask()
        {
            if (SelectedProject == null)
            {
                MessageBox.Show("Выберите проект.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TaskTitle))
            {
                MessageBox.Show("Введите название задачи.");
                return;
            }
            _services.Tasks.Add(new TaskItem
            {
                ProjectId = SelectedProject.Id,
                EmployeeId = SelectedEmployee != null ? (int?)SelectedEmployee.Id : null,
                Title = TaskTitle,
                Description = TaskDescription,
                Progress = Math.Max(0, Math.Min(100, TaskProgress))
            });
            ReloadTasks();
            ClearTaskForm();
        }
        private void UpdateTask()
        {
            if (SelectedTask == null) return;
            SelectedTask.Title = TaskTitle;
            SelectedTask.Description = TaskDescription;
            SelectedTask.Progress = Math.Max(0, Math.Min(100, TaskProgress));
            SelectedTask.Status = TaskStatus;
            _services.Tasks.Update(SelectedTask);
            ReloadTasks();
        }
        private void DeleteTask()
        {
            if (SelectedTask == null) return;
            _services.Tasks.Delete(SelectedTask.Id);
            ReloadTasks();
            ClearTaskForm();
        }
        private void AssignTask()
        {
            if (SelectedTask == null) return;
            try
            {
                _services.AssignTask(SelectedTask.Id,
                    SelectedEmployee != null ? (int?)SelectedEmployee.Id : null);
                ReloadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateProgress()
        {
            if (SelectedTask == null) return;
            try
            {
                _services.UpdateProgress(SelectedTask.Id, TaskProgress);
                ReloadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // отчёты
        private void ReportProject()
        {
            if (SelectedProject == null) return;
            ReportText = _services.BuildProjectReport(SelectedProject.Id);
        }
        private void ReportAllProjects()
        {
            ReportText = _services.BuildAllProjectsReport();
        }
        private void ReportEmployee()
        {
            if (SelectedEmployee == null) return;
            ReportText = _services.BuildEmployeeReport(SelectedEmployee.Id);
        }
        // формы
        private void FillEmployeeForm()
        {
            if (SelectedEmployee == null) return;
            EmployeeFullName = SelectedEmployee.FullName;
            EmployeePosition = SelectedEmployee.Position;
            EmployeeEmail = SelectedEmployee.Email;
        }
        private void FillProjectForm()
        {
            if (SelectedProject == null) return;
            ProjectName = SelectedProject.Name;
            ProjectDescription = SelectedProject.Description;
            ProjectStartDate = SelectedProject.StartDate;
            ProjectEndDate = SelectedProject.EndDate;
        }
        private void FillTaskForm()
        {
            if (SelectedTask == null) return;
            TaskTitle = SelectedTask.Title;
            TaskDescription = SelectedTask.Description;
            TaskProgress = SelectedTask.Progress;
            TaskStatus = SelectedTask.Status;
        }
        private void ClearEmployeeForm()
        {
            EmployeeFullName = "";
            EmployeePosition = "";
            EmployeeEmail = "";
        }
        private void ClearProjectForm()
        {
            ProjectName = "";
            ProjectDescription = "";
            ProjectStartDate = DateTime.Today;
            ProjectEndDate = null;
        }
        private void ClearTaskForm()
        {
            TaskTitle = "";
            TaskDescription = "";
            TaskProgress = 0;
            TaskStatus = TaskStatus.New;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
    }
}