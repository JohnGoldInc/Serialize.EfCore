using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.EfCore.Interfaces;
using Serialize.EfCore.Serializers;
using Serialize.EfCore.Tests.Internals;

namespace Serialize.EfCore.Tests.Issues
{
    // https://github.com/esskar/Serialize.EfCore/issues/35
    [TestClass]
    public class Issue35
    {
        [TestMethod]
        public void LetExpressionTests()
        {
            var expressions = new List<Expression>();

            Expression<Func<IEnumerable<int>, IEnumerable<int>>> intExpr = c =>
                from x in c
                let test = 8
                where x == test
                select x;
            expressions.Add(intExpr);

            Expression<Func<IEnumerable<string>, IEnumerable<string>>> strExpr = c =>
                from x in c
                let test = "bar"
                where x == test
                select x;
            expressions.Add(strExpr);            

            foreach (var textSerializer in new ITextSerializer[] { new JsonSerializer(), new XmlSerializer() })
            {
                var serializer = new ExpressionSerializer(textSerializer);
                foreach (var expected in expressions)
                {
                    var serialized = serializer.SerializeText(expected);
                    var actual = serializer.DeserializeText(serialized);

                    ExpressionAssert.AreEqual(expected, actual);
                }
            }
        }
    }
}
