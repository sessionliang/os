using System;
using System.Collections;

namespace BaiRong.Core
{
    public class PairArrayList : IEnumerable
	{
        private ArrayList arraylist = new ArrayList();

        public Pair GetPair(string key)
        {
            IEnumerator ie = arraylist.GetEnumerator();
            while (ie.MoveNext())
            {
                Pair pair = (Pair)ie.Current;
                if (pair.Key == key)
                {
                    return pair;
                }
            }
            return null;
        }

        public object GetValue(string key)
        {
            object value = null;
            Pair pair = this.GetPair(key);
            if (pair != null)
            {
                value = pair.Value;
            }
            return value;
        }

        public int Add(Pair pair)
        {
            return this.arraylist.Add(pair);
        }

        public void Insert(int index, Pair pair)
        {
            this.arraylist.Insert(index, pair);
        }

        public ArrayList Keys
        {
            get
            {
                ArrayList keys = new ArrayList();
                foreach (Pair pair in this.arraylist)
                {
                    keys.Add(pair.Key);
                }
                return keys;
            }
        }

        public bool ContainsKey(string key)
        {
            foreach (Pair pair in this.arraylist)
            {
                if (pair.Key == key)
                {
                    return true;
                }
            }
            return false;
        }

        public ArrayList GetKeys(string startKey)
        {
            ArrayList keys = new ArrayList();
            foreach (Pair pair in this.arraylist)
            {
                if (pair.Key.StartsWith(startKey))
                {
                    keys.Add(pair.Key);
                }
            }
            return keys;
        }

        public IEnumerator GetEnumerator()
        {
            return arraylist.GetEnumerator();
        }
    }
}