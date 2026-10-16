using System.ComponentModel;
namespace Модуль_5_задание_3
{
    public class TaskItem : INotifyPropertyChanged
    {
        private string _text;
        private bool _done;
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
            }
        }
        public string DisplayText
        {
            get { return (Done ? "+ " : "- ") + Text; }
        }
        public TaskItem(string text)
        {
            _text = text;
            _done = false;
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