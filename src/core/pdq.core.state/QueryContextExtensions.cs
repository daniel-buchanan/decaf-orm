﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using pdq.common;
using pdq.state.Utilities;

namespace pdq.state
{
	internal static class QueryContextExtensions
	{
		public static IHelperExtensions Helpers(this IQueryContext context)
        {
            var internalContext = (IQueryContextInternal)context;
			return new HelperExtensions(internalContext);
        }

		public static string GetTableName(
			this IHelperExtensions self,
			Expression expression)
        {
			var tableType = self.Context.ExpressionHelper.GetParameterType(expression);
			return GetTableName(self, tableType);
        }

        public static string GetTableName(
            this IHelperExtensions self,
            Type type)
            => self.Context.ReflectionHelper.GetTableName(type);

        public static string GetTableName<T>(this IHelperExtensions self)
            => self.Context.ReflectionHelper.GetTableName<T>();

        public static string GetTableAlias(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.ExpressionHelper.GetParameterName(expression);

        public static string GetColumnName(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.ExpressionHelper.GetMemberName(expression);

        public static string GetColumnName(
            this IHelperExtensions self,
            PropertyInfo prop)
            => self.Context.ReflectionHelper.GetFieldName(prop);

        public static IWhere ParseWhere(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.Parsers.Where.Parse(expression, self.Context);

        public static IWhere ParseJoin(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.Parsers.Join.Parse(expression, self.Context);

        public static IEnumerable<DynamicColumnInfo> GetPropertyInformation(
            this IHelperExtensions self,
            Expression obj)
            => self.Context.DynamicExpressionHelper.GetProperties(obj, self.Context);
	}
}

