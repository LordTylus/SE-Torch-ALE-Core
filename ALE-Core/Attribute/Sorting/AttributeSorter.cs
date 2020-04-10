using System;
using System.Collections.Generic;

namespace ALE_Core.Attribute.Sorting {

    public class AttributeSorter {

        public static void Sort<T>(List<T> attributeAwares, List<AttributeSortRule> sortRules, AttributeNullDecision nullDecision = AttributeNullDecision.NULLS_ON_BOTTOM) where T : IAttributeAware {

            if (sortRules == null || sortRules.Count == 0)
                return;

            attributeAwares.Sort(delegate (T entry1, T entry2) {

                /* Nullsafety on entry */
                if (entry1 == null) {

                    if (entry2 == null)
                        return 0;

                    if (nullDecision == AttributeNullDecision.NULLS_ON_BOTTOM)
                        return 1;
                    else
                        return -1;

                } else if (entry2 == null) {

                    if (nullDecision == AttributeNullDecision.NULLS_ON_BOTTOM)
                        return -1;
                    else
                        return 1;
                }

                foreach (AttributeSortRule rule in sortRules) {

                    string attribute = rule.Attribute;
                    AttributeNullDecision attributeNullDecision = rule.NullDecision;

                    IComparable attribute1 = entry1.GetValueOf(attribute);
                    IComparable attribute2 = entry2.GetValueOf(attribute);

                    /* Nullsafety on entry */
                    if (attribute1 == null) {

                        if (attribute2 == null)
                            return 0;

                        if (attributeNullDecision == AttributeNullDecision.NULLS_ON_BOTTOM)
                            return 1;
                        else
                            return -1;

                    } else if (attribute2 == null) {

                        if (attributeNullDecision == AttributeNullDecision.NULLS_ON_BOTTOM)
                            return -1;
                        else
                            return 1;
                    }

                    int result = attribute1.CompareTo(attribute2);

                    if(result != 0) {

                        if (rule.Direction == AttributeSortDirection.DESCENDING)
                            result *= -1;

                        return result;
                    }
                }

                return 0;
            });
        }
    }
}
