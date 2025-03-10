﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.EfCore.Interfaces;
using Serialize.EfCore.Serializers;
using Serialize.EfCore.Tests.Internals;

namespace Serialize.EfCore.Tests.Issues
{
    // https://github.com/esskar/Serialize.EfCore/issues/30
    [TestClass]
    public class Issue30
    {
        /*
        [TestMethod]
        public void SerializeDeserializeLambdaWithNullableTest()
        {
            foreach (var textSerializer in new ITextSerializer[] { new JsonSerializer(), new XmlSerializer() })
            {
                var serializer = new ExpressionSerializer(textSerializer);
                string actual;
                string expected;

                {
                    int seven = 7;
                    Expression<Func<Fish, bool>> expectedExpression = f => f.Count == seven;
                    expected = serializer.SerializeText(expectedExpression);
                }

                {
                    int? seven = 7;
                    Expression<Func<Fish, bool>> actualExpression = f => f.Count == seven;
                    actual = serializer.SerializeText(actualExpression);
                }

                Assert.AreEqual(expected, actual);
            }
        }*/

        [TestMethod]
        public void SerializeLambdaWithNullableTest()
        {
            foreach (var textSerializer in new ITextSerializer[] { new JsonSerializer(), new XmlSerializer() })
            {
                var serializer = new ExpressionSerializer(textSerializer);
                var fish = new[]
                {
                    new Fish {Count = 0},
                    new Fish {Count = 1},
                    new Fish(),
                    new Fish {Count = 1}
                };
                int? count = 1;
                Expression<Func<Fish, bool>> expectedExpression = f => f.Count == count;
                var expected = fish.Where(expectedExpression.Compile()).Count();

                var serialized = serializer.SerializeText(expectedExpression);
                var actualExpression = (Expression<Func<Fish, bool>>)serializer.DeserializeText(serialized);
                var actual = fish.Where(actualExpression.Compile()).Count();

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
