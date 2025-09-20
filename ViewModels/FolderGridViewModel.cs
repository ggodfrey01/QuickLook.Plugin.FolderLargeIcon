using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QuickLook.Plugin.HelloWorld.Services;

namespace QuickLook.Plugin.HelloWorld.ViewModels
{
    internal sealed class FolderGridViewModel : IDisposable
    {
        public ObservableCollection<FolderItemViewModel> Items { get; } = new();
        public int IconSizePx { get; set; } = 128;

        private readonly SemaphoreSlim _thumbLimiter = new(4);
        private CancellationTokenSource _cts;

        public async Task LoadAsync(string folderPath)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            Items.Clear();

            var entries = Directory.EnumerateFileSystemEntries(folderPath)
                                   .Select(p => new
                                   {
                                       Path = p,
                                       IsDir = Directory.Exists(p),
                                       Name = System.IO.Path.GetFileName(p)
                                   })
                                   .OrderByDescending(e => e.IsDir)
                                   .ThenBy(e => e.Name, StringComparer.CurrentCultureIgnoreCase)
                                   .ToList();

            foreach (var e in entries)
            {
                token.ThrowIfCancellationRequested();
                var vm = new FolderItemViewModel(e.Path, e.Name, e.IsDir);
                Items.Add(vm);
                _ = LoadThumbAsync(vm, token);
            }
        }

        private async Task LoadThumbAsync(FolderItemViewModel vm, CancellationToken token)
        {
            await _thumbLimiter.WaitAsync(token);
            try
            {
                await Task.Yield();
                BitmapSource bmp = ShellImageFactory.GetLargeThumbnail(vm.Path, IconSizePx);
                if (token.IsCancellationRequested) return;
                vm.Thumbnail = bmp;
            }
            catch
            {
                // optional: log
            }
            finally
            {
                _thumbLimiter.Release();
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _thumbLimiter.Dispose();
        }
    }
}