using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicketReservation
{
    public partial class MainWindow : Window, IDefaultNewResPropSource
    {
        private Filemanager fileManager;

        public MainWindow()
        {
            fileManager = new Filemanager();

            InitializeComponent();
            DefaultKindComboBox.ItemsSource = ((ReservationKind[])Enum.GetValues(typeof(ReservationKind))).ToList().GetRange(0, 2);
        }

        private void WinRendered(object sender, EventArgs e)
        {
            foreach (SectionEditor se in fileManager.LoadFile("test.xml", this))
            {
                se.Margin = new Thickness(5);
                se.DefaultPropsSource = this;

                MainTabControl.Items.Add(new TabItem() { Content = se, Header = se.SectionName.Length > 0 ? se.SectionName : "Nepojmenovaná sekce" });
            }
        }

        private void DefExpanderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Keyboard.ClearFocus();
                (e.OriginalSource as UIElement).Focus();
            }
        }

        public DateTime? GetDefaultBookedOn() { return DefaultBookedOnTextBox.Text.GetDateTime(); }
        public ReservationKind GetDefaultKind() { return (ReservationKind)DefaultKindComboBox.SelectedIndex; }
        public bool GetDefaultSold() { return (bool)DefaultSoldCheckBox.IsChecked; }
        public string GetDefaultUser() { return DefaultUserTextBox.Text; }
    }
}
