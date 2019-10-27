using System.Collections.Generic;

namespace ALE_Core.Attribute.Filter {

    public class AttributeFilterer {

        public static void Filter<T>(List<T> attributeAwares, List<AbstractAttributeFilterRule> filterRules) where T : IAttributeAware {

            if (filterRules == null || filterRules.Count == 0)
                return;

            for (int i = attributeAwares.Count - 1; i >= 0; i--) {

                T attributeAware = attributeAwares[i];

                foreach (AbstractAttributeFilterRule filterRule in filterRules) {

                    if (!filterRule.Matches(attributeAware)) {
                        attributeAwares.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
