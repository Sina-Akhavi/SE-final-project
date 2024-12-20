using System.Collections.Generic;
using System.Linq;
using Xunit;
using Paradaim.BaseGateway.Boundary.Requests;
using Paradaim.BaseGateway.Boundary.Response;
using Paradaim.BaseGateway.Tests.Models;
using System.Linq;
using Xunit;
using FluentAssertions; // Ensure this line is present


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
        
    }
}
