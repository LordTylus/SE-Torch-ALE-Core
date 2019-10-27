
namespace ALE_Core.Attribute.Filter {

    public abstract class AbstractAttributeFilterRule {

        public string Attribute { get; }
        public AttributeRelation Relation { get; }

        public AbstractAttributeFilterRule(string Attribute, AttributeRelation Relation) {
            this.Attribute = Attribute;
            this.Relation = Relation;
        }

        public abstract bool Matches(IAttributeAware attributeAware);
    }
}
