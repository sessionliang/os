using System;
using System.Collections;
using System.Text;

namespace BaiRong.Core.Configuration
{
    public class ProviderCollection : ICollection, IEnumerable
    {
        // Fields
        private Hashtable _Hashtable = new Hashtable();
        private bool _ReadOnly;

        // Methods
        public virtual void Add(ProviderConfig provider)
        {
            if (this._ReadOnly)
            {
                throw new NotSupportedException("Collection Read Only");
            }
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if ((provider.Name == null) || (provider.Name.Length < 1))
            {
                throw new ArgumentException("Config provider name null or empty");
            }
            this._Hashtable.Add(provider.Name, provider);
        }

        public void Clear()
        {
            if (this._ReadOnly)
            {
                throw new NotSupportedException("Collection Read Only");
            }
            this._Hashtable.Clear();
        }

        public void CopyTo(ProviderConfig[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return this._Hashtable.Values.GetEnumerator();
        }

        public void Remove(string name)
        {
            if (this._ReadOnly)
            {
                throw new NotSupportedException("Collection Read Only");
            }
            this._Hashtable.Remove(name);
        }

        public void SetReadOnly()
        {
            if (!this._ReadOnly)
            {
                this._ReadOnly = true;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this._Hashtable.Values.CopyTo(array, index);
        }

        // Properties
        public int Count
        {
            get
            {
                return this._Hashtable.Count;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public ProviderConfig this[string name]
        {
            get
            {
                return (this._Hashtable[name] as ProviderConfig);
            }
        }

        public object SyncRoot
        {
            get
            {
                return this;
            }
        }
    }
}
