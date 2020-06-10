using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Maps
{
    interface IMapsProvider
    {
        public Task GeoPositionAsync(string query);
    }
}
