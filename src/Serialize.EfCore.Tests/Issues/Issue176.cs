using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serialize.EfCore.Serializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;

namespace Serialize.EfCore.Tests.Issues
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

            var serializer = new ExpressionSerializer(new JsonSerializer()); // Using EfCoreExpressionSerializer fails when ExpressionSerializer does not fail

            var value = serializer.SerializeText(expression); //{"__type":"LambdaExpressionNode:#Serialize.EfCore.Nodes","NodeType":18,"Type":{"GenericArguments":[{"GenericArguments":[{"Name":"Serialize.EfCore.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},{"GenericArguments":[{"Name":"Serialize.EfCore.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"}],"Name":"System.Func`2"},"Body":{"__type":"MethodCallExpressionNode:#Serialize.EfCore.Nodes","NodeType":6,"Type":{"GenericArguments":[{"Name":"Serialize.EfCore.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},"Arguments":[{"__type":"ParameterExpressionNode:#Serialize.EfCore.Nodes","NodeType":38,"Type":{"GenericArguments":[{"Name":"Serialize.EfCore.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},"Name":"l"},{"__type":"ConstantExpressionNode:#Serialize.EfCore.Nodes","NodeType":9,"Type":{"Name":"System.Int32"},"Value":2}],"Method":{"DeclaringType":{"Name":"System.Linq.Queryable"},"Signature":"System.Linq.IQueryable`1[TSource] Take[TSource](System.Linq.IQueryable`1[TSource], Int32)","GenericArguments":[{"Name":"Serialize.EfCore.Tests.Issues.Issue175+WeatherForecast"}],"IsGenericMethod":true}},"Parameters":[{"__type":"ParameterExpressionNode:#Serialize.EfCore.Nodes","NodeType":38,"Type":{"GenericArguments":[{"Name":"Serialize.EfCore.Tests.Issues.Issue175+WeatherForecast"}],"Name":"System.Linq.IQueryable`1"},"Name":"l"}]}
            var actualExpression = (Expression<Func<IQueryable<WeatherForecast>, IQueryable<WeatherForecast>>>)serializer.DeserializeText(value);
            var func = actualExpression.Compile();

            Assert.AreEqual(2, func(list).Count(), take);

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
