using System;
using System.Collections.Generic;

namespace Utilities
{
	class Pool<T> where T : UnityEngine.Object
	{
		private List<T> m_pool;
		
		private Func<T, bool> m_availabilityFunc;
		private Func<T> m_createFunc;
		
		public Pool (Func<T, bool> onAvailable, Func<T> onCreate)
		{
			m_pool = new List<T>();
			m_availabilityFunc = onAvailable;
			m_createFunc = onCreate;
		}
		
		public int Count { get { return m_pool.Count; } }
		
		public T GetAvailable()
        {
			foreach (T element in m_pool)
            {
				if (m_availabilityFunc(element))
					return element;
            }

			return m_createFunc();
        }
	}
}
