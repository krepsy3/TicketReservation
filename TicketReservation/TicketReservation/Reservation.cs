using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservation
{
    public enum ReservationKind { None = -1, Table = 0, Stall = 1 }

    public class Reservation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void UpdateProperty(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        #region Properties
        private bool _sold;
        public bool Sold
        {
            get { return _sold; }
            set
            {
                _sold = value;
                UpdateProperty(nameof(Sold));
            }
        }

        private ReservationKind _kind;
        public ReservationKind Kind
        {
            get { return _kind; }
            set
            {
                _kind = value;
                UpdateProperty(nameof(Kind));
            }
        }

        private int _seatno;
        public int SeatNo
        {
            get { return _seatno; }
            set
            {
                _seatno = value;
                if (_seatno < -1) SeatNo = -1;
                UpdateProperty(nameof(SeatNo));
            }
        }

        private string _ticketcode;
        public string TicketCode
        {
            get { return _ticketcode; }
            set
            {
                _ticketcode = value;
                UpdateProperty(nameof(TicketCode));
            }
        }

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

        private string _user;
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                UpdateProperty(nameof(User));
            }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                UpdateProperty(nameof(Contact));
            }
        }

        private DateTime _bookedon;
        public DateTime BookedOn
        {
            get { return _bookedon; }
            set
            {
                _bookedon = value;
                UpdateProperty(nameof(BookedOn));
            }
        }
        #endregion

        public Reservation(DateTime bookedon, string user = "Admin", ReservationKind kind = ReservationKind.Stall, string name = "", string contact = "", bool sold = false, int seatno = -1, string ticketcode = "0" )
        {
            BookedOn = bookedon;
            User = user;
            Kind = kind;
            Name = name;
            Contact = contact;
            Sold = sold;
            SeatNo = seatno;
            TicketCode = ticketcode;
        }
    }
}
