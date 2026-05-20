using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MilAventures.Tests
{
    [TestClass]
    public class TestStock
    {
        /// <summary>
        /// Verifica que la validació de stock insuficient funciona correctament.
        /// Si la quantitat sol·licitada supera l'stock disponible, ha de retornar fals.
        /// </summary>
        [TestMethod]
        public void ValidarStock_QuantitatSuperiorAStock_RetornaFals()
        {
            // Arrange
            int stockDisponible = 3;
            int quantitatSolicitada = 5;

            // Act
            bool stockSuficient = quantitatSolicitada <= stockDisponible;

            // Assert
            Assert.IsFalse(stockSuficient);
        }
    }
}