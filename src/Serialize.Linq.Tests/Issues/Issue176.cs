using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.Linq.Factories;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Nodes;
using Serialize.Linq.Serializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using static Serialize.Linq.Tests.Issues.Issue176.EntityQueryRootExpressionNode;

namespace Serialize.Linq.Tests.Issues
{
    [TestClass]
    public class Issue176
    {

        [TestMethod]
        public void ExtendingExpressionSerializerFailsForTakeMethod()
        {
            var take = 2;
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var list = Enumerable.Range(1, 14).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)],
                PrecipitationByHour = Enumerable.Range(0, 24).Select(hour => new PrecipitationByHour
                {
                    Date = startDate.AddDays(index),
                    Hour = hour,
                    Chance = Random.Shared.Next(0, 100),
                }).ToList(),
            }).AsQueryable();

            //Act
            Expression<Func<IQueryable<WeatherForecast>, IQueryable<WeatherForecast>>> expression = l => l.Take(take);

            var serializer = new EfCoreExpressionSerializer(new JsonSerializer()); // Using EfCoreExpressionSerializer fails when ExpressionSerializer does not fail
            var value = serializer.SerializeText(expression); //{"__type":"LambdaExpressionNode:#Serialize.Linq.Nodes","NodeType":18,"Type":{"GenericArguments":[{"GenericArguments":[{"Name":"Serialize.Linq.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},{"GenericArguments":[{"Name":"Serialize.Linq.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"}],"Name":"System.Func`2"},"Body":{"__type":"MethodCallExpressionNode:#Serialize.Linq.Nodes","NodeType":6,"Type":{"GenericArguments":[{"Name":"Serialize.Linq.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},"Arguments":[{"__type":"ParameterExpressionNode:#Serialize.Linq.Nodes","NodeType":38,"Type":{"GenericArguments":[{"Name":"Serialize.Linq.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},"Name":"l"},{"__type":"ConstantExpressionNode:#Serialize.Linq.Nodes","NodeType":9,"Type":{"Name":"System.Int32"},"Value":2}],"Method":{"DeclaringType":{"Name":"System.Linq.Queryable"},"Signature":"System.Linq.IQueryable`1[TSource] Take[TSource](System.Linq.IQueryable`1[TSource], Int32)","GenericArguments":[{"Name":"Serialize.Linq.Tests.Issues.Issue175+WeatherForecast"}],"IsGenericMethod":true}},"Parameters":[{"__type":"ParameterExpressionNode:#Serialize.Linq.Nodes","NodeType":38,"Type":{"GenericArguments":[{"Name":"Serialize.Linq.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},"Name":"l"}]}
            var actualExpression = (Expression<Func<IQueryable<WeatherForecast>, IQueryable<WeatherForecast>>>)serializer.DeserializeText(value);
            var func = actualExpression.Compile();

            Assert.AreEqual(2, func(list).Count(), take);

        }



        public class EntityQueryRootExpressionNodeNodeFactory : NodeFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EntityQueryRootExpressionNodeNodeFactory"/> class.
            /// </summary>
            /// <param name="factorySettings">The settings to be used by the factory.</param>
            public EntityQueryRootExpressionNodeNodeFactory(FactorySettings? factorySettings) : base(factorySettings)
            {
            }

            /// <inheritdoc/>
            public override ExpressionNode Create(Expression expression)
            {
                if (expression is EntityQueryRootExpression entityQueryRootExpression) return new EntityQueryRootExpressionNode(this, entityQueryRootExpression);
                return base.Create(expression);
            }
        }
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

                if (context is SerializeExpressionContext seContext)
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

            public class EfCoreExpressionSerializer : ExpressionSerializer
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="EfCoreExpressionSerializer"/> class.
                /// </summary>
                /// <param name="serializer">The serializer to use.</param>
                /// <param name="factorySettings">The factory settings to use.</param>
                public EfCoreExpressionSerializer(ISerializer serializer, FactorySettings? factorySettings = null)
                    : base(serializer, factorySettings)
                {
                    serializer.AddKnownType(typeof(EntityQueryRootExpressionNode));
                    base.AddKnownType(typeof(EntityQueryRootExpressionNode));
                }

                /// <inheritdoc/>
                protected override INodeFactory CreateFactory(Expression expression, FactorySettings? factorySettings)
                    => new EntityQueryRootExpressionNodeNodeFactory(factorySettings);

            }

        }
        public class SerializeExpressionContext : ExpressionContext, IExpressionContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SerializeExpressionContext"/> class.
            /// </summary>
            /// <param name="dbContext">The database context.</param>
            public SerializeExpressionContext(DbContext dbContext)
                => this.DbContext = dbContext;

            /// <summary>
            /// Gets the database context.
            /// </summary>
            public DbContext DbContext { get; }
        }

#pragma warning disable SA1402 // File may only contain a single type
        /// <summary>
        ///     WeatherForecast.
        /// </summary>
        public class WeatherForecast
        {
            /// <summary>
            /// Gets or sets Date.
            /// </summary>
            [Key]
            public DateOnly Date { get; set; }

            /// <summary>
            /// Gets or sets TemperatureC.
            /// </summary>
            public int TemperatureC { get; set; }

            /// <summary>
            /// Gets or sets Summary.
            /// </summary>
            public string? Summary { get; set; }

            /// <summary>
            /// Gets TemperatureF.
            /// </summary>
            public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);

            /// <summary>
            /// Gets or sets PrecipitationByHour.
            /// </summary>
            [ForeignKey("Date")]
            public ICollection<PrecipitationByHour>? PrecipitationByHour { get; set; }
        }

        /// <summary>
        ///    PrecipitationByHour.
        /// </summary>
        [PrimaryKey(nameof(Date), nameof(Hour))]
        public class PrecipitationByHour
        {
            /// <summary>
            /// Gets or sets Date.
            /// </summary>
            public DateOnly Date { get; set; }

            /// <summary>
            /// Gets or sets Hour.
            /// </summary>
            public int Hour { get; set; }

            /// <summary>
            /// Gets or sets Chance.
            /// </summary>
            public int Chance { get; set; }
        }

    }
}
