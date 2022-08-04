using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace pdq.core_tests.Mocks
{
    public class MockDbParameterCollection : DbParameterCollection
    {
        private readonly List<MockDbParameter> underlyingList;

        public MockDbParameterCollection()
        {
            this.underlyingList = new List<MockDbParameter>();
        }

        public override int Count => this.underlyingList.Count;

        public override object SyncRoot => null;

        public override int Add(object value)
        {
            this.underlyingList.Add(new MockDbParameter()
            {
                Value = value
            });
            return this.underlyingList.Count;
        }

        public override void AddRange(Array values)
        {
            foreach(var v in values)
            {
                this.underlyingList.Add(new MockDbParameter()
                {
                    Value = v
                });
            }
        }

        public override void Clear() => this.underlyingList.Clear();

        public override bool Contains(object value) => this.underlyingList.Any(p => p.Value == value);

        public override bool Contains(string value) => this.underlyingList.Any(p => p.ParameterName == value);

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
            this.underlyingList.InsertRange(index, items);
        }

        public override IEnumerator GetEnumerator() => this.underlyingList.GetEnumerator();

        public override int IndexOf(object value) => this.underlyingList.FindIndex(p => p.Value == value);

        public override int IndexOf(string parameterName) => this.underlyingList.FindIndex(p => p.ParameterName == parameterName);

        public override void Insert(int index, object value) => this.underlyingList.Insert(index, new MockDbParameter()
        {
            Value = value
        });

        public override void Remove(object value) => this.underlyingList.RemoveAll(p => p.Value == value);

        public override void RemoveAt(int index) => this.underlyingList.RemoveAt(index);

        public override void RemoveAt(string parameterName) => this.underlyingList.RemoveAll(p => p.ParameterName == parameterName);

        protected override DbParameter GetParameter(int index)
        {
            return this.underlyingList[index];
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return this.underlyingList.FirstOrDefault(p => p.ParameterName == parameterName);
        }

        protected override void SetParameter(int index, DbParameter value)
        {
            this.underlyingList[index] = value as MockDbParameter;
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            var index = this.underlyingList.FindIndex(p => p.ParameterName == parameterName);
            SetParameter(index, value);
        }
    }
}

