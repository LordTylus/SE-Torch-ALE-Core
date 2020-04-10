namespace ALE_Core.Attribute.Filter {

    public class AttributeFilterDefinition {

        public string Attribute { get; }
        public AttributeRelation Relation { get; }
        public string Value { get; }

        public AttributeFilterDefinition(string Attribute, AttributeRelation Relation, string Value) {
            this.Attribute = Attribute;
            this.Relation = Relation;
            this.Value = Value;
        }
    }
}
