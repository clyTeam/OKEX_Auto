﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OKEX.Auto.Core.Utilities.Linq
{
    /// <summary>
    /// 参考：http://tomasp.net/blog/linq-expand.aspx
    /// </summary>
    public class ExpandableQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IOrderedQueryable
    {
        ExpandableQueryProvider<T> _provider;
        IQueryable<T> _inner;

        internal IQueryable<T> InnerQuery { get { return _inner; } }

        internal ExpandableQuery(IQueryable<T> inner)
        {
            _inner = inner;
            _provider = new ExpandableQueryProvider<T>(this);
        }

        Expression IQueryable.Expression { get { return _inner.Expression; } }
        Type IQueryable.ElementType { get { return typeof(T); } }
        IQueryProvider IQueryable.Provider { get { return _provider; } }
        public IEnumerator<T> GetEnumerator() { return _inner.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _inner.GetEnumerator(); }
        public override string ToString() { return _inner.ToString(); }
    }

    internal class ExpandableQueryProvider<T> : IQueryProvider
    {
        ExpandableQuery<T> _query;

        internal ExpandableQueryProvider(ExpandableQuery<T> query)
        {
            _query = query;
        }

        // The following four methods first call ExpressionExpander to visit the expression tree, then call
        // upon the inner query to do the remaining work.

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new ExpandableQuery<TElement>(_query.InnerQuery.Provider.CreateQuery<TElement>(expression.Expand()));
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return _query.InnerQuery.Provider.CreateQuery(expression.Expand());
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute<TResult>(expression.Expand());
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute(expression.Expand());
        }
    }
}
