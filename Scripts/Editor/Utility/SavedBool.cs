namespace UnityEditor
{
	internal class SavedBool
	{
		private bool m_Value;
		private string m_Name;

		public bool value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				if (this.m_Value == value)
					return;
				this.m_Value = value;
				EditorPrefs.SetBool(this.m_Name, value);
			}
		}

		public SavedBool(string name, bool value)
		{
			this.m_Name = name;
			this.m_Value = EditorPrefs.GetBool(name, value);
		}

		public static implicit operator bool(SavedBool s)
		{
			return s.value;
		}
	}
}