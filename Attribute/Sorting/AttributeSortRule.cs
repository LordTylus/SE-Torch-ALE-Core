
namespace ALE_Core.Attribute.Sorting {

    public class AttributeSortRule {

        public string Attribute { get; }
        public AttributeSortDirection Direction { get; }
        public AttributeNullDecision NullDecision { get; }

        public AttributeSortRule(string Attribute, AttributeSortDirection Direction = AttributeSortDirection.ASCENDING, AttributeNullDecision NullDecision = AttributeNullDecision.NULLS_ON_BOTTOM) {

            this.Attribute = Attribute;
            this.Direction = Direction;
            this.NullDecision = NullDecision;
        }
    }
}
