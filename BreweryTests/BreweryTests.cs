using BreweryAPI.Models;
using BreweryAPI.Services;

namespace BreweryTests
{
    [TestClass]
    public sealed class BreweryTests
    {
        #region IsBeerStockSufficient
        [TestMethod]
        public void IsBeerStockSufficient_True()
        {
            var breweryStock = new BreweryStock{Amount = 10};
            var amountToBuy = 10;

            var result = BreweryService.IsBeerStockSufficient(breweryStock, amountToBuy);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsBeerStockSufficient_False()
        {
            var breweryStock = new BreweryStock{Amount = 10};
            var amountToBuy = 15;

            var result = BreweryService.IsBeerStockSufficient(breweryStock, amountToBuy);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsBeerStockSufficient_Null()
        {
            var amountToBuy = 10;

            var result = BreweryService.IsBeerStockSufficient(null, amountToBuy);

            Assert.IsFalse(result);
        }
        #endregion
    }
}
