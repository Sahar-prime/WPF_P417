using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using TextEditor.Models;

namespace TextEditor.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        // Поля класса
        private readonly FileService fileService = new();
        private string text = string.Empty; //Текст в редакторе
        private string? currentPath; //Путь к текущему файлу
        private bool isDitry; //Есть ли не сохраненные изменения
        private bool wordWrap = true; //Переносить ли строки

        //Конструктор для создания команды
        public MainViewModel()
        {
            NewCommand = new RelayCommand(_ => NewDocument());
            OpenCommand = new RelayCommand(_ => OpenDocument());
            SaveCommand = new RelayCommand(_ => SaveDocument());
            SaveAsCommand = new RelayCommand(_ => SaveDocumentAs());
            ExitCommand = new RelayCommand(_ => Application.Current.MainWindow?.Close());
        }
        //Свойства для биндов
        public string Text
        {
            get => text;
            set
            {
                if(SetProperty(ref text,value))
                {
                    isDitry = true;
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }
        public string? CurrentPath
        {
            get => currentPath;
            private set
            {
                if(SetProperty(ref currentPath,value))
                {
                    OnPropertyChanged(nameof(DocumentName));
                    OnPropertyChanged(nameof(WindowTitle));
                }
            }
        }
        public string DocumentName => currentPath is null ? "Безымянный" : Path.GetFileName(currentPath);
        public string WindowTitle => $"{(isDitry ? "*" : "")}{DocumentName} - Текстовый редактор";
        public bool IsDitry
        {
            get => isDitry;
            private set
            {
                if(SetProperty(ref isDitry,value))
                {
                    OnPropertyChanged(nameof(WindowTitle));
                }
            }
        }
        public bool WordWrap
        {
            get => wordWrap;
            set => SetProperty(ref wordWrap,value);
        }
        public string StatusText
        {
            get
            {
                int lines = string.IsNullOrEmpty(text) ? 0 : text.Split('\n').Length;
                return $"Строк: {lines}\tСимволов: {text.Length}";
            }
        }

        // Команды
        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand ExitCommand { get; }

        private void NewDocument()
        {
            if (!CanCloseCurrent()) return;

            SetTextWithoutMarkingDirty(string.Empty);
            CurrentPath = null;
            isDitry = false;
        }
        private void OpenDocument()
        {
            if (!CanCloseCurrent()) return;

            var dialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Открыть файл"
            };
            if(dialog.ShowDialog() != true) return;
            try
            {
                string content = fileService.ReadFile(dialog.FileName);
                SetTextWithoutMarkingDirty(content);
                CurrentPath = dialog.FileName;
                isDitry = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл: \n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool SaveDocument()
        {
            if(currentPath is null)
            {
                return SaveDocumentAs();
            }
            try
            {
                fileService.WriteFile(currentPath, text);
                IsDitry = false;
                return true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Не удалось сохранить файл: \n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private bool SaveDocumentAs()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Сохранить файл",
                FileName = currentPath is null ? "Безымянный.txt" : Path.GetFileName(currentPath)
            };
            if(dialog.ShowDialog() != true) return false;
            CurrentPath = dialog.FileName;
            return SaveDocument();
        }
        public bool CanCloseCurrent()
        {
            if (!IsDitry) return true;
            var result = MessageBox.Show(
                $"Сохранить изменения в файле\"{DocumentName}\"?",
                "Текстовый редактор",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            return result switch
            {
                MessageBoxResult.Yes => SaveDocument(),
                MessageBoxResult.No => true,
                _ => false
            };
        }
        private void SetTextWithoutMarkingDirty(string value)
        {
            text = value;
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(StatusText));
        }
    }
}
