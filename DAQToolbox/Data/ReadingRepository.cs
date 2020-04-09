using System.Collections.Generic;
using System.Linq;

namespace DAQToolbox.Data
{
    public class ReadingRepository : Repository<Reading>, IReadingRepository
    {
        public ReadingRepository(DatabaseContext context) : base(context)
        { }

        public IEnumerable<Reading> GetByLabel(string label)
        {
            return context.Set<Reading>().Where(r => r.Label == label);
        }
    }
}
