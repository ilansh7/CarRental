using System;
using System.Linq;
using System.Web.Security;

#pragma warning disable 618

namespace SunshineRentals
{
    public class OrderLogic : BaseLogic
    {
        /// <summary>
        /// Save order to DB.
        /// </summary>
        /// <param name="fleetId">The user's choice of car.</param>
        /// <param name="userId">The user's id.</param>
        /// <param name="from">Start rental request time.</param>
        /// <param name="to">En drental request time.</param>
        public void WriteOrder(int fleetId, int userId, DateTime from, DateTime to)
        {
            Rental rental = new Rental { FleetId = fleetId, UserId = userId, StartRentalDate = from, EndRentalDate = to };
            DB.Rentals.Add(rental);
            DB.SaveChanges();
        }
    }
}
