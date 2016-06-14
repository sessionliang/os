using System;
using System.Collections;

namespace BaiRong.Core
{
    public class DictionaryEntryArrayList : ArrayList 
	{
        public DictionaryEntry GetEntry(object key)
        {
            DictionaryEntry theEntry = new DictionaryEntry();
            IEnumerator ie = base.GetEnumerator();
            while (ie.MoveNext())
            {
                DictionaryEntry entry = (DictionaryEntry)ie.Current;
                if (entry.Key == key)
                {
                    return entry;
                }
            }
            return theEntry;
        }

        public override object this[int index]
        {
            set
            {
                if (value is DictionaryEntry)
                {
                    base[index] = value;
                }
                else
                {
                    throw new ArgumentException(value.GetType().FullName + " 类型错误", "value");
                }
            }
        }

        public override int Add(object value)
        {
            if (value is DictionaryEntry)
            {
                return base.Add(value);
            }
            else
            {
                throw new ArgumentException(value.GetType().FullName + " 类型错误", "value");
            }
        }

        public override void Insert(int index, object value)
        {
            if (value is DictionaryEntry)
            {
                base.Insert(index, value);
            }
            else
            {
                throw new ArgumentException(value.GetType().FullName + " 类型错误", "value");
            }
        }

        public override void InsertRange(int index, ICollection c)
        {
            System.Collections.IEnumerator ie = c.GetEnumerator();
            while (ie.MoveNext())
            {
                if (!(ie.Current is DictionaryEntry))
                {
                    throw new ArgumentException(ie.Current.GetType().FullName + " 类型错误", "c");
                }
            }
            base.InsertRange(index, c);
        }


        public override void SetRange(int index, ICollection c)
        {
            System.Collections.IEnumerator ie = c.GetEnumerator();
            int i = 0;
            while (ie.MoveNext())
            {
                if (!(ie.Current is DictionaryEntry))
                {
                    throw new ArgumentException(ie.Current.GetType().FullName + " 类型错误", "c");
                }
                i++;
            }
            base.SetRange(index, c);
        }


        public override void AddRange(ICollection c)
        {
            System.Collections.IEnumerator ie = c.GetEnumerator();
            while (ie.MoveNext())
            {
                if (!(ie.Current is DictionaryEntry))
                {
                    throw new ArgumentException(ie.Current.GetType().FullName + " 类型错误", "c");
                }
            }
            base.AddRange(c);
        }

        public ArrayList Keys
        {
            get
            {
                ArrayList arraylist = new ArrayList();
                foreach (DictionaryEntry entry in this)
                {
                    arraylist.Add(entry.Key);
                }
                return arraylist;
            }
        }
	}

}
