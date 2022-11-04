using System.Reactive.Disposables;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleViewer.Shell
{
    public class ShellViewModel : ReactiveObject, IActivatableViewModel
    {
        [Reactive] public string? Message { get; set; }
        public ViewModelActivator Activator { get; }

        public ShellViewModel()
        {
            Activator = new ViewModelActivator();
            this.WhenActivated(d =>
            {
                this.HandleActivation();
                Disposable.Create(() => this.HandleDeactivation())
                  .DisposeWith(d);
            });
        }

        private void HandleActivation()
        {
            Message = "Hello World!";
        }

        private void HandleDeactivation() { }
    }
}
