using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    internal class WhereEntity
    {
        internal string ColumnName;

        internal int ColumnIndex;  // 当多个ColumnName同名时用ColumnIndex来区分

        internal ConditionTypeEnum ConditionType;

        internal RelationTypeEnum RelationType;

        internal object Value;
    }
}
