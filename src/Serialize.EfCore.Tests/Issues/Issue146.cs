using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.EfCore.Serializers;
using Serialize.EfCore.Tests.Internals;

namespace Serialize.EfCore.Tests.Issues
{
    /// <summary>
    /// https://github.com/esskar/Serialize.EfCore/issues/146
    /// </summary>
    [TestClass]
    public class Issue146
    {
        [TestMethod]
        public void DefaultExpressionsCanBeSerialized()
        {
            var defaultValue = Expression.Default(typeof(int));
            var expressionSerializer = new ExpressionSerializer(new JsonSerializer());
            expressionSerializer.SerializeText(defaultValue);
        }

        [TestMethod]
        public void DefaultExpressionsCanBeDeserialized()
        {
            var expected = Expression.Default(typeof(int));
            var expressionSerializer = new ExpressionSerializer(new JsonSerializer());
            var actual = expressionSerializer.DeserializeText(expressionSerializer.SerializeText(expected));

            ExpressionAssert.AreEqual(expected, actual);
        }
    }
}
