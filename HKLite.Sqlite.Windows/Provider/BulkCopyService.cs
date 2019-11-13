using System;

using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.Common;

namespace HKLite
{
    internal abstract class BulkCopyService
    {
        internal abstract DalLayerInfo GetDalyerInfo();
    }
}
