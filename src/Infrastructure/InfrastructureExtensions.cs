using FlightNode.Common.BaseClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace FlightNode.Identity.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static void SetModified<TEntity>(this DbEntityEntry<TEntity> entry, IEnumerable<Expression<Func<TEntity, object>>> expressions)
            where TEntity : class, IEntity
        {
            foreach (var expression in expressions)
            {
                entry.Property(expression).IsModified = true;
            }
        }
    }
}
