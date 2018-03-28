using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace TicketReservation
{
    public class Filemanager
    {
        public readonly string fileVersionAttributeString = "1";

        public ReservationFileInfo LoadFile(string fileName) { return LoadFile(fileName, null); }
        public ReservationFileInfo LoadFile(string fileName, IDefaultNewResPropSource defsource)
        {
            if (File.Exists(fileName))
            {
                ReservationFileInfo result = new ReservationFileInfo(fileName);

                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlElement root = doc.DocumentElement;

                if (root.Name == "ReservationFile" && root.GetAttribute("Version") == fileVersionAttributeString)
                {
                    foreach (XmlNode node in root.ChildNodes)
                    {
                        if (node.Name == "Sections")
                        {
                            foreach (XmlNode sectionsnode in node.ChildNodes)
                            {
                                if (sectionsnode.Name == "Section")
                                {
                                    string sectionname = (sectionsnode as XmlElement).GetAttribute("SectionName");
                                    List<Reservation> reservations = new List<Reservation>();

                                    foreach (XmlNode sectionnode in sectionsnode.ChildNodes)
                                    {
                                        if (sectionnode.Name == "Reservations")
                                        {
                                            foreach (XmlNode reservationsnode in sectionnode.ChildNodes)
                                            {
                                                if (reservationsnode.Name == "Reservation")
                                                {
                                                    XmlElement reservation = reservationsnode as XmlElement;
                                                    string name = reservation.GetAttribute("Name");
                                                    ReservationKind kind = (ReservationKind)int.Parse(reservation.GetAttribute("Kind"));
                                                    int seatno = int.Parse(reservation.GetAttribute("SeatNo"));
                                                    string ticketcode = reservation.GetAttribute("TicketCode");
                                                    bool sold = bool.Parse(reservation.GetAttribute("Sold"));
                                                    string user = reservation.GetAttribute("User");
                                                    DateTime bookedon = new DateTime(Convert.ToInt64(reservation.GetAttribute("BookedOn")));
                                                    string contact = reservation.GetAttribute("Contact");

                                                    reservations.Add(new Reservation(bookedon, user, kind, name, contact, sold, seatno, ticketcode));
                                                }
                                            }
                                        }
                                    }

                                    if (defsource != null) result.Sections.Add(new SectionEditor(new SectionManager(reservations), sectionname, defsource));
                                    else result.Sections.Add(new SectionEditor(new SectionManager(reservations), sectionname));
                                }
                            }
                        }
                    }
                }

                return result;
            }

            else return null;
        }

        public void Save(ReservationFileInfo savefileinfo)
        {
            SaveFile(savefileinfo.FullName, savefileinfo.Sections, savefileinfo.Layouts);
        }

        public void SaveFile(string path, IEnumerable<SectionEditor> sections, IEnumerable<RoomLayout> layouts)
        {
            using (XmlWriter xw = XmlWriter.Create(new FileStream(path, FileMode.Create), new XmlWriterSettings() { Indent = true, IndentChars = "	", Encoding = Encoding.UTF8 }))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("ReservationFile");
                xw.WriteAttributeString("Version", fileVersionAttributeString);

                xw.WriteStartElement("Sections");
                foreach (SectionEditor se in sections)
                {
                    xw.WriteStartElement("Section");
                    xw.WriteAttributeString("SectionName", se.SectionName);

                    List<Reservation> reservations = se.GetReservations();
                    xw.WriteStartElement("Reservations");
                    foreach (Reservation reservation in reservations)
                    {
                        xw.WriteStartElement("Reservation");

                        xw.WriteAttributeString("Name", reservation.Name);
                        xw.WriteAttributeString("Kind", ((int)reservation.Kind).ToString());
                        xw.WriteAttributeString("SeatNo", reservation.SeatNo.ToString());
                        xw.WriteAttributeString("TicketCode", reservation.TicketCode);
                        xw.WriteAttributeString("Sold", reservation.Sold.ToString());
                        xw.WriteAttributeString("User", reservation.User);
                        xw.WriteAttributeString("BookedOn", reservation.BookedOn.Ticks.ToString());
                        xw.WriteAttributeString("Contact", reservation.Contact);

                        xw.WriteEndElement();
                    }
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                }

                xw.WriteEndElement();
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
            }
        }
    }
}
