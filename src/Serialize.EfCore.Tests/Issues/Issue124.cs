﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.EfCore.Serializers;

namespace Serialize.EfCore.Tests.Issues
{
    // https://github.com/esskar/Serialize.EfCore/issues/124
    [TestClass]
    public class Issue124
    {
        [TestMethod]
        public void DeserializeExpressionsWithCollectionInterface()
        {
            var request = new Request();
            Expression<Func<Thing, bool>> expression = 
                x => request.Ids.Contains(x.Id) & x.UserId == request.Id;

            DoSerializeThenDeserialize(expression);
        }

        private static void DoSerializeThenDeserialize<T>(Expression<Func<T, bool>> predicate)
        {
            var expressionSerializer = new ExpressionSerializer(new JsonSerializer());
            var serialized = expressionSerializer.SerializeText(predicate);
            expressionSerializer.DeserializeText(serialized);
        }

        public class Thing
        {
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

        }

        public class Request
        {
            public ICollection<Guid> Ids { get; set; }

            public Guid Id { get; set; }
        }
    }
}
