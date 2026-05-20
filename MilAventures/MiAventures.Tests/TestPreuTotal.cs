using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MilAventures.Tests
{
    [TestClass]
    public class TestPreuTotal
    {
        /// <summary>
        /// Verifica que el càlcul del preu total d'una reserva és correcte.
        /// Preu total = preu per persona x nombre de participants.
        /// </summary>
        [TestMethod]
        public void CalcularPreuTotal_ParticipantsPerPreu_RetornaCorrect()
        {
            // Arrange
            decimal preuPerPersona = 50m;
            int participants = 4;

            // Act
            decimal total = preuPerPersona * participants;

            // Assert
            Assert.AreEqual(200m, total);
        }
    }
}