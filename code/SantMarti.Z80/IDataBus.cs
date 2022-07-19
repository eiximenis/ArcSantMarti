using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Z80
{
    public interface IDataBus
    {
        byte Fetch(ushort address);
    }
}
