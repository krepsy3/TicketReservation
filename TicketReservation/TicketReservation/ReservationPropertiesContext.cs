using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketReservation
{
    public struct ReservationPropertiesContext
    {
        public DateTime BookedOn { get; set; }
        public string Contact { get; set; }
        public ReservationKind Kind { get; set; }
        public string Name { get; set; }
        public int SeatNo { get; set; }
        public bool Sold { get; set; }
        public string TicketCode { get; set; }
        public string User { get; set; }

        public bool ChangeBookedOn { get; set; }
        public bool ChangeContact { get; set; }
        public bool ChangeKind { get; set; }
        public bool ChangeName { get; set; }
        public bool ChangeSeatNo { get; set; }
        public bool ChangeSold { get; set; }
        public bool ChangeTicketCode { get; set; }
        public bool ChangeUser { get; set; }

        public Reservation ApplyChanges(Reservation applyto)
        {
            Reservation result = new Reservation(applyto.BookedOn, applyto.User, applyto.Kind, applyto.Name, applyto.Contact, applyto.Sold, applyto.SeatNo, applyto.TicketCode);

            if (ChangeBookedOn) result.BookedOn = BookedOn;
            if (ChangeContact) result.Contact = Contact;
            if (ChangeKind) result.Kind = Kind;
            if (ChangeName) result.Name = Name;
            if (ChangeSeatNo) result.SeatNo = SeatNo;
            if (ChangeSold) result.Sold = Sold;
            if (ChangeTicketCode) result.TicketCode = TicketCode;
            if (ChangeUser) result.User = User;

            return result;
        }
    }
}
