using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Serialize.EfCore.Interfaces;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Serialize.EfCore.Nodes
{

    public class EntityQueryRootExpressionNode : ExpressionNode<EntityQueryRootExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityQueryRootExpressionNode"/> class.
        /// </summary>
        public EntityQueryRootExpressionNode() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityQueryRootExpressionNode"/> class with the specified factory and expression.
        /// </summary>
        /// <param name="factory">The factory to create nodes.</param>
        /// <param name="expression">The expression to initialize the node with.</param>
        public EntityQueryRootExpressionNode(INodeFactory factory, EntityQueryRootExpression expression)
            : base(factory, expression) { }

        /// <summary>
        /// Gets or sets the type of the elements in the query.
        /// </summary>
        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "E")]
#endif
        #endregion
        public TypeNode? ElementType { get; set; }

        /// <summary>
        /// Initializes the node with the specified expression.
        /// </summary>
        /// <param name="expression">The expression to initialize the node with.</param>
        protected override void Initialize(EntityQueryRootExpression expression)
        {
            ElementType = new TypeNode(Factory, expression.ElementType);
        }

        /// <summary>
        /// Converts the node to an expression.
        /// </summary>
        /// <param name="context">The context for the expression.</param>
        /// <returns>The converted expression.</returns>
        public override Expression ToExpression(IExpressionContext context)
        {
            var elementType = ElementType!.ToType(context);

            IEntityType? entityType = null;

            if (context is ExpressionContext seContext)
            {
                var dbContext = seContext.DbContext;
                entityType = dbContext.Model.GetEntityTypes().FirstOrDefault(e => e.ClrType == elementType);
            }
            if (entityType == null)
            {
#pragma warning disable EF1001 // Internal EF Core API usage.
                var model = new Model();

                entityType = model.AddEntityType(elementType, false, ConfigurationSource.Explicit)!;
#pragma warning restore EF1001 // Internal EF Core API usage.
            }

            return new EntityQueryRootExpression(entityType);
        }


    }
}