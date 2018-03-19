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
            bookedon = new DateTime(0);
            newbookedon = new DateTime(0);
            newselectionhandled = false;
            CurrentSelectionState = SelectionState.Adding;

            InitializeComponent();
            DataContext = sectionManager;
            StateTextBlock.DataContext = this;
            ConfirmReservationButton.DataContext = this;

            EditorKindComboBox.ItemsSource = Enum.GetValues(typeof(ReservationKind));
            MainListView.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(CheckColumnWidth), true);
        }

        public SectionEditor(SectionManager sm, string sectionName, IDefaultNewResPropSource defSource) : this(sm, sectionName)
        {
            DefaultPropsSource = defSource;
        }
        
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

        #region Reservation manipulations
        private void DeleteReservationCtMenu(object sender, RoutedEventArgs e)
        {
            if (MainListView.SelectedIndex >= 0 && MainListView.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Jste si jisti, že chcete smazat tuto rezervaci?", "Potvrzení smazání", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    sectionManager.RemoveReservationAt(MainListView.SelectedIndex);
                }
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

        private void SaveReservation(int index, Reservation r)
        {
            bool bookedonmalformed = false;
            if (EditorBookedOnTextBox.Text != "" && EditorBookedOnTextBox.Text.GetDateTime() == null) bookedonmalformed = true;
            bool seatnomalformed = false;
            if (EditorSeatNoTextBox.Text != "") try { int.Parse(EditorSeatNoTextBox.Text.Replace(" ", "")); } catch { seatnomalformed = true; }

            if (contactchanged) r.Contact = newcontact;
            if (kindchanged) r.Kind = newkind;
            if (namechanged) r.Name = newname;
            if (soldchanged) r.Sold = newsold;
            if (ticketcodechanged) r.TicketCode = newticketcode;
            if (userchanged) r.User = newuser;
            if (bookedonchanged && !bookedonmalformed) r.BookedOn = newbookedon;
            if (seatnochanged && !seatnomalformed) r.SeatNo = newseatno;

            sectionManager.ChangeReservationAt(index, r);

            if (seatnomalformed || bookedonmalformed)
            {
                string msg;
                if ((seatnomalformed && !bookedonmalformed) || (!seatnomalformed && bookedonmalformed))
                {
                    msg = bookedonmalformed ? "Datum zápisu" : "Číslo stolu";
                    msg += " nebylo změněno kvůli chybnému formátování.";
                }

                else msg = "Položky datum zápisu a číslo stolu nebyly změněny kvůli chybnému formátování.";

                MessageBox.Show(msg, "Chyba formátování", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        #endregion

        #region Reservation Properties TextBoxes handle
        private DateTime bookedon;
        private string user;
        private ReservationKind kind;
        private string name;
        private string contact;
        private bool sold;
        private int seatno;
        private string ticketcode;

        private bool bookedonchanged;
        private bool userchanged;
        private bool kindchanged;
        private bool namechanged;
        private bool contactchanged;
        private bool soldchanged;
        private bool seatnochanged;
        private bool ticketcodechanged;

        private DateTime newbookedon;
        private string newuser;
        private ReservationKind newkind;
        private string newname;
        private string newcontact;
        private bool newsold;
        private int newseatno;
        private string newticketcode;

        private void ReservationTextPropertyUpdated(object sender, TextChangedEventArgs e) { if (sender is Control) UpdateReservationPropsFields((Control)sender); }
        private void ReservationSoldUpdated(object sender, RoutedEventArgs e) { if (sender is Control) UpdateReservationPropsFields((Control)sender); }
        private void ReservationKindUpdated(object sender, SelectionChangedEventArgs e) { if (sender is Control) UpdateReservationPropsFields((Control)sender); }

        private void UpdateReservationPropsFields(Control sender)
        {
            switch (sender.Name)
            {
                #region EditorNameTextBox
                case "EditorNameTextBox":
                    {
                        newname = EditorNameTextBox.Text;

                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newname != ((Reservation)MainListView.SelectedItem).Name))
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

                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newcontact != ((Reservation)MainListView.SelectedItem).Contact))
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

                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newseatno != ((Reservation)MainListView.SelectedItem).SeatNo) || malformed) seatnochanged = true;
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

                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newticketcode != ((Reservation)MainListView.SelectedItem).TicketCode))
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

                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newuser != ((Reservation)MainListView.SelectedItem).User))
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

                        else malformed = true;
                        
                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newbookedon != ((Reservation)MainListView.SelectedItem).BookedOn) || malformed) bookedonchanged = true;
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

                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newsold != ((Reservation)MainListView.SelectedItem).Sold))
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

                        if (CurrentSelectionState == SelectionState.EditingMultiple || (CurrentSelectionState == SelectionState.EditingSingle && newkind != ((Reservation)MainListView.SelectedItem).Kind))
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
            //Update Section's editing state (set CurrentSelectionState) and get the previous state as lastselstate
            #region stateupdate
            SelectionState lastselstate = CurrentSelectionState;          
            switch (MainListView.SelectedItems.Count)
            {
                case 0: CurrentSelectionState = SelectionState.Adding; break;
                case 1: CurrentSelectionState = SelectionState.EditingSingle; break;
                default: CurrentSelectionState = SelectionState.EditingMultiple; break;
            }
            #endregion

            bool changed = bookedonchanged || contactchanged || kindchanged || namechanged || seatnochanged || soldchanged || ticketcodechanged || userchanged;

        }
        #endregion
    }
}