﻿using System.Linq;
using Newtonsoft.Json;
using Xunit;
using Swashbuckle.AspNetCore.Swagger;

namespace Swashbuckle.AspNetCore.SwaggerGen.Test
{
    public class SwaggerResponseAttributeFilterTests
    {
        [Fact]
        public void Apply_SetsResponses_FromAttributes()
        {
            // Arrange
            var operation = new Operation { OperationId = "foobar" };
            var filterContext = FilterContextFor(nameof(FakeActions.AnnotatedWithSwaggerResponseAttributes));

            // Act
            this.Subject().Apply(operation, filterContext);

            // Assert
            var responses = operation.Responses;
            var response1 = responses["204"];
            Assert.Equal("No content is returned.", response1.Description);
            Assert.Null(response1.Schema);
            var response2 = responses["400"];
            Assert.Equal("This returns a dictionary.", response2.Description);
            Assert.NotNull(response2.Schema);
        }

        private OperationFilterContext FilterContextFor(
            string actionFixtureName,
            string controllerFixtureName = "NotAnnotated")
        {
            var fakeProvider = new FakeApiDescriptionGroupCollectionProvider();
            var apiDescription = fakeProvider
                .Add("GET", "collection", actionFixtureName, controllerFixtureName)
                .ApiDescriptionGroups.Items.First()
                .Items.First();

            return new OperationFilterContext(
                apiDescription,
                new SchemaRegistry(new JsonSerializerSettings()));
        }

        private SwaggerResponseAttributeFilter Subject()
        {
            return new SwaggerResponseAttributeFilter();
        }
    }
}