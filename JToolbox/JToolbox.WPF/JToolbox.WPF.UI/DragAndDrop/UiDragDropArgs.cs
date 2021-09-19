using JToolbox.WPF.Core.Awareness.Args;
using System.Windows;

namespace JToolbox.WPF.UI.DragAndDrop
{
    public class UiDragDropArgs : DragDropArgs
    {
        public FrameworkElement SourceElement { get; set; }
        public FrameworkElement TargetElement { get; set; }
    }
}