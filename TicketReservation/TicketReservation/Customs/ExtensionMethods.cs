using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TicketReservation
{
    static class ExtensionMethods
    {
        public static DateTime? GetDateTime(this string input)
        {
            int[] datenums = new int[3];
            int control = 0;

            string date = input;
            date = date.Replace(" ", "");
            foreach (char c in date) { if (!"0123456789.".Contains(c.ToString())) return null; }

            foreach (string s in date.Split('.'))
            {
                int temp;
                if (int.TryParse(s, out temp) && temp > 0)
                {
                    datenums[control] = temp;
                    control++;
                }

                if (control == 3) break;
            }

            if (control != 3) return null;

            if (datenums[2] <= 9999)
            {
                switch (datenums[1])
                {
                    case 2:
                        {
                            if ((datenums[0] > 0 && datenums[0] < 29 && !DateTime.IsLeapYear(datenums[2])) || (datenums[0] > 0 && datenums[0] < 30 && DateTime.IsLeapYear(datenums[2])))
                                return new DateTime(datenums[2], datenums[1], datenums[0]);
                            else return null;
                        }

                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        {
                            if ((datenums[0] > 0 && datenums[0] < 32))
                                return new DateTime(datenums[2], datenums[1], datenums[0]);
                            else return null;
                        }

                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        {
                            if ((datenums[0] > 0 && datenums[0] < 31))
                                return new DateTime(datenums[2], datenums[1], datenums[0]);
                            else return null;
                        }

                    default: return null;
                }
            }

            else return null;
        }

        public static int? GetInt32(this string input)
        {
            try { return int.Parse(input.Replace(" ","")); }
            catch { return null; }
        }

        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T) yield return (child as T);
                foreach (T childofchild in child.FindVisualChildren<T>()) if (childofchild is T) yield return (childofchild as T);
            }
        }
    }
}
