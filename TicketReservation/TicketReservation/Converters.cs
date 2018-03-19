using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TicketReservation
{
    public class SelectionStateEnumToEditingStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SectionEditor.SelectionState)) return Binding.DoNothing;
            else
            {
                switch ((SectionEditor.SelectionState)value)
                {
                    default:
                    case SectionEditor.SelectionState.Adding: return "Přidávání rezervace";
                    case SectionEditor.SelectionState.EditingSingle: return "Editace rezervace";
                    case SectionEditor.SelectionState.EditingMultiple: return "Hromadné úpravy";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class SelectionStateEnumToEditingButtonStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SectionEditor.SelectionState)) return Binding.DoNothing;
            else
            {
                switch ((SectionEditor.SelectionState)value)
                {
                    default:
                    case SectionEditor.SelectionState.Adding: return "Přidat rezervaci";
                    case SectionEditor.SelectionState.EditingSingle:
                    case SectionEditor.SelectionState.EditingMultiple: return "Potvrdit změny";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class SelectedKindIndexToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) return Binding.DoNothing;
            else
            {
                if ((int)value == 0) return true;
                else return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class SoldBoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return Binding.DoNothing;
            else
            {
                if ((bool)value) return "Zaplaceno";
                else return "Jen Rezervováno";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class SeatNoIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) return Binding.DoNothing;
            else
            {
                if ((int)value >= 0) return value.ToString();
                else return "není";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public class TicketKindEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ReservationKind)) return Binding.DoNothing;
            else
            {
                if ((ReservationKind)value == ReservationKind.Stall) return "Stání";
                else return "Stůl";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string)) return Binding.DoNothing;
            else
            {
                if ((string)value == "Stání") return ReservationKind.Stall;
                else if ((string)value == "Stůl") return ReservationKind.Table;
                else return Binding.DoNothing;
            }
        }
    }

    public class InvertedBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return Binding.DoNothing;
            else
            {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility)) return Binding.DoNothing;
            else
            {
                return (Visibility)value == Visibility.Hidden || (Visibility)value == Visibility.Collapsed ? true : false;
            }
        }
    }

    public class TextBoxLengthToHelpTextBlockVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int)) return Binding.DoNothing;
            else
            {
                return (int)value > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility)) return Binding.DoNothing;
            else
            {
                return (Visibility)value == Visibility.Hidden || (Visibility)value == Visibility.Collapsed ? 0 : 1;
            }
        }
    }
}