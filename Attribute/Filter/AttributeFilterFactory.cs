using System;

namespace ALE_Core.Attribute.Filter {

    public class AttributeFilterFactory {

        public static AttributeFilterDefinition GetFilterDefinition(string text) {

            text = text.Trim();

            string relationString;

            if(text.Contains("!=")) 
                relationString = "!=";
            else if (text.Contains(">="))
                relationString = ">=";
            else if (text.Contains("<="))
                relationString = "<=";
            else if (text.Contains("="))
                relationString = "=";
            else if (text.Contains("<"))
                relationString = "<";
            else if (text.Contains(">"))
                relationString = ">";
            else
                return null;

            var splits = text.Split(new String[] { relationString }, StringSplitOptions.None);

            var relation = GetRelation(relationString);
            if (relation == null)
                return null;

            return new AttributeFilterDefinition(splits[0], relation.Value, splits[1]);
        }

        private static AttributeRelation? GetRelation(string relationString) {

            if (relationString == "=")
                return AttributeRelation.EQUALS;

            if (relationString == "!=")
                return AttributeRelation.NOT_EQUALS;

            if (relationString == "<=")
                return AttributeRelation.SMALLER_EQUALS;

            if (relationString == "<")
                return AttributeRelation.SMALLER;

            if (relationString == ">=")
                return AttributeRelation.BIGGER_EQUALS;

            if (relationString == ">")
                return AttributeRelation.BIGGER;

            return null;
        }
    }
}
