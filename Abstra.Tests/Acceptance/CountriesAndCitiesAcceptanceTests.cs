using System.Net;
using System.Net.Http.Json;
using Abstra.Repository.Data;
using Abstra.Api;
using Abstra.Domain.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Abstra.Tests.Acceptance;

public class CountriesAndCitiesAcceptanceTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly AbstraDbContext _dbContext;
    private string? _authToken;

    public CountriesAndCitiesAcceptanceTests(WebApplicationFactory<Program> factory)
    {
        var databaseName = "AcceptanceTestDb_" + Guid.NewGuid();
        
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
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
        return _authToken;
    }

    [Fact]
    public async Task EndToEnd_ShouldCreateCountryAndCities()
    {
        // Arrange
        await GetAuthTokenAsync();

        // Step 1: Create a country
        var countryRequest = new CreateCountryRequest 
        { 
            Name = "United States", 
            Code = "USA" 
        };
        var countryResponse = await _client.PostAsJsonAsync("/api/countries", countryRequest);
        countryResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var country = await countryResponse.Content.ReadFromJsonAsync<CountryDto>();
        country.Should().NotBeNull();

        // Step 2: Create a state for the country
        var stateRequest = new CreateStateRequest 
        { 
            Name = "New York", 
            Code = "NY",
            CountryId = country!.Id
        };
        var stateResponse = await _client.PostAsJsonAsync("/api/states", stateRequest);
        if (stateResponse.StatusCode != HttpStatusCode.Created)
        {
            var errorContent = await stateResponse.Content.ReadAsStringAsync();
            throw new Exception($"State creation failed with status {stateResponse.StatusCode}: {errorContent}");
        }
        stateResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var state = await stateResponse.Content.ReadFromJsonAsync<StateDto>();
        state.Should().NotBeNull();

        // Step 3: Create cities for the state
        var city1Request = new CreateCityRequest 
        { 
            Name = "New York", 
            StateId = state!.Id, 
            Population = 8000000 
        };
        var city1Response = await _client.PostAsJsonAsync("/api/cities", city1Request);
        city1Response.StatusCode.Should().Be(HttpStatusCode.Created);
        var city1 = await city1Response.Content.ReadFromJsonAsync<CityDto>();
        city1.Should().NotBeNull();
        city1!.Name.Should().Be("New York");

        var city2Request = new CreateCityRequest 
        { 
            Name = "Buffalo", 
            StateId = state.Id, 
            Population = 4000000 
        };
        var city2Response = await _client.PostAsJsonAsync("/api/cities", city2Request);
        city2Response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Step 4: Get all cities for the state
        var citiesResponse = await _client.GetAsync($"/api/cities/state/{state.Id}");
        citiesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var cities = await citiesResponse.Content.ReadFromJsonAsync<List<CityDto>>();
        cities.Should().NotBeNull();
        cities!.Should().HaveCount(2);
        cities.Should().Contain(c => c.Name == "New York");
        cities.Should().Contain(c => c.Name == "Buffalo");

        // Step 5: Update a city
        var updateRequest = new UpdateCityRequest 
        { 
            Name = "New York City", 
            StateId = state.Id, 
            Population = 8500000 
        };
        var updateResponse = await _client.PutAsJsonAsync($"/api/cities/{city1.Id}", updateRequest);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedCity = await updateResponse.Content.ReadFromJsonAsync<CityDto>();
        updatedCity!.Name.Should().Be("New York City");
        updatedCity.Population.Should().Be(8500000);

        // Step 6: Delete a city
        var deleteResponse = await _client.DeleteAsync($"/api/cities/{city1.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Step 7: Verify city was deleted
        var getDeletedResponse = await _client.GetAsync($"/api/cities/{city1.Id}");
        getDeletedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

        // Step 8: Delete the state
        var deleteStateResponse = await _client.DeleteAsync($"/api/states/{state.Id}");
        deleteStateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Step 9: Delete the country
        var deleteCountryResponse = await _client.DeleteAsync($"/api/countries/{country.Id}");
        deleteCountryResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        _client.Dispose();
    }
}
