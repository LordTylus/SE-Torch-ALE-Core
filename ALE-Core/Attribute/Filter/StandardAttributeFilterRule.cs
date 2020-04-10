using System;

namespace ALE_Core.Attribute.Filter {

    public class StandardAttributeFilterRule : AbstractAttributeFilterRule {

        public IComparable Comparable { get; }

        public StandardAttributeFilterRule(string Attribute, AttributeRelation Relation, IComparable Comparable) 
            : base(Attribute, Relation) {

            this.Comparable = Comparable;
        }

        public override bool Matches(IAttributeAware attributeAware) {

            if (attributeAware == null)
                return false;

            IComparable comparableAttribute = attributeAware.GetValueOf(Attribute);

            if (comparableAttribute == null)
                return false;

            int compareResult = comparableAttribute.CompareTo(Comparable);

            switch (Relation) {

                case AttributeRelation.EQUALS:
                    return compareResult == 0;
                case AttributeRelation.NOT_EQUALS:
                    return compareResult != 0;
                case AttributeRelation.BIGGER:
                    return compareResult > 0;
                case AttributeRelation.BIGGER_EQUALS:
                    return compareResult >= 0;
                case AttributeRelation.SMALLER:
                    return compareResult < 0;
                case AttributeRelation.SMALLER_EQUALS:
                    return compareResult <= 0;
                default:
                    throw new ArgumentException("Unknown relation '" + Relation + "'!");
            }
        }
    }
}
