namespace NaughtyAttributes
{
    public interface IValuableAttribute
    {
        public string ValueName { get; }
        public float Value { get; }
        public bool IsDynamic { get; }
    }
}