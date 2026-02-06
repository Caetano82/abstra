using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Abstra.Repository.Data;
using Abstra.Api;
using Abstra.Domain.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Abstra.Tests.Integration;

public class CountriesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly AbstraDbContext _dbContext;
    private string? _authToken;

    public CountriesControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        var databaseName = "TestDb_" + Guid.NewGuid();
        
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AbstraDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AbstraDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName)
                           .EnableSensitiveDataLogging()
                           .UseInternalServiceProvider(null);
                });
            });
        });

        _client = _factory.CreateClient();
        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AbstraDbContext>();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        if (_authToken != null)
            return _authToken;

        var loginRequest = new LoginRequest { Username = "testuser", Password = "testpass" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        _authToken = loginResponse!.Token;
        return _authToken;
    }

    [Fact]
    public async Task GetAll_WithoutAuth_ShouldReturnUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/countries");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_WithAuth_ShouldReturnCountries()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.GetAsync("/api/countries");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var countries = await response.Content.ReadFromJsonAsync<List<CountryDto>>();
        countries.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_WithValidData_ShouldCreateCountry()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new CreateCountryRequest { Name = "Test Country", Code = "TST" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/countries", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var country = await response.Content.ReadFromJsonAsync<CountryDto>();
        country.Should().NotBeNull();
        country!.Name.Should().Be("Test Country");
        country.Code.Should().Be("TST");
    }

    [Fact]
    public async Task Create_WithDuplicateCode_ShouldReturnBadRequest()
    {
        // Arrange
        var token = await GetAuthTokenAsync();
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var request = new CreateCountryRequest { Name = "Test Country", Code = "DUP" };
        var firstResponse = await _client.PostAsJsonAsync("/api/countries", request);
        firstResponse.EnsureSuccessStatusCode();

        // Act
        var response = await _client.PostAsJsonAsync("/api/countries", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        _client.Dispose();
    }
}

