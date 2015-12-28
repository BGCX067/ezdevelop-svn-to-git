using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev.Data
{
    public class DoubleKeyValueObject: DoubleKeyValueObject<Guid, Guid, string>
    {
        public DoubleKeyValueObject(Guid key1, Guid key2, string category, string value) : base(key1, key2, category, value)
        {
        }
    }
}
