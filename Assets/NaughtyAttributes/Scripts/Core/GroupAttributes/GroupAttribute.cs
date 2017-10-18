using System;

namespace NaughtyAttributes
{
    public abstract class GroupAttribute : NaughtyAttribute
    {
        private string name;

        public GroupAttribute(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
