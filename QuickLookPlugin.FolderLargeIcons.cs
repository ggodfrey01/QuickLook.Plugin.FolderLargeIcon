using System.IO;
using System.Windows;                 // Size
using QuickLook.Common.Plugin;        // IViewer, ContextObject
using QuickLook.Plugin.HelloWorld.Views;

namespace QuickLook.Plugin.HelloWorld
{
    // Name this "Plugin" for consistency with official samples.
    public class Plugin : IViewer
    {
        private FolderGridView _view;

        public int Priority => 0;

        public void Init()
        {
            // no-op
        }

        public bool CanHandle(string path)
        {
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }

        public void Prepare(string path, ContextObject context)
        {
            context.PreferredSize = new Size(900, 600);
            context.Title = new DirectoryInfo(path).Name;
            context.IsBusy = true;
        }

        public void View(string path, ContextObject context)
        {
            _view = new FolderGridView();
            context.ViewerContent = _view;

            // Kick off loading the folder content (your control implements this).
            _ = _view.LoadAsync(path);

            context.IsBusy = false;
        }

        public void Cleanup()
        {
            _view?.Cleanup();
            _view = null;
        }
    }
}