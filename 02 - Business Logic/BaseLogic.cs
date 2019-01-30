using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineRentals
{
    public class BaseLogic : IDisposable
    {
        protected readonly CarRentalEntities DB = new CarRentalEntities();

        public void Dispose()
        {
            DB.Dispose();
        }
    }
}
