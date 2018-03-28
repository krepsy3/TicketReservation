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
using System.ComponentModel;
using Microsoft.Win32;

namespace TicketReservation
{
    public partial class MainWindow : Window, IDefaultNewResPropSource
    {
        private Filemanager fileManager;
        private bool skipquitconfirmation = false;

        private ObservableCollection<ReservationFileInfo> _loadedfiles;
        public ObservableCollection<ReservationFileInfo> LoadedFiles { get { return _loadedfiles; } }

        private ObservableCollection<SectionEditor> _loadedsections;
        public ObservableCollection<SectionEditor> LoadedSections { get { return _loadedsections; } }

        public MainWindow()
        {
            fileManager = new Filemanager();
            _loadedfiles = new ObservableCollection<ReservationFileInfo>();
            _loadedsections = new ObservableCollection<SectionEditor>();
            DataContext = this;

            InitializeComponent();
            DefaultKindComboBox.ItemsSource = ((ReservationKind[])Enum.GetValues(typeof(ReservationKind))).ToList().GetRange(0, 2);
        }

        private void WinRendered(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (string arg in args)
            {
                if (arg == "/skip") skipquitconfirmation = true;
                else if (File.Exists(arg))
                {
                    try { ImportFile(fileManager.LoadFile(arg, this)); }
                    catch { }
                }
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
                case "LoadedFilesRBGroup": UpdateLoadedFilesRBGroup(); break;
            }
        }

        private void UpdateLoadedFilesRBGroup()
        {
            List<RadioButton> rbs = LoadedFilesListBox.FindVisualChildren<RadioButton>().ToList();
            foreach (RadioButton rb in rbs) rb.IsChecked = false;
            rbs[rbs.Count - 1].IsChecked = true;
            foreach (ReservationFileInfo rfi in LoadedFiles) if (!rfi.Exists) fileManager.Save(rfi);
        }

        private int GetSelectedFileIndex()
        {
            RadioButton checkedrb = null;
            foreach(RadioButton rb in LoadedFilesListBox.FindVisualChildren<RadioButton>()) if ((bool)rb.IsChecked) checkedrb = rb;
            if (checkedrb != null) return (LoadedFilesListBox.FindVisualChildren<RadioButton>().ToList()).IndexOf(checkedrb);
            else return -1;
        }

        private void AddSection(object sender, RoutedEventArgs e)
        {
            FileNameDialog fnd = null;
            do
            {
                FileNameDialog d = new FileNameDialog("", "Jméno sekce", "Zvolte prosím jméno pro novou sekci:");
                if (!((bool)d.ShowDialog())) return;
                if (d.FileName.Replace(" ", "") == "") MessageBox.Show("Sekce nemůže mít prázdné jméno. Zadejte prosím platné jméno pro sekci.", "Chyba přidání sekce", MessageBoxButton.OK, MessageBoxImage.Error);
                fnd = d;
            }
            while (fnd.FileName.Replace(" ", "") == "");
            
            SectionEditor se = new SectionEditor(new SectionManager(), fnd.FileName, this);
            LoadedSections.Add(se);
            se.Margin = new Thickness(5);
            se.Bound = true;
            MainTabControl.Items.Add(new TabItem() { Content = se, Header = se.SectionName.Length > 0 ? se.SectionName : "Nepojmenovaná sekce" });
            se.FileName = LoadedFiles[GetSelectedFileIndex()].Name;
            LoadedFiles[GetSelectedFileIndex()].Sections.Add(se);
        }

        private void UnloadFile(object sender, RoutedEventArgs e)
        {
            MessageBoxResult r = MessageBox.Show("Přejete si uzavřít spolu se souborem i jeho sekce a sály?", "Chcete zavřít vše", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (r != MessageBoxResult.Cancel)
            {
                ReservationFileInfo unload = null;
                try { unload = LoadedFiles[(LoadedFilesListBox.FindVisualChildren<Button>().ToList()).IndexOf((Button)sender)]; }
                catch { }
                if (unload != null) LoadedFiles.Remove(unload);
                
                if (r == MessageBoxResult.Yes)
                {
                    foreach (SectionEditor se in unload.Sections)
                    {
                        LoadedSections.Remove(se);
                        int index = -1;
                        foreach (TabItem ti in MainTabControl.Items)
                        {
                            if (ti.Content == se)
                            {
                                index = MainTabControl.Items.IndexOf(ti);
                                break;
                            }
                        }

                        if (index >= 0) MainTabControl.Items.RemoveAt(index);
                    }

                    if (MessageBox.Show("Uložit změny provedené v sekcích a sálech?", "Uložit změny", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        fileManager.Save(unload);
                    }
                }

                UpdateLoadedFilesRBGroup();
            }
        }

        private void ImportFile(ReservationFileInfo rfi)
        {
            LoadedFiles.Add(rfi);
            foreach (SectionEditor se in rfi.Sections)
            {
                se.Margin = new Thickness(5);
                se.Bound = true;
                se.FileName = rfi.Name;
                MainTabControl.Items.Add(new TabItem() { Content = se, Header = se.SectionName.Length > 0 ? se.SectionName : "Nepojmenovaná sekce" });
                LoadedSections.Add(se);
            }
        }

        private void CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

        private void CreateFile(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog fsd = new SaveFileDialog();
            fsd.Filter = "Soubory XML (*.xml)|*.xml|Všechny soubory (*.*)|*.*";
            fsd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fsd.Title = "Zvolte jméno souboru a cílovou složku";

            if ((bool)fsd.ShowDialog())
            {
                ReservationFileInfo rfi = new ReservationFileInfo(fsd.FileName);
                LoadedFiles.Add(rfi);
            }
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Soubory XML (*.xml)|*.xml|Všechny soubory (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if ((bool)ofd.ShowDialog()) ImportFile(fileManager.LoadFile(ofd.FileName, this));
        }

        private void SaveFile(object sender, ExecutedRoutedEventArgs e)
        {
            List<SectionEditor> Checkeds = new List<SectionEditor>();
            foreach (CheckBox cb in LoadedSectionsListBox.FindVisualChildren<CheckBox>()) if ((bool)cb.IsChecked) Checkeds.Add(LoadedSections[LoadedSectionsListBox.FindVisualChildren<CheckBox>().ToList().IndexOf(cb)]);
            fileManager.SaveFile(LoadedFiles[GetSelectedFileIndex()].FullName, Checkeds , null);
        }

        private void SaveFileAs(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog fsd = new SaveFileDialog();
            fsd.Filter = "Soubory XML (*.xml)|*.xml|Všechny soubory (*.*)|*.*";
            fsd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            if ((bool)fsd.ShowDialog())
            {
                ReservationFileInfo rfi = new ReservationFileInfo(fsd.FileName);
                foreach (SectionEditor se in LoadedFiles[GetSelectedFileIndex()].Sections) { rfi.Sections.Add(se); }
                foreach (RoomLayout rl in LoadedFiles[GetSelectedFileIndex()].Layouts) { rfi.Layouts.Add(rl); }
                LoadedFiles[GetSelectedFileIndex()] = rfi;
                ApplicationCommands.Save.Execute(null, null);
            }
        }

        private void DisplayHelp(object sender, ExecutedRoutedEventArgs e) { }

        private void DisplayAbout(object sender, ExecutedRoutedEventArgs e) { }

        private void Exit(object sender, ExecutedRoutedEventArgs e) { Close(); }

        private void OnWinClose(object sender, CancelEventArgs e)
        {
            if (!skipquitconfirmation)
            {
                if (MessageBox.Show("Jste si jisti, že chcete odejít? Veškerá neuložená data budou ztracena!", "Potvrzení ukončení", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
