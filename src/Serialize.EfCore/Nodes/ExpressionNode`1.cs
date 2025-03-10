﻿using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
#if SERIALIZE_LINQ_BORKED_VERION
    [DataContract]
#else
    [DataContract(Name = "ExpressionNodeGeneric")]
#endif
#else
    [DataContract(Name = "tE")]    
#endif
    [Serializable]
#endregion
    [DebuggerDisplay("ExpressionNode")]
    public abstract class ExpressionNode<TExpression> : ExpressionNode where TExpression : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode{TExpression}"/> class.
        /// </summary>
        protected ExpressionNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode{TExpression}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="expression">The expression.</param>
        protected ExpressionNode(INodeFactory factory, TExpression expression)
            : base(factory, expression.NodeType, expression.Type)
        {
            Initialize(expression);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode{TExpression}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="nodeType">Type of the node.</param>
        /// <param name="type">The type.</param>
        protected ExpressionNode(INodeFactory factory, ExpressionType nodeType, Type type = null)
            : base(factory, nodeType, type) { }

        /// <summary>
        /// Initializes this instance using the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected abstract void Initialize(TExpression expression);
    }
}