using System.Windows;

namespace JToolbox.WPF.UI
{
    public class BindingProxy : Freezable
    {
        public static readonly DependencyProperty ProxyProperty =
            DependencyProperty.Register(nameof(Proxy), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public object Proxy
        {
            get { return GetValue(ProxyProperty); }
            set { SetValue(ProxyProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}