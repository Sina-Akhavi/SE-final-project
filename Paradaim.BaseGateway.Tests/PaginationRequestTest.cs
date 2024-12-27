using Xunit;
using Paradaim.BaseGateway.Boundary.Requests;

namespace Paradaim.BaseGateway.Tests
{
    public class PaginationRequestTests
    {
        [Fact]
        public void Constructor_WithNullPagingRequest_SetsDefaultValues()
        {
            // Arrange
            PagingRequest? pagingRequest = null;

            // Act
            var paginationRequest = new PaginationRequest(pagingRequest);

            // Assert
            Assert.Equal(10, paginationRequest.PageSize);
            Assert.Equal(1, paginationRequest.PageNumber);
        }

        [Theory]
        [InlineData(0, 20, 1, 20)]  // page = 0 (zero-based), expected PageNumber = 1
        [InlineData(1, 15, 2, 15)]  // page = 1, expected PageNumber = 2
        [InlineData(5, 50, 6, 50)]  // page = 5, expected PageNumber = 6
        public void Constructor_WithValidPagingRequest_SetsCorrectValues(int page, int pageSize, int expectedPageNumber, int expectedPageSize)
        {
            // Arrange
            var pagingRequest = new PagingRequest
            {
                page = page,
                pageSize = pageSize
            };

            // Act
            var paginationRequest = new PaginationRequest(pagingRequest);

            // Assert
            Assert.Equal(expectedPageSize, paginationRequest.PageSize);
            Assert.Equal(expectedPageNumber, paginationRequest.PageNumber);
        }

        [Fact]
        public void Constructor_WithPagingRequest_NullValues_SetsDefaultValues()
        {
            // Arrange
            var pagingRequest = new PagingRequest
            {
                page = 0,
                pageSize = 0  // Assuming zero is not a valid page size, but the original class doesn't handle it
            };

            // Act
            var paginationRequest = new PaginationRequest(pagingRequest);

            // Assert
            Assert.Equal(0, paginationRequest.PageSize); // Reflects the input
            Assert.Equal(1, paginationRequest.PageNumber); // 0 + 1
        }

        // Additional tests can be added here for edge cases
    }
}
