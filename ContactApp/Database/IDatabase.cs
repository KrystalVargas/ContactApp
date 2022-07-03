using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Database
{
    interface IDatabase
    {
        public void SaveToDatabase(object contactInfo);
        public object ReadFromDatabase();
        public void CreateDatabase();
    }
}
