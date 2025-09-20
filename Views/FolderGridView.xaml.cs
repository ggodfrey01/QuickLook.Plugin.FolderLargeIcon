using System.Threading.Tasks;
using System.Windows.Controls;
using QuickLook.Plugin.HelloWorld.ViewModels;

namespace QuickLook.Plugin.HelloWorld.Views
{
    public partial class FolderGridView : UserControl
    {
        private readonly FolderGridViewModel _vm = new();

        public FolderGridView()
        {
            InitializeComponent();
            DataContext = _vm;
            List.ItemsSource = _vm.Items;
        }

        public Task LoadAsync(string folderPath) => _vm.LoadAsync(folderPath);

        public void Cleanup() => _vm.Dispose();
    }
}