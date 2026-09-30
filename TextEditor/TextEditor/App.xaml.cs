using System.Configuration;
using System.Data;
using System.Windows;
using TextEditor.ViewModels;
using TextEditor.View;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var vm = new MainViewModel();
            var window = new View.MainWindow { DataContext = vm };
            window.Show();
        }
    }

}
