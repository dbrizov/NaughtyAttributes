namespace NaughtyAttributes
{
    public abstract class ShowAttribute : MetaAttribute, IShowAttribute
    {
        public abstract bool Visible { get; }
    }
}