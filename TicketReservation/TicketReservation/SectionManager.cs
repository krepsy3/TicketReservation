using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservation
{
    public class SectionManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void UpdateProperty(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties
        private ObservableCollection<Reservation> _reservations;
        public ObservableCollection<Reservation> Reservations { get { return _reservations; } }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                UpdateProperty(nameof(Name));
            }
        }
        #endregion

        public SectionManager()
        {
            _reservations = new ObservableCollection<Reservation>();
        }

        public SectionManager(IEnumerable<Reservation> baseList)
        {
            _reservations = new ObservableCollection<Reservation>(baseList);
        }

        public void AddReservation(Reservation reservation)
        {
            _reservations.Add(reservation);
        }

        public void RemoveReservationAt(int index)
        {
            if (index < _reservations.Count) _reservations.RemoveAt(index);
        }

        public void RemoveReservation(Reservation item)
        {
            _reservations.Remove(item);
        }

        public void ChangeReservationAt(int index, ReservationPropertiesContext context)
        {
            _reservations[index] = context.ApplyChanges(_reservations[index]);
        }

        public void ChangeReservation(Reservation item, ReservationPropertiesContext context)
        {
            ChangeReservationAt(_reservations.IndexOf(item), context);
        }

        public void MultipleChangeReservation(List<Reservation> reservations, ReservationPropertiesContext context)
        {

        }
    }
}
