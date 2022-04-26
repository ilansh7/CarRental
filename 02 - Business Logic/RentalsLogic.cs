using System;
using System.Linq;
using System.Web.Security;

#pragma warning disable 618

namespace SunshineRentals
{
    public class RentalsLogic : BaseLogic
    {
        #region Fleet

        /// <summary>
        /// Check if this manufactor have vehicles.
        /// </summary>
        /// <param name="manufactorId">The id of manufactor.</param>
        public bool IsManufactorOccupied(int manufactorId)
        {
            return DB.Vehicles.Any(v => v.ManufactorId == manufactorId);
        }

        /// <summary>
        /// Check if this manufactor exsists (to avoid duplicates).
        /// </summary>
        /// <param name="name">The manufactor name.</param>
        public bool IsManufactorExsist(string name)
        {
            return DB.Manufactors.Any(m => m.Name == name);
        }

        /// <summary>
        /// Get manufactor id.
        /// </summary>
        /// <param name="name">The manufactor name.</param>
        public int GetManufactorId(string name)
        {
            var Manufactor = DB.Manufactors.Where(m => m.Name == name).Select(m => m.ManufactorId).FirstOrDefault().ToString();
            return int.Parse(Manufactor);
        }

        /// <summary>
        /// Check if cur vehicle belonf to fleet.
        /// </summary>
        /// <param name="vehicleId">The vehicle's id.</param>
        public bool IsVehicleOccupied(int vehicleId)
        {
            return DB.Fleets.Any(f => f.TypeId == vehicleId);
        }

        /// <summary>
        /// Check if this Vehicle exsists (to avoid duplicates).
        /// </summary>
        /// <param name="manufactorId">Manufactor's id.</param>
        /// <param name="model">The medel of car.</param>
        /// <param name="year">Year of manufature.</param>
        /// <param name="transmittion">automaic/manual.</param>
        public int IsVehicleExsist(int manufactorId, string model, int year, bool transmittion)
        {
            var Vehicle = DB.Vehicles.Where(v => v.ManufactorId == manufactorId && v.Model == model && v.Year == year && v.Transmission == transmittion)
                .Select(v => v.TypeId).FirstOrDefault().ToString();
            return int.Parse(Vehicle);
        }

        /// <summary>
        /// Check if this Vehicle exsists (to avoid duplicates).
        /// </summary>
        /// <param name="vehicleId">The vehicle's id.</param>
        /// <param name="licencePlate">Tvehicle's Licence plate.</param>
        public int IsCarInFleet(int vehicleId, string licencePlate)
        {
            var Fleet = DB.Fleets.Where(f => f.TypeId == vehicleId && f.LicencePlate == licencePlate)
                .Select(f => f.FleetId).FirstOrDefault().ToString();
            return int.Parse(Fleet);
        }

        /// <summary>
        /// Check if car rented.
        /// </summary>
        /// <param name="carId">The car's id.</param>
        public bool IsCarRented(int carId)
        {
            return DB.Rentals.Any(r => r.FleetId == carId);
        }

        #endregion

    }
}
