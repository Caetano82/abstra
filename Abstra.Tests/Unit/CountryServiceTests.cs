using Abstra.Domain.Contracts;
using Abstra.Domain.DTOs;
using Abstra.Domain.Entities;
using Abstra.Application.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Abstra.Tests.Unit;

public class CountryServiceTests
{
    private readonly Mock<ICountryRepository> _repositoryMock;
    private readonly CountryService _service;

    public CountryServiceTests()
    {
        _repositoryMock = new Mock<ICountryRepository>();
        _service = new CountryService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCountries()
    {
        // Arrange
        var countries = new List<Country>
        {
            new() { Id = 1, Name = "United States", Code = "USA", CreatedAt = DateTime.UtcNow },
            new() { Id = 2, Name = "Canada", Code = "CAN", CreatedAt = DateTime.UtcNow }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(countries);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Code == "USA");
        result.Should().Contain(c => c.Code == "CAN");
    }

    [Fact]
    public async Task GetByIdAsync_WhenCountryExists_ShouldReturnCountry()
    {
        // Arrange
        var country = new Country { Id = 1, Name = "United States", Code = "USA", CreatedAt = DateTime.UtcNow };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(country);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("United States");
        result.Code.Should().Be("USA");
    }

    [Fact]
    public async Task GetByIdAsync_WhenCountryDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Country?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WhenCodeIsUnique_ShouldCreateCountry()
    {
        // Arrange
        var request = new CreateCountryRequest { Name = "United States", Code = "USA" };
        _repositoryMock.Setup(r => r.GetByCodeAsync("USA")).ReturnsAsync((Country?)null);
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Country>()))
            .ReturnsAsync((Country c) => c);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("United States");
        result.Code.Should().Be("USA");
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Country>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenCodeAlreadyExists_ShouldThrowException()
    {
        // Arrange
        var request = new CreateCountryRequest { Name = "United States", Code = "USA" };
        var existingCountry = new Country { Id = 1, Code = "USA" };
        _repositoryMock.Setup(r => r.GetByCodeAsync("USA")).ReturnsAsync(existingCountry);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_WhenCountryExists_ShouldUpdateCountry()
    {
        // Arrange
        var country = new Country { Id = 1, Name = "United States", Code = "USA", CreatedAt = DateTime.UtcNow };
        var request = new UpdateCountryRequest { Name = "United States of America", Code = "USA" };
        
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(country);
        _repositoryMock.Setup(r => r.GetByCodeAsync("USA")).ReturnsAsync(country);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Country>()))
            .ReturnsAsync((Country c) => c);

        // Act
        var result = await _service.UpdateAsync(1, request);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("United States of America");
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Country>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenCountryDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var request = new UpdateCountryRequest { Name = "United States", Code = "USA" };
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Country?)null);

        // Act
        var result = await _service.UpdateAsync(999, request);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenCountryExists_ShouldReturnTrue()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_WhenCountryDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
    }
}
