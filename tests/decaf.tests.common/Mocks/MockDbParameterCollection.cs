using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace decaf.tests.common.Mocks
{
    public class MockDbParameterCollection : DbParameterCollection
    {
        private readonly List<MockDbParameter> underlyingList;

        public MockDbParameterCollection()
        {
            underlyingList = new List<MockDbParameter>();
        }

        public override int Count => underlyingList.Count;

        public override object SyncRoot => null;

        public override int Add(object value)
        {
            underlyingList.Add(new MockDbParameter()
            {
                Value = value
            });
            return underlyingList.Count;
        }

        public override void AddRange(Array values)
        {
            foreach(var v in values)
            {
                underlyingList.Add(new MockDbParameter()
                {
                    Value = v
                });
            }
        }

        public override void Clear() => underlyingList.Clear();

        public override bool Contains(object value) => underlyingList.Any(p => p.Value == value);

        public override bool Contains(string value) => underlyingList.Any(p => p.ParameterName == value);

        public override void CopyTo(Array array, int index)
        {
            var items = new List<MockDbParameter>();
            foreach (var v in array)
            {
                items.Add(new MockDbParameter()
                {
                    Value = v
                });
            }
            underlyingList.InsertRange(index, items);
        }

        public override IEnumerator GetEnumerator() => underlyingList.GetEnumerator();

        public override int IndexOf(object value) => underlyingList.FindIndex(p => p.Value == value);

        public override int IndexOf(string parameterName) => underlyingList.FindIndex(p => p.ParameterName == parameterName);

        public override void Insert(int index, object value) => underlyingList.Insert(index, new MockDbParameter()
        {
            Value = value
        });

        public override void Remove(object value) => underlyingList.RemoveAll(p => p.Value == value);

        public override void RemoveAt(int index) => underlyingList.RemoveAt(index);

        public override void RemoveAt(string parameterName) => underlyingList.RemoveAll(p => p.ParameterName == parameterName);

        protected override DbParameter GetParameter(int index)
        {
            return underlyingList[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return underlyingList.FirstOrDefault(p => p.ParameterName == parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            underlyingList[index] = value as MockDbParameter;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var index = underlyingList.FindIndex(p => p.ParameterName == parameterName);
            SetParameter(index, value);
        }
    }
}

