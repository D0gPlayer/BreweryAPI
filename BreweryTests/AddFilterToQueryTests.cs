using BreweryAPI.Data;
using BreweryAPI.Extensions;
using BreweryAPI.Models;
using BreweryAPI.Services;
using System;
using System.Linq;

namespace BreweryTests;

[TestClass]
public class AddFilterToQueryTests
{
    // Helper method to generate test data.
    private List<Beer> GetTestBeers()
    {
        // Create two breweries.
        Guid brewery1 = Guid.NewGuid();
        Guid brewery2 = Guid.NewGuid();

        return new List<Beer>
            {
                new Beer("Corona", 5.5f, brewery1),
                new Beer("Heineken", 4.5f, brewery2),
                new Beer("Guinness", 6.0f, brewery1),
                new Beer("Corona", 5.5f, brewery2)
            };
    }

    [TestMethod]
    public void Filter_By_Name_ReturnsCorrectResults()
    {
        // Arrange
        var beers = GetTestBeers().AsQueryable();
        var filters = new Dictionary<string, string> { { "Name", "Corona" } };

        // Act
        var result = beers.AddFilter(filters).ToList();

        // Assert
        Assert.AreEqual(2, result.Count, "Expected exactly 2 beers with Name 'Corona'.");
        foreach (var beer in result)
        {
            Assert.AreEqual("Corona", beer.Name);
        }
    }

    [TestMethod]
    public void Filter_By_Price_ReturnsCorrectResults()
    {
        // Arrange
        var beers = GetTestBeers().AsQueryable();
        // Price property is float, filtering by "5.5" should match two beers.
        var filters = new Dictionary<string, string> { { "Price", "5.5" } };

        // Act
        var result = beers.AddFilter(filters).ToList();

        // Assert
        Assert.AreEqual(2, result.Count, "Expected exactly 2 beers with Price 5.5.");
        foreach (var beer in result)
        {
            Assert.AreEqual(5.5f, beer.Price);
        }
    }

    [TestMethod]
    public void Filter_By_BreweryId_ReturnsCorrectResults()
    {
        // Arrange
        var beers = GetTestBeers();
        // Use the BreweryId from the first Beer for filtering.
        Guid targetBrewery = beers[0].BreweryId;
        var queryableBeers = beers.AsQueryable();
        var filters = new Dictionary<string, string> { { "BreweryId", targetBrewery.ToString() } };

        // Act
        var result = queryableBeers.AddFilter(filters).ToList();

        // Assert
        // There are two beers with BreweryId equal to targetBrewery.
        Assert.AreEqual(2, result.Count, "Expected exactly 2 beers with the specified BreweryId.");
        foreach (var beer in result)
        {
            Assert.AreEqual(targetBrewery, beer.BreweryId);
        }
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidDataException))]
    public void Filter_InvalidProperty_ThrowsInvalidDataException()
    {
        // Arrange
        var beers = GetTestBeers().AsQueryable();
        var filters = new Dictionary<string, string> { { "NonExistentProperty", "value" } };

        // Act - expect an exception when filtering on a property that does not exist.
        var result = beers.AddFilter(filters).ToList();
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidCastException))]
    public void Filter_InvalidGuidFormat_ThrowsInvalidCastException()
    {
        // Arrange
        var beers = GetTestBeers().AsQueryable();
        var filters = new Dictionary<string, string> { { "BreweryId", "not-a-guid" } };

        // Act - expect an exception when the BreweryId cannot be parsed as a Guid.
        var result = beers.AddFilter(filters).ToList();
    }
}

