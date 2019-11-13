using System;

using System.Collections.Generic;
using System.Text;

namespace HKLite
{
    internal enum RelationTypeEnum
    {
        Equal,
        EqualOrLarger,
        Larger,
        EqualOrSmaller,
        Smaller,
        IsNull,
        Like,
        LikeLeft,
        LikeRight,
    }
    internal enum ConditionTypeEnum
    {
        None,
        And,
        Or,
    }
}
