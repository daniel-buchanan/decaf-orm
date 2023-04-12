﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using pdq.common;
using pdq.common.Utilities.Reflection.Dynamic;
using pdq.state.Utilities;

namespace pdq.state
{
	public static class QueryContextExtensions
	{
		public static IHelperExtensions Helpers(this IQueryContext context)
        {
            var internalContext = (IQueryContextInternal)context;
			return new HelperExtensions(internalContext);
        }

        private static IQueryContextInternal ToInternal(this IQueryContext context)
            => context as IQueryContextInternal;

		public static string GetTableName(
			this IHelperExtensions self,
			Expression expression)
        {
			var tableType = self.Context.ToInternal().ExpressionHelper.GetParameterType(expression);
			return GetTableName(self, tableType);
        }

        public static string GetTableName(
            this IHelperExtensions self,
            Type type)
            => self.Context.ToInternal().ReflectionHelper.GetTableName(type);

        public static string GetTableName<T>(this IHelperExtensions self)
            => self.Context.ToInternal().ReflectionHelper.GetTableName<T>();

        public static string GetFieldName(this IHelperExtensions self, PropertyInfo prop)
            => self.Context.ToInternal().ReflectionHelper.GetFieldName(prop);

        public static string GetTableAlias(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.ToInternal().ExpressionHelper.GetParameterName(expression);

        public static string GetColumnName(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.ToInternal().ExpressionHelper.GetMemberName(expression);

        public static string GetColumnName(
            this IHelperExtensions self,
            PropertyInfo prop)
            => self.Context.ToInternal().ReflectionHelper.GetFieldName(prop);

        public static IWhere ParseWhere(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.ToInternal().Parsers.Where.Parse(expression, self.Context.ToInternal());

        public static IWhere ParseJoin(
            this IHelperExtensions self,
            Expression expression)
            => self.Context.ToInternal().Parsers.Join.Parse(expression, self.Context.ToInternal());

        public static IEnumerable<DynamicColumnInfo> GetPropertyInformation(
            this IHelperExtensions self,
            Expression obj)
            => self.Context.ToInternal().DynamicExpressionHelper.GetProperties(obj, self.Context.ToInternal());
	}
}

