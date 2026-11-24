using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductManagement.DTOs;
using Xunit;

namespace ProductManagement.Tests;

public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/product");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _client.GetAsync("/api/product/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var createDto = new ProductCreateDto
        {
            Name = "Test Product",
            Price = 100m,
            ImageUrls = new List<string> { "https://example.com/1.jpg" }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/product", createDto);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ProductReadDto>();

        var response = await _client.GetAsync($"/api/product/{created!.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenValid()
    {
        var dto = new ProductCreateDto
        {
            Name = "New Product",
            Price = 150m,
            ImageUrls = new List<string> { "https://example.com/a.jpg", "https://example.com/b.jpg" }
        };

        var response = await _client.PostAsJsonAsync("/api/product", dto);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenInvalid()
    {
        var dto = new ProductCreateDto
        {
            Name = "",
            Price = -10m,
            ImageUrls = new List<string>()
        };

        var response = await _client.PostAsJsonAsync("/api/product", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenExists()
    {
        var createDto = new ProductCreateDto
        {
            Name = "Before Update",
            Price = 50m,
            ImageUrls = new List<string> { "https://example.com/before.jpg" }
        };

        var createResponse = await _client.PostAsJsonAsync("/api/product", createDto);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ProductReadDto>();

        var updateDto = new ProductUpdateDto
        {
            Name = "After Update",
            Price = 75m,
            ImageUrls = new List<string> { "https://example.com/after.jpg" }
        };

        var response = await _client.PutAsJsonAsync($"/api/product/{created!.Id}", updateDto);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenNotExists()
    {
        var updateDto = new ProductUpdateDto
        {
            Name = "Missing",
            Price = 50m,
            ImageUrls = new List<string>()
        };

        var response = await _client.PutAsJsonAsync("/api/product/999999", updateDto);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenExists()
    {
        var createDto = new ProductCreateDto
        {
            Name = "To Delete",
            Price = 30m,
            ImageUrls = new List<string>()
        };

        var createResponse = await _client.PostAsJsonAsync("/api/product", createDto);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ProductReadDto>();

        var response = await _client.DeleteAsync($"/api/product/{created!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotExists()
    {
        var response = await _client.DeleteAsync("/api/product/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
