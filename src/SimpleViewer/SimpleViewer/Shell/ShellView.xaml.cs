using System;
using System.Reactive.Linq;

using Kitware.VTK;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using ReactiveMarbles.ObservableEvents;

namespace SimpleViewer.Shell
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView
    {
        public ILogger<ShellView> Logger { get; set; }
        public ShellView(ShellViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            Logger = NullLogger<ShellView>.Instance;
            this.RenderCtl.Events().Load
            .Subscribe(x => Test(this.RenderCtl));
        }


        private void Test(RenderWindowControl renderCtl)
        {
            Logger.LogDebug("LoadTestActor");
            renderCtl.AddTestActors = true;
            renderCtl.RenderWindow.Render();
        }
    }
}
