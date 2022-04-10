namespace GameCore
{
    ///<Summary>
    /// Um modificador de valor.
    ///</Summary>
    public class AttributeValueMod
    {
        public enum StatModType { Flat, Percent, PercentOverAll }
        public readonly StatModType type;
        public readonly float value;
        public readonly object source;

        public AttributeValueMod(StatModType type, float value, object source)
        {
            this.type = type;
            this.value = value;
            this.source = source;
        }
    }
}