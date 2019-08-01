using Avalonia;
using Avalonia.Markup.Xaml;

namespace Cyriller.Desktop
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
   }
}