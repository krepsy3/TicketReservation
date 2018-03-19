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
        private ObservableCollection<Reservation> _reservationsinternal;
        private ObservableCollection<Reservation> _reservations { get { return _reservationsinternal; } set { _reservationsinternal = value; UpdateProperty(nameof(Reservations)); } }
        public ObservableCollection<Reservation> Reservations { get { return new ObservableCollection<Reservation>(_reservations); } }

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

        public void ChangeReservationAt(int index, Reservation replacement)
        {
            _reservations[index] = replacement;
        }

        public void MultipleChangeReservation(List<Reservation> reservations)
        {

        }
    }
}
