using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.EfCore.Extensions;
using Serialize.EfCore.Tests.Container;
using Serialize.EfCore.Tests.Internals;

namespace Serialize.EfCore.Tests.Issues
{
    /// <summary>
    /// https://github.com/esskar/Serialize.EfCore/issues/41
    /// </summary>
    [TestClass]
    public class Issue41
    {
        [TestMethod]
        public void ToExpressionWithInterferingType()
        {
            Expression<Func<IEnumerable<int>, IEnumerable<int>>> expression = c =>
                from x in c
                let randomVar123456789 = 8
                where x == randomVar123456789
                select x;

            var node = expression.ToExpressionNode();

            var result = node.ToExpression();

            ExpressionAssert.AreEqual(expression, result);
        }

        [TestMethod]
        public void ToExpressionWithInterferingTypeFromOtherAssembly()
        {
            var container = new ExpressionContainer();
            var expression = container.GetExpression();
            var node = expression.ToExpressionNode();

            var result = node.ToExpression();

            ExpressionAssert.AreEqual(expression, result);
        }
    }
}
