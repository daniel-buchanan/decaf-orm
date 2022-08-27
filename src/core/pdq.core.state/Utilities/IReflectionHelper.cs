using System;
using System.Collections.Generic;
using System.Reflection;
using pdq.common;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("pdq.services")]
namespace pdq.state.Utilities
{
    internal interface IReflectionHelper
    {
        bool DetermineIfAnonymous(Type type);
        string GetFieldName(PropertyInfo field, QueryTypes queryType = QueryTypes.None);
        string GetFieldName(MemberInfo field);
        T GetInstanceObject<T>(object obj, string name, Type toCreate, object[] instanceParams);
        object GetInstanceObject(Type toCreate, Type typeParam, object[] instanceParams);
        List<DynamicColumnInfo> GetMemberDetails(dynamic toUse, QueryTypes cmdType = QueryTypes.None);
        List<DynamicColumnInfo> GetMemberDetails(Type toUse, QueryTypes queryType = QueryTypes.None);
        Type GetMemberType(object o, string member);
        object GetPropertyValue(object o, string member);
        string GetTableName<T>();
        string GetTableName(Type tp);
        Type GetUnderlyingType<T>(T someType);
        Type GetUnderlyingType(Type t);
        Type GetUnderlyingType<T>();
        bool IsNullableType(Type toCheck);
        bool IsNullableType<T>();
    }
}