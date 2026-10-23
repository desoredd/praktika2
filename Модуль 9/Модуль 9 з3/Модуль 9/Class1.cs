using System;
using System.ComponentModel;
namespace Модуль_9
{
    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
    public class TaskItem : INotifyPropertyChanged
    {
        private string _text;
        private bool _done;
        private TaskPriority _priority;
        private DateTime? _dueDate;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
                OnPropertyChanged("DisplayText");
            }
        }
        public bool Done
        {
            get { return _done; }
            set
            {
                _done = value;
                OnPropertyChanged("Done");
                OnPropertyChanged("DisplayText");
                OnPropertyChanged("IsOverdue");
                OnPropertyChanged("IsDueSoon");
                OnPropertyChanged("StatusIcon");
            }
        }
        public TaskPriority Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged("Priority");
                OnPropertyChanged("PriorityText");
                OnPropertyChanged("PriorityBrush");
                OnPropertyChanged("DisplayText");
            }
        }
        public DateTime? DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                OnPropertyChanged("DueDate");
                OnPropertyChanged("DueDateText");
                OnPropertyChanged("IsOverdue");
                OnPropertyChanged("IsDueSoon");
                OnPropertyChanged("StatusIcon");
            }
        }
        public string DisplayText
        {
            get { return (Done ? "✔ " : "• ") + Text; }
        }
        public string PriorityText
        {
            get
            {
                switch (Priority)
                {
                    case TaskPriority.High: return "Высокий";
                    case TaskPriority.Medium: return "Средний";
                    default: return "Низкий";
                }
            }
        }
        public string PriorityBrush
        {
            get
            {
                switch (Priority)
                {
                    case TaskPriority.High: return "#E53935";   // красный
                    case TaskPriority.Medium: return "#FB8C00"; // оранжевый
                    default: return "#43A047";                  // зелёный
                }
            }
        }
        public string DueDateText
        {
            get
            {
                if (DueDate == null) return "Без срока";
                return "до " + DueDate.Value.ToString("dd.MM.yyyy");
            }
        }
        public bool IsOverdue
        {
            get
            {
                return !Done && DueDate.HasValue && DueDate.Value.Date < DateTime.Today;
            }
        }
        public bool IsDueSoon
        {
            get
            {
                if (Done || !DueDate.HasValue) return false;
                var days = (DueDate.Value.Date - DateTime.Today).TotalDays;
                return days >= 0 && days <= 2;
            }
        }
        public string StatusIcon
        {
            get
            {
                if (Done) return "✔";
                if (IsOverdue) return "⚠";
                if (IsDueSoon) return "⏰";
                return "";
            }
        }
        public TaskItem(string text, TaskPriority priority = TaskPriority.Medium, DateTime? dueDate = null)
        {
            _text = text;
            _done = false;
            _priority = priority;
            _dueDate = dueDate;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }
}