using System;

namespace ALE_Core.Attribute {

    public interface IAttributeAware {

        IComparable GetValueOf(string attribute);

        Type GetValueType(string attribute);
    }
}
