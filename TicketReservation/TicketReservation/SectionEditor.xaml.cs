using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicketReservation
{
    public partial class SectionEditor : UserControl, INotifyPropertyChanged
    {
        public enum SelectionState {Adding, EditingSingle, EditingMultiple};
        public event PropertyChangedEventHandler PropertyChanged;
        protected void UpdateProperty(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region properties
        private SectionManager sectionManager;

        private IDefaultNewResPropSource _defaultpropssource;
        public IDefaultNewResPropSource DefaultPropsSource { get { return _defaultpropssource; } set { _defaultpropssource = value; UpdateProperty(nameof(DefaultPropsSource)); } }

        private string _sectionname;
        public string SectionName { get { return _sectionname; } private set { _sectionname = value; UpdateProperty(nameof(SectionName)); } }

        private SelectionState _currentselectionstate;
        public SelectionState CurrentSelectionState { get { return _currentselectionstate; } private set { _currentselectionstate = value; UpdateProperty(nameof(CurrentSelectionState)); } }
        #endregion

        public SectionEditor(SectionManager sm, string sectionName)
        {
            sectionManager = sm;
            SectionName = sectionName;
            newselectionhandled = false;
            CurrentSelectionState = SelectionState.Adding;

            InitializeComponent();
            DataContext = sectionManager;
            StateTextBlock.DataContext = this;
            ConfirmReservationButton.DataContext = this;

            EditorKindComboBox.ItemsSource = ((ReservationKind[])Enum.GetValues(typeof(ReservationKind))).ToList().GetRange(0, 2);
            MainListView.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(CheckColumnWidth), true);
        }

        public SectionEditor(SectionManager sm, string sectionName, IDefaultNewResPropSource defSource) : this(sm, sectionName) { DefaultPropsSource = defSource; }

        public List<Reservation> GetReservations()
        {
            return new List<Reservation>(sectionManager.Reservations);
        }

        #region Form Manipulation
        private void ControlMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                MainListView.SelectedIndex = -1;
                Keyboard.ClearFocus();
                MainListView.Focus();
            }
        }

        private void RightPanelMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) MainListView.SelectedIndex = -1;
            e.Handled = false;
        }

        private void RightPanelExpanderMouseHandle(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                e.Handled = true;
                Keyboard.ClearFocus();
                (sender as UIElement).Focus();
            }
        }

        private void CheckColumnWidth(object sender, DragDeltaEventArgs e)
        {
            if ((e.OriginalSource as Thumb).TemplatedParent is GridViewColumnHeader)
            {
                GridViewColumnHeader gvch = (e.OriginalSource as Thumb).TemplatedParent as GridViewColumnHeader;
                TextBlock tb = new TextBlock { Text = gvch.Content as string, Padding = new Thickness(5) };
                tb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                tb.Arrange(new Rect(tb.DesiredSize));
                if (gvch.Column.ActualWidth < tb.ActualWidth) gvch.Column.Width = tb.ActualWidth;
            }
        }
        #endregion

        #region Commands
        private void DeleteReservation(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainListView.SelectedIndex >= 0 && MainListView.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Jste si jisti, že chcete smazat tuto rezervaci?", "Potvrzení smazání", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes) sectionManager.RemoveReservationAt(MainListView.SelectedIndex);
            }

            else if (MainListView.SelectedIndex >= 0 && MainListView.SelectedItems.Count > 1)
            {
                if (MessageBox.Show("Jste si jisti, že chcete smazat tyto rezervace?", "Potvrzení smazání", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    List<Reservation> temp = new List<Reservation>();
                    foreach (Reservation r in MainListView.SelectedItems) { temp.Add(r); }
                    foreach (Reservation r in temp) { sectionManager.RemoveReservation(r); }
                }
            }
        }

        private void CanDeleteReservation(object sender, CanExecuteRoutedEventArgs e) { if (MainListView.SelectedIndex >= 0) e.CanExecute = true; else e.CanExecute = false; }
        #endregion

        #region Reservation manipulations
        private ReservationPropertiesContext SaveGetContext()
        {
            bool bookedonmalformed = false;
            if (EditorBookedOnTextBox.Text != "" && EditorBookedOnTextBox.Text.GetDateTime() == null) bookedonmalformed = true;
            bool seatnomalformed = false;
            if (EditorSeatNoTextBox.Text != "") try { int.Parse(EditorSeatNoTextBox.Text.Replace(" ", "")); } catch { seatnomalformed = true; }

            if (seatnomalformed || bookedonmalformed)
            {
                string msg;
                if ((seatnomalformed && !bookedonmalformed) || (!seatnomalformed && bookedonmalformed))
                {
                    msg = bookedonmalformed? "Datum zápisu" : "Číslo stolu";
                    msg += " nebylo změněno kvůli chybnému formátování.";
                }

                else msg = "Položky datum zápisu a číslo stolu nebyly změněny kvůli chybnému formátování.";

                MessageBox.Show(msg, "Chyba formátování", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return new ReservationPropertiesContext() { BookedOn = newbookedon, Contact = newcontact,
                                                        Kind = newkind, Name = newname,
                                                        SeatNo = newseatno, Sold = newsold,
                                                        TicketCode = newticketcode, User = newuser,
                                                        ChangeBookedOn = bookedonchanged && !bookedonmalformed, ChangeContact = contactchanged,
                                                        ChangeKind = kindchanged, ChangeName = namechanged,
                                                        ChangeSeatNo = seatnochanged && !seatnomalformed, ChangeSold = soldchanged,
                                                        ChangeTicketCode = ticketcodechanged, ChangeUser = userchanged
                                                      };
            
        }

        private void SaveReservation(Reservation item) { sectionManager.ChangeReservation(item, SaveGetContext()); }
        private void SaveReservationAt(int index) { sectionManager.ChangeReservationAt(index, SaveGetContext()); }
        private void SaveReservations(List<Reservation> reservations) { sectionManager.MultipleChangeReservation(reservations, SaveGetContext()); }
        
        private void ConfirmReservation(object sender, RoutedEventArgs e)
        {
            switch (CurrentSelectionState)
            {
                case SelectionState.Adding:
                    {
                        break;
                    }

                case SelectionState.EditingSingle:
                    {
                        break;
                    }

                case SelectionState.EditingMultiple:
                    {
                        break;
                    }
            }
        }

        private void CancelReservation(object sender, RoutedEventArgs e)
        {
            ResetAdding();
        }
        #endregion

            #region Reservation Properties TextBoxes handle
        private DateTime bookedon = new DateTime();
        private string user;
        private ReservationKind kind = ReservationKind.None;
        private string name;
        private string contact;
        private bool sold;
        private int seatno = -1;
        private string ticketcode;

        private bool bookedonchanged;
        private bool userchanged;
        private bool kindchanged;
        private bool namechanged;
        private bool contactchanged;
        private bool soldchanged;
        private bool seatnochanged;
        private bool ticketcodechanged;

        private DateTime newbookedon = new DateTime();
        private string newuser;
        private ReservationKind newkind = ReservationKind.None;
        private string newname;
        private string newcontact;
        private bool newsold;
        private int newseatno = -1;
        private string newticketcode;

        private void ReservationTextPropertyUpdated(object sender, TextChangedEventArgs e) { if (sender is Control) UpdateReservationPropsFields((Control)sender, false); }
        private void ReservationSoldUpdated(object sender, RoutedEventArgs e) { if (sender is Control) UpdateReservationPropsFields((Control)sender, false); }
        private void ReservationKindUpdated(object sender, SelectionChangedEventArgs e) { if (sender is Control) UpdateReservationPropsFields((Control)sender, false); }
        private void UpdateAllPropsFields(bool reset)
        {
            UpdateReservationPropsFields(EditorNameTextBox, reset);
            UpdateReservationPropsFields(EditorContactTextBox, reset);
            UpdateReservationPropsFields(EditorSeatNoTextBox, reset);
            UpdateReservationPropsFields(EditorTicketCodeTextBox, reset);
            UpdateReservationPropsFields(EditorUserTextBox, reset);
            UpdateReservationPropsFields(EditorBookedOnTextBox, reset);
            UpdateReservationPropsFields(EditorSoldCheckBox, reset);
            UpdateReservationPropsFields(EditorKindComboBox, reset);
        }

        private void UpdateReservationPropsFields(Control sender, bool reset)
        {
            switch (sender.Name)
            {
                #region EditorNameTextBox
                case "EditorNameTextBox":
                    {
                        newname = EditorNameTextBox.Text;

                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newname != ((Reservation)MainListView.SelectedItem).Name)))
                        {
                            namechanged = true;
                            PropertyControlChangedAdorner.AddToControl(EditorNameTextBox);
                        }

                        else
                        {
                            namechanged = false;
                            PropertyControlChangedAdorner.RemoveFromControl(EditorNameTextBox);
                        }

                        break;
                    }
                #endregion
                #region EditorContactTextBox
                case "EditorContactTextBox":
                    {
                        newcontact = EditorContactTextBox.Text;

                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newcontact != ((Reservation)MainListView.SelectedItem).Contact)))
                        {
                            contactchanged = true;
                            PropertyControlChangedAdorner.AddToControl(EditorContactTextBox);
                        }

                        else
                        {
                            contactchanged = false;
                            PropertyControlChangedAdorner.RemoveFromControl(EditorContactTextBox);
                        }

                        break;
                    }
                #endregion
                #region EditorSeatNoTextBox
                case "EditorSeatNoTextBox":
                    {
                        bool malformed;

                        if (EditorSeatNoTextBox.Text.Length > 0)
                        {
                            int tempseatno;
                            if (int.TryParse(EditorSeatNoTextBox.Text, out tempseatno))
                            {
                                newseatno = tempseatno;
                                malformed = false;
                            }
                            else malformed = true;
                        }

                        else
                        {
                            malformed = false;
                            newseatno = -1;
                        }

                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newseatno != ((Reservation)MainListView.SelectedItem).SeatNo) || malformed)) seatnochanged = true;
                        else seatnochanged = false;
                        
                        if (malformed)
                        {
                            PropertyControlChangedAdorner.RemoveFromControl(EditorSeatNoTextBox);
                            MalformedTBAdorner.AddToControl(EditorSeatNoTextBox);
                            EditorSeatNoTextBox.ToolTip = new ToolTip() { Content = "Zadaný text nelze použít jako číslo stolu (špatné formátování)" };
                        }

                        else if (seatnochanged)
                        {
                            PropertyControlChangedAdorner.AddToControl(EditorSeatNoTextBox);
                            MalformedTBAdorner.RemoveFromControl(EditorSeatNoTextBox);
                            EditorSeatNoTextBox.ToolTip = null;
                        }

                        else
                        {
                            PropertyControlChangedAdorner.RemoveFromControl(EditorSeatNoTextBox);
                            MalformedTBAdorner.RemoveFromControl(EditorSeatNoTextBox);
                            EditorSeatNoTextBox.ToolTip = null;
                        }

                        break;
                    }
                #endregion
                #region EditorTicketCodeTextBox
                case "EditorTicketCodeTextBox":
                    {
                        newticketcode = EditorTicketCodeTextBox.Text;

                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newticketcode != ((Reservation)MainListView.SelectedItem).TicketCode)))
                        {
                            ticketcodechanged = true;
                            PropertyControlChangedAdorner.AddToControl(EditorTicketCodeTextBox);
                        }

                        else
                        {
                            ticketcodechanged = false;
                            PropertyControlChangedAdorner.RemoveFromControl(EditorTicketCodeTextBox);
                        }

                        break;
                    }
                #endregion
                #region EditorUserTextBox
                case "EditorUserTextBox":
                    {
                        newuser = EditorUserTextBox.Text;

                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newuser != ((Reservation)MainListView.SelectedItem).User)))
                        {
                            userchanged = true;
                            PropertyControlChangedAdorner.AddToControl(EditorUserTextBox);
                        }

                        else
                        {
                            userchanged = false;
                            PropertyControlChangedAdorner.RemoveFromControl(EditorUserTextBox);
                        }

                        break;
                    }
                #endregion
                #region EditorBookedOnTextBox
                case "EditorBookedOnTextBox":
                    {
                        bool malformed;

                        if (EditorBookedOnTextBox.Text.Length > 0)
                        {
                            DateTime? dt = EditorBookedOnTextBox.Text.GetDateTime();

                            if (dt != null)
                            {
                                newbookedon = (DateTime)dt;
                                malformed = false;
                            }
                            else malformed = true;
                        }

                        else malformed = false;
                        
                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newbookedon != ((Reservation)MainListView.SelectedItem).BookedOn) || malformed)) bookedonchanged = true;
                        else bookedonchanged = false;

                        if (malformed)
                        {
                            PropertyControlChangedAdorner.RemoveFromControl(EditorBookedOnTextBox);
                            MalformedTBAdorner.AddToControl(EditorBookedOnTextBox);
                            EditorBookedOnTextBox.ToolTip = new ToolTip() { Content = "Zadaný text nelze použít jako datum (špatné formátování)" };
                        }

                        else if (bookedonchanged)
                        {
                            PropertyControlChangedAdorner.AddToControl(EditorBookedOnTextBox);
                            MalformedTBAdorner.RemoveFromControl(EditorBookedOnTextBox);
                            EditorBookedOnTextBox.ToolTip = null;
                        }

                        else
                        {
                            PropertyControlChangedAdorner.RemoveFromControl(EditorBookedOnTextBox);
                            MalformedTBAdorner.RemoveFromControl(EditorBookedOnTextBox);
                            EditorBookedOnTextBox.ToolTip = null;
                        }

                        break;
                    }
                    #endregion
                #region EditorSoldCheckBox
                case "EditorSoldCheckBox":
                    {
                        newsold = (bool)EditorSoldCheckBox.IsChecked;

                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newsold != ((Reservation)MainListView.SelectedItem).Sold)))
                        {
                            soldchanged = true;
                            PropertyControlChangedAdorner.AddToControl(EditorSoldCheckBox);
                        }

                        else
                        {
                            soldchanged = false;
                            PropertyControlChangedAdorner.RemoveFromControl(EditorSoldCheckBox);
                        }

                        break;
                    }
                #endregion
                #region EditorKindComboBox
                case "EditorKindComboBox":
                    {
                        newkind = (ReservationKind)EditorKindComboBox.SelectedIndex;

                        if (!reset && (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newkind != ((Reservation)MainListView.SelectedItem).Kind)))
                        {
                            kindchanged = true;
                            PropertyControlChangedAdorner.AddToControl(EditorKindComboBox);
                        }

                        else
                        {
                            kindchanged = false;
                            PropertyControlChangedAdorner.RemoveFromControl(EditorKindComboBox);
                        }

                        break;
                    }
                #endregion
                default: break;
            }
        }

        private bool newselectionhandled;

        private void ReservationSelected(object sender, SelectionChangedEventArgs e)
        {
            if (!newselectionhandled)
            {
                #region Selection State Update
                SelectionState lastselstate = CurrentSelectionState;
                switch (MainListView.SelectedItems.Count)
                {
                    case 0: CurrentSelectionState = SelectionState.Adding; break;
                    case 1: CurrentSelectionState = SelectionState.EditingSingle; break;
                    default: CurrentSelectionState = SelectionState.EditingMultiple; break;
                }
                #endregion
                #region lastitem
                Reservation lastitem = null;
                if (lastselstate == SelectionState.EditingSingle)
                {
                    if (CurrentSelectionState == SelectionState.EditingSingle || CurrentSelectionState == SelectionState.Adding) lastitem = (Reservation)e.RemovedItems[0];
                    else
                    {
                        switch (e.RemovedItems.Count)
                        {
                            case 0:
                                {
                                    foreach (object selection in MainListView.SelectedItems)
                                    {
                                        bool islast = false;
                                        foreach (object added in e.AddedItems) if (selection != added) islast = true;
                                        if (islast)
                                        {
                                            lastitem = (Reservation)selection;
                                            break;
                                        }
                                    }
                                    break;
                                }

                            case 1: lastitem = (Reservation)e.RemovedItems[0]; break;
                            default: break;
                        }
                    }
                }

                bool gotlastitem = lastitem != null;
                #endregion
                #region laststate handle
                bool switchtoprevious = false;
                switch (lastselstate)
                {
                    case SelectionState.Adding:
                        {
                            if (CurrentSelectionState != SelectionState.Adding)
                            {
                                bookedon = newbookedon;
                                contact = newcontact;
                                kind = newkind;
                                name = newname;
                                seatno = newseatno;
                                sold = newsold;
                                ticketcode = newticketcode;
                                user = newuser;
                            }
                            break;
                        }

                    case SelectionState.EditingSingle:
                    case SelectionState.EditingMultiple:
                        {
                            bool changed = bookedonchanged || contactchanged || kindchanged || namechanged || seatnochanged || soldchanged || ticketcodechanged || userchanged;
                            if ((lastselstate == SelectionState.EditingMultiple && CurrentSelectionState == SelectionState.EditingMultiple) || !changed) break;
                            string msg = lastselstate == SelectionState.EditingSingle ? "V předchozí editované rezervaci jste provedli neuložené změny. Uložit nyní? Můžete se také vrátit k její editaci."
                                                                                      : "V režimu hromadné úpravy jste provedli neuložené změny. Uložit nyní? Můžete se také vrátit k hromadné editaci přechozího výběru.";
                            string tit = lastselstate == SelectionState.EditingSingle ? "Neuložené změny v rezervaci" : "Neuložené změny hromadných úprav";
                            switch (MessageBox.Show(msg, tit, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Yes))
                            {
                                default: break;
                                case MessageBoxResult.Cancel: switchtoprevious = true; break;
                                case MessageBoxResult.Yes:
                                    {
                                        if (lastselstate == SelectionState.EditingSingle && gotlastitem) SaveReservation(lastitem);
                                        else SaveReservations((List<Reservation>)((e.RemovedItems as List<object>).Cast<Reservation>()));
                                        break;
                                    }
                            }
                            break;
                        }
                }
                #endregion
                #region currentstate handle
                if (switchtoprevious)
                {
                    if (lastselstate == SelectionState.EditingSingle)
                    {
                        newselectionhandled = true;
                        MainListView.SelectedIndex = MainListView.Items.IndexOf(lastitem);
                    }

                    else
                    {
                        newselectionhandled = true;
                        MainListView.SelectedItems.Clear();

                        foreach(object o in e.RemovedItems)
                        {
                            newselectionhandled = true;
                            MainListView.SelectedItems.Add(o);
                        }
                    }

                    CurrentSelectionState = lastselstate;
                }

                else
                {
                    switch (CurrentSelectionState)
                    {
                        #region prepare editor for adding
                        case SelectionState.Adding:
                            {
                                if (lastselstate != SelectionState.Adding)
                                {
                                    EditorBookedOnTextBox.Text = bookedon.Ticks > 0 ? bookedon.Day + "." + bookedon.Month + "." + bookedon.Year : "";
                                    EditorContactTextBox.Text = contact;
                                    EditorKindComboBox.SelectedIndex = (int)kind;
                                    EditorNameTextBox.Text = name;
                                    EditorSeatNoTextBox.Text = seatno.ToString();
                                    EditorSoldCheckBox.IsChecked = sold;
                                    EditorTicketCodeTextBox.Text = ticketcode;
                                    EditorUserTextBox.Text = user;
                                }

                                break;
                            }
                        #endregion
                        #region prepare editor for single item editing
                        case SelectionState.EditingSingle:
                            {
                                Reservation propertysource = (Reservation)MainListView.SelectedItem;

                                EditorBookedOnTextBox.Text = propertysource.BookedOn.Ticks > 0 ? propertysource.BookedOn.Day + "." + propertysource.BookedOn.Month + "." + propertysource.BookedOn.Year : "";
                                EditorContactTextBox.Text = propertysource.Contact;
                                EditorKindComboBox.SelectedIndex = (int)(propertysource.Kind);
                                EditorNameTextBox.Text = propertysource.Name;
                                EditorSeatNoTextBox.Text = propertysource.SeatNo.ToString();
                                EditorSoldCheckBox.IsChecked = propertysource.Sold;
                                EditorTicketCodeTextBox.Text = propertysource.TicketCode;
                                EditorUserTextBox.Text = propertysource.User;

                                break;
                            }
                        #endregion
                        #region prepare editor for multiple item editing
                        case SelectionState.EditingMultiple:
                            {
                                newcontact = EditorContactTextBox.Text = "";
                                newkind = ((ReservationKind)(EditorKindComboBox.SelectedIndex = -1));
                                newname = EditorNameTextBox.Text = "";
                                newsold = ((bool)(EditorSoldCheckBox.IsChecked = false));
                                newticketcode = EditorTicketCodeTextBox.Text = "";
                                newuser = EditorUserTextBox.Text = "";

                                EditorBookedOnTextBox.Text = "";
                                newbookedon = new DateTime();
                                EditorSeatNoTextBox.Text = "";
                                newseatno = -1;

                                break;
                            }
                        #endregion
                    }
                }
                #endregion

                UpdateAllPropsFields(true);
            }

            else newselectionhandled = false;
        }

        private void ResetAdding()
        {
            if (DefaultPropsSource != null)
            {
                if (DefaultPropsSource.GetDefaultBookedOn() != null) bookedon = (DateTime)DefaultPropsSource.GetDefaultBookedOn();
                else bookedon = new DateTime();

                kind = DefaultPropsSource.GetDefaultKind();
                sold = DefaultPropsSource.GetDefaultSold();
                user = DefaultPropsSource.GetDefaultUser();
            }

            else
            {
                bookedon = new DateTime();
                kind = ReservationKind.None;
                sold = false;
                user = "";
            }

            contact = "";
            name = "";
            seatno = -1;
            ticketcode = "";
            
            if (CurrentSelectionState == SelectionState.Adding)
            {
                EditorBookedOnTextBox.Text = bookedon.Ticks > 0 ? bookedon.Day + "." + bookedon.Month + "." + bookedon.Year : "";
                EditorContactTextBox.Text = contact;
                EditorKindComboBox.SelectedIndex = (int)kind;
                EditorNameTextBox.Text = name;
                EditorSeatNoTextBox.Text = seatno.ToString();
                EditorSoldCheckBox.IsChecked = sold;
                EditorTicketCodeTextBox.Text = ticketcode;
                EditorUserTextBox.Text = user;

                UpdateAllPropsFields(false);
            }
        }
        #endregion
    }
}