using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.EfCore.Extensions;

namespace Serialize.EfCore.Tests.Issues
{
    /// <summary>
    /// https://github.com/esskar/Serialize.EfCore/issues/68
    /// </summary>
    [TestClass]
    public class Issue68
    {
        class MyEntity
        {
            public string Name { get; set; }
        }

        [TestMethod]
        public void SerializePropertyAsConstant()
        {
            var obj = new MyEntity { Name = "Peter" };
            Expression<Func<MyEntity, bool>> expression =
                x => x.Name != obj.Name;

            var exp = expression.ToExpressionNode().ToString();
            Assert.AreEqual("x => (x.Name != \"Peter\")", exp);
        }

        [TestMethod]
        public void SerializePropertyOfPropertyAsConstant()
        {
            var obj = new MyEntity { Name = "Peter" };
            Expression<Func<MyEntity, bool>> expression =
                x => x.Name.Length != obj.Name.Length;

            var exp = expression.ToExpressionNode().ToString();
            Assert.AreEqual("x => (x.Name.Length != 5)", exp);
        }
    }
}
