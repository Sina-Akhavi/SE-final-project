using System.Collections.Generic;
using System.Linq;
using Xunit;
using Paradaim.BaseGateway.Boundary.Requests;
using FluentAssertions;
using Paradaim.BaseGateway.Tests.Models;



namespace Paradaim.BaseGateway.Tests
{
    public class ApiResponseTests
    {
        /// <summary>
        /// Tests that ApiResponse correctly sorts the data in descending order based on the specified column.
        /// </summary>
        [Fact]
        public void ApiResponse_ShouldSortDataDescendingByPrice()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Name = "Product A", Price = 10.0m },
                new Product { Name = "Product B", Price = 30.0m },
                new Product { Name = "Product C", Price = 20.0m }
            }.AsQueryable();

            var sortRequest = new SortRequest("Price", ESortType.Desc);

            // Act
            var apiResponse = new ApiResponse<Product>(products, sortRequest);

            // Assert
            var expectedOrder = products.OrderByDescending(p => p.Price).ToList();
            Assert.Equal(expectedOrder.Select(p => p.Price), apiResponse.Data.Select(p => p.Price));
        }


         private List<Item> GetSampleData()
        {
            return new List<Item>
            {
                new Item { Id = 1, Name = "Alpha" },
                new Item { Id = 2, Name = "Bravo" },
                new Item { Id = 3, Name = "Charlie" },
                new Item { Id = 4, Name = "Delta" },
                new Item { Id = 5, Name = "Echo" }
            };
        }


        [Fact]
        public void Constructor_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            var data = GetSampleData().AsQueryable();
            var pagination = new PaginationRequest
            {
                PageNumber = 2,
                PageSize = 2
            };
            var sortRequests = new SortRequest[]
            {
                new SortRequest { ColName = "Id", SortType = ESortType.Asc }
            };

            // Act
            var response = new ApiResponse<Item>(data, pagination, sortRequests);

            // Assert
            response.TotalNumber.Should().Be(5);
            response.Data.Should().HaveCount(2);
            response.Data.Select(d => d.Id).Should().ContainInOrder(3, 4);
            response.PaginationRequest.Should().BeEquivalentTo(pagination);
        }


        [Fact]
        public void OrderBy_SingleExpression_ShouldSortCorrectly()
        {
            // Arrange
            var data = new List<TestModel>
            {
                new TestModel { Id = 3, Name = "Charlie" },
                new TestModel { Id = 1, Name = "Alice" },
                new TestModel { Id = 2, Name = "Bob" }
            };

            var response = new ApiResponse<TestModel>(data);

            // Act
            response.OrderBy(x => x.Id);

            // Assert
            var expected = data.OrderBy(x => x.Id).ToList();
            Assert.Equal(expected, response.Data);
        }


        [Fact]
        public void Constructor_WithSortRequest_ShouldSortDataCorrectly()
        {
            // Arrange
            var data = new List<TestModel>
            {
                new TestModel { Id = 3, Name = "Charlie" },
                new TestModel { Id = 1, Name = "Alice" },
                new TestModel { Id = 2, Name = "Bob" }
            }.AsQueryable();

            var sortRequest = new SortRequest
            {
                ColName = nameof(TestModel.Id),
                SortType = ESortType.Asc
            };

            // Act
            var response = new ApiResponse<TestModel>(data, sortRequest);

            // Assert
            Assert.Equal(3, response.TotalNumber); // Total number of items
            var expectedData = data.OrderBy(x => x.Id).ToList();
            Assert.Equal(expectedData, response.Data);
        }

        [Fact]
        public void Constructor_WithPaginationAndSortRequest_ShouldPaginateAndSortDataCorrectly()
        {
            // Arrange
            var data = new List<TestModel>
            {
                new TestModel { Id = 3, Name = "Charlie" },
                new TestModel { Id = 1, Name = "Alice" },
                new TestModel { Id = 2, Name = "Bob" },
                new TestModel { Id = 4, Name = "Dave" }
            }.AsQueryable();

            var paginationRequest = new PaginationRequest
            {
                PageNumber = 2,
                PageSize = 2
            };

            var sortRequest = new SortRequest
            {
                ColName = nameof(TestModel.Id),
                SortType = ESortType.Asc
            };

            // Act
            var response = new ApiResponse<TestModel>(data, paginationRequest, sortRequest);

            // Assert
            Assert.Equal(4, response.TotalNumber); // Total number of items
            Assert.Equal(2, response.Data.Count);  // Page size
            Assert.Equal(new[] { 3, 4 }, response.Data.Select(x => x.Id)); // Correct paginated data
            Assert.Equal(paginationRequest, response.PaginationRequest); // Pagination request is set
        }
        

        [Fact]
        public void Constructor_WithIEnumerable_ShouldInitializeCorrectly()
        {
            // Arrange
            var data = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Alice" },
                new TestModel { Id = 2, Name = "Bob" },
                new TestModel { Id = 3, Name = "Charlie" }
            };

            // Act
            var response = new ApiResponse<TestModel>(data);

            // Assert
            Assert.Equal(data.Count, response.TotalNumber); // Verify TotalNumber is correct
            Assert.Equal(data, response.Data); // Verify Data is correctly initialized
        }

        [Fact]
        public void Order_SingleSortRequest_Ascending_ShouldSortData()
        {
            // Arrange
            var data = new List<TestModel>
            {
                new TestModel { Id = 3, Name = "Charlie" },
                new TestModel { Id = 1, Name = "Alice" },
                new TestModel { Id = 2, Name = "Bob" }
            }.AsQueryable();

            var sortRequests = new[]
            {
                new SortRequest { ColName = nameof(TestModel.Id), SortType = ESortType.Asc }
            };

            // Act
            var response = new ApiResponse<TestModel>(data, sortRequests);

            // Assert
            var expected = data.OrderBy(x => x.Id).ToList();
            Assert.Equal(expected, response.Data);
        }

        [Fact]
        public void Order_SingleSortRequest_Descending_ShouldSortData()
        {
            // Arrange
            var data = new List<TestModel>
            {
                new TestModel { Id = 3, Name = "Charlie" },
                new TestModel { Id = 1, Name = "Alice" },
                new TestModel { Id = 2, Name = "Bob" }
            }.AsQueryable();

            var sortRequests = new[]
            {
                new SortRequest { ColName = nameof(TestModel.Id), SortType = ESortType.Desc }
            };

            var response = new ApiResponse<TestModel>(data, sortRequests);

            var expected = data.OrderByDescending(x => x.Id).ToList();
            Assert.Equal(expected, response.Data);
        }

    }
}
