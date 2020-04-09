using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAQToolbox.Data
{
    public interface IReadingRepository : IRepository<Reading>
    {
        IEnumerable<Reading> GetByLabel(string label);
    }
}
