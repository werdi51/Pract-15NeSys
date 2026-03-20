using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pract_15.Models;

namespace Pract15
{
    public class DBService
    {
        private Pract15TrpoElectroShopContext context;
        public Pract15TrpoElectroShopContext Context => context;

        private static DBService? instance;
        public static DBService Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBService();
                return instance;
            }
        }
        private DBService()
        {
            context = new Pract15TrpoElectroShopContext();
        }
    }
}
