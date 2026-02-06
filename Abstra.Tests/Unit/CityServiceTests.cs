using Abstra.Domain.Contracts;
using Abstra.Domain.DTOs;
using Abstra.Domain.Entities;
using Abstra.Application.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Abstra.Tests.Unit;

public class CityServiceTests
{
    private readonly Mock<ICityRepository> _cityRepositoryMock;
    private readonly Mock<IStateRepository> _stateRepositoryMock;
    private readonly CityService _service;

    public CityServiceTests()
    {
        _cityRepositoryMock = new Mock<ICityRepository>();
        _stateRepositoryMock = new Mock<IStateRepository>();
        _service = new CityService(_cityRepositoryMock.Object, _stateRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WhenStateExists_ShouldCreateCity()
    {
        // Arrange
        var request = new CreateCityRequest { Name = "New York", StateId = 1, Population = 8000000 };
        var country = new Country { Id = 1, Name = "United States", Code = "USA" };
        var state = new State { Id = 1, Name = "New York", Code = "NY", CountryId = 1, Country = country };
        var city = new City { Id = 1, Name = "New York", StateId = 1, Population = 8000000, State = state };

        _stateRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(state);
        _cityRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<City>()))
            .ReturnsAsync((City c) => 
            {
                c.Id = 1;
                c.State = state;
                return c;
            });

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("New York");
        result.StateId.Should().Be(1);
        result.Population.Should().Be(8000000);
    }

    [Fact]
    public async Task CreateAsync_WhenStateDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var request = new CreateCityRequest { Name = "New York", StateId = 999 };
        _stateRepositoryMock.Setup(r => r.ExistsAsync(999)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task GetByStateIdAsync_ShouldReturnCitiesForState()
    {
        // Arrange
        var country = new Country { Id = 1, Name = "United States", Code = "USA" };
        var state = new State { Id = 1, Name = "New York", Code = "NY", CountryId = 1, Country = country };
        var cities = new List<City>
        {
            new() { Id = 1, Name = "New York", StateId = 1, State = state },
            new() { Id = 2, Name = "Buffalo", StateId = 1, State = state }
        };

        _cityRepositoryMock.Setup(r => r.GetByStateIdAsync(1)).ReturnsAsync(cities);

        // Act
        var result = await _service.GetByStateIdAsync(1);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(c => c.Name == "New York");
        result.Should().Contain(c => c.Name == "Buffalo");
    }
}
