using System.Windows.Input;

namespace TicketReservation
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Alt) });
        public static readonly RoutedUICommand About = new RoutedUICommand("About", "About", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.F2) });
        public static readonly RoutedUICommand SaveAs = new RoutedUICommand("Save As", "Save As", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Shift | ModifierKeys.Control) });
    }
}
