using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace TicketReservation
{
    class MalformedTBAdorner : Adorner
    {
        public MalformedTBAdorner(UIElement adornedElement) : base(adornedElement) { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen redlinepen = new Pen(Brushes.Red, 2);
            Pen whitelinepen = new Pen(Brushes.White, 3);
            drawingContext.DrawLine(redlinepen, new Point(-1, -1), new Point(ActualWidth + 1, -1));
            drawingContext.DrawLine(redlinepen, new Point(-1, -1), new Point(-1, ActualHeight + 1));
            drawingContext.DrawLine(redlinepen, new Point(ActualWidth + 1, -1), new Point(ActualWidth + 1, ActualHeight + 1));
            drawingContext.DrawLine(redlinepen, new Point(-1, ActualHeight + 1), new Point(ActualWidth + 1, ActualHeight + 1));
            drawingContext.DrawEllipse(Brushes.Red, redlinepen, new Point(ActualWidth - (ActualHeight / 2), ActualHeight / 2), (ActualHeight / 2) - 4, (ActualHeight / 2) - 4);
            drawingContext.DrawLine(whitelinepen, new Point(ActualWidth - (ActualHeight / 2), 5), new Point(ActualWidth - (ActualHeight / 2), ActualHeight - 11));
            drawingContext.DrawEllipse(Brushes.White, whitelinepen, new Point(ActualWidth - (ActualHeight / 2), ActualHeight - 7), 0.7, 0.7);
        }

        public static bool AddToControl(UIElement uielement)
        {
            try
            {
                bool alreadyin = false;
                Adorner[] ac = AdornerLayer.GetAdornerLayer(uielement).GetAdorners(uielement);

                if (ac != null)
                {
                    foreach (Adorner a in ac)
                    {
                        if (a is MalformedTBAdorner) alreadyin = true;
                    }
                }

                if (!alreadyin) AdornerLayer.GetAdornerLayer(uielement).Add(new MalformedTBAdorner(uielement));

                return true;
            }
            catch { return false; }
        }

        public static bool RemoveFromControl(UIElement uielement)
        {
            try
            {
                MalformedTBAdorner self = null;
                Adorner[] ac = AdornerLayer.GetAdornerLayer(uielement).GetAdorners(uielement);

                if (ac != null)
                {
                    foreach (Adorner a in ac)
                    {
                        if (a is MalformedTBAdorner) self = (MalformedTBAdorner)a;
                    }
                }

                if (self != null) AdornerLayer.GetAdornerLayer(uielement).Remove(self);

                return true;
            }
            catch { return false; }
        }
    }

    class PropertyControlChangedAdorner : Adorner
    {
        public PropertyControlChangedAdorner(UIElement adornedElement) : base(adornedElement) { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen yellowlinepen = new Pen(Brushes.Gold, 2);
            drawingContext.DrawLine(yellowlinepen, new Point(-1, -1), new Point(ActualWidth + 1, -1));
            drawingContext.DrawLine(yellowlinepen, new Point(-1, -1), new Point(-1, ActualHeight + 1));
            drawingContext.DrawLine(yellowlinepen, new Point(ActualWidth + 1, -1), new Point(ActualWidth + 1, ActualHeight + 1));
            drawingContext.DrawLine(yellowlinepen, new Point(-1, ActualHeight + 1), new Point(ActualWidth + 1, ActualHeight + 1));
        }

        public static bool AddToControl(UIElement uielement)
        {
            try
            {
                bool alreadyin = false;
                Adorner[] ac = AdornerLayer.GetAdornerLayer(uielement).GetAdorners(uielement);

                if (ac != null)
                {
                    foreach (Adorner a in ac)
                    {
                        if (a is PropertyControlChangedAdorner) alreadyin = true;
                    }
                }

                if (!alreadyin) AdornerLayer.GetAdornerLayer(uielement).Add(new PropertyControlChangedAdorner(uielement));

                return true;
            }
            catch { return false; }
        }

        public static bool RemoveFromControl(UIElement uielement)
        {
            try
            {
                PropertyControlChangedAdorner self = null;
                Adorner[] ac = AdornerLayer.GetAdornerLayer(uielement).GetAdorners(uielement);

                if (ac != null)
                {
                    foreach (Adorner a in ac)
                    {
                        if (a is PropertyControlChangedAdorner) self = (PropertyControlChangedAdorner)a;
                    }
                }

                if (self != null) AdornerLayer.GetAdornerLayer(uielement).Remove(self);

                return true;
            }
            catch { return false; }
        }
    }
}
