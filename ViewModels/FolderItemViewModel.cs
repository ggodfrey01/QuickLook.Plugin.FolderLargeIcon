using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace QuickLook.Plugin.HelloWorld.ViewModels
{
    internal sealed class FolderItemViewModel : INotifyPropertyChanged
    {
        public string Path { get; }
        public string Name { get; }
        public bool IsDirectory { get; }

        private BitmapSource _thumbnail;
        public BitmapSource Thumbnail
        {
            get => _thumbnail;
            set { _thumbnail = value; OnPropertyChanged(); }
        }

        public FolderItemViewModel(string path, string name, bool isDir)
        {
            Path = path; Name = name; IsDirectory = isDir;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}