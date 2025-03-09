using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Serialize.EfCore.Nodes;

namespace Serialize.EfCore.Interfaces
{
    public interface IExpressionContext
    {
        DbContext DbContext { get; set; }

        BindingFlags? GetBindingFlags();

        ParameterExpression GetParameterExpression(ParameterExpressionNode node);

        Type ResolveType(TypeNode node);
    }
}
