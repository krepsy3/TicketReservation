using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
        private ObservableCollection<ReservationFileInfo> _loadedfiles;
        public ObservableCollection<ReservationFileInfo> LoadedFiles { get { return _loadedfiles; } }

        public MainWindow()
        {
            fileManager = new Filemanager();
            _loadedfiles = new ObservableCollection<ReservationFileInfo>();
            DataContext = this;

            InitializeComponent();
            DefaultKindComboBox.ItemsSource = ((ReservationKind[])Enum.GetValues(typeof(ReservationKind))).ToList().GetRange(0, 2);
        }

        private void WinRendered(object sender, EventArgs e)
        {
            foreach (SectionEditor se in fileManager.LoadFile("test.xml", this))
            {
                LoadedFiles.Add(new ReservationFileInfo("test.xml"));
                LoadedFiles.Add(new ReservationFileInfo("test.xml"));
                LoadedFiles.Add(new ReservationFileInfo("test.xml"));
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

        private void ListBoxDeselect(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) ((ListBox)sender).SelectedIndex = -1;
        }

        private void LBRadioButtonOnLoad(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).GroupName)
            {
                default: break;
                case "LoadedFilesRBGroup": CheckRadioButtons(LoadedFilesListBox); break;
            }
        }

        private void CheckRadioButtons(DependencyObject parent)
        {
            List<RadioButton> rbs = new List<RadioButton>(parent.FindVisualChildren<RadioButton>());
            if (rbs.Count >= 1)
            {
                bool ischecked = false;
                foreach (RadioButton rb in rbs) if ((bool)rb.IsChecked)
                    {
                        ischecked = true;
                        break;
                    }
                if (!ischecked) rbs[0].IsChecked = true;
            }
        } 

        private void UnloadFile(object sender, RoutedEventArgs e)
        {
            LoadedFilesListBox.FindVisualChildren<Button>();
        }
    }
}
