using System;
using System.Collections.Generic;

namespace SahiApp
{
    // משפחה שמקבלת חבילה
    [Serializable]
    public class Family
    {
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Floor { get; set; }
        public int Apartment { get; set; }
        public string Notes { get; set; }
        public bool IsDelivered { get; set; }

        public Family(string lastName, string address, int floor, int apartment, string notes)
        {
            LastName = lastName;
            Address = address;
            Floor = floor;
            Apartment = apartment;
            Notes = notes;
            IsDelivered = false;
        }
    }

    // דרישה 1+2: מחלקת בסיס מופשטת להיררכיה ופולימורפיזם
    [Serializable]
    public abstract class Route
    {
        public string Name { get; set; }
        public List<Family> Families { get; set; } // ארגון עצמים ע"י Collection

        public Route(string name)
        {
            Name = name;
            Families = new List<Family>();
        }

        // מאפיין וירטואלי/מופשט שמחושב בצורה פולימורפית
        public abstract int MaxPackages { get; }
        public abstract string RouteType { get; }

        public int TotalPackages => Families.Count;

        public override string ToString()
        {
            return $"{Name} ({TotalPackages} חבילות) - {RouteType}";
        }
    }

    // דרישה 1: מחלקה נגזרת 1 - מסלול רגלי
    [Serializable]
    public class FootRoute : Route
    {
        public double MaxWalkingDistanceKm { get; set; }

        public FootRoute(string name, double distance) : base(name)
        {
            MaxWalkingDistanceKm = distance;
        }

        public override int MaxPackages => 6; // גג 6 חבילות ברגל
        public override string RouteType => "רגלי (קרוב)";
    }

    // דרישה 1: מחלקה נגזרת 2 - מסלול רכב
    [Serializable]
    public class VehicleRoute : Route
    {
        public string VehicleType { get; set; }

        public VehicleRoute(string name, string vehicleType) : base(name)
        {
            VehicleType = vehicleType;
        }

        public override int MaxPackages => 12; // גג 12 חבילות ברכב
        public override string RouteType => "רכב (יכול להיות רחוק)";
    }

    // מחלקה לניהול המאגר (כולל פונקציות הוספה, שינוי ומחיקה - דרישה 4)
    [Serializable]
    public class DataManager
    {
        public List<string> VolunteersPool { get; set; }
        public List<Route> AllRoutes { get; set; }

        public DataManager()
        {
            VolunteersPool = new List<string>();
            AllRoutes = new List<Route>();
        }

        // דרישה 4: יכולת הוספה, שינוי ומחיקה
        public void AddVolunteer(string name) => VolunteersPool.Add(name);
        public void RemoveVolunteer(string name) => VolunteersPool.Remove(name);

        public void AddRoute(Route route) => AllRoutes.Add(route);
        public void RemoveRoute(Route route) => AllRoutes.Remove(route);
    }
}