using System;
using System.Collections;

namespace BaiRong.Core.Data
{
	public sealed class DummyDataSource : ICollection 
	{

		private int dataItemCount;

		public DummyDataSource(int dataItemCount) 
		{
			this.dataItemCount = dataItemCount;
		}

		public int Count 
		{
			get 
			{
				return dataItemCount;
			}
		}

		public bool IsReadOnly 
		{
			get 
			{
				return false;
			}
		}

		public bool IsSynchronized 
		{
			get 
			{
				return false;
			}
		}

		public object SyncRoot 
		{
			get 
			{
				return this;
			}
		}

		public void CopyTo(Array array, int index) 
		{
			for (IEnumerator e = this.GetEnumerator(); e.MoveNext();)
				array.SetValue(e.Current, index++);
		}

		public IEnumerator GetEnumerator() 
		{
			return new DummyDataSourceEnumerator(dataItemCount);
		}


		private class DummyDataSourceEnumerator : IEnumerator 
		{

			private int count;
			private int index;

			public DummyDataSourceEnumerator(int count) 
			{
				this.count = count;
				this.index = -1;
			}

			public object Current 
			{
				get 
				{
					return null;
				}
			}

			public bool MoveNext() 
			{
				index++;
				return index < count;
			}

			public void Reset() 
			{
				this.index = -1;
			}
		}
	}
}
