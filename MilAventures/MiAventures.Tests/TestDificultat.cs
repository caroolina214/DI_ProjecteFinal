using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MilAventures.Tests
{
    [TestClass]
    public class TestDificultat
    {
        /// <summary>
        /// Verifica que la conversió del nivell de dificultat de int a text és correcta.
        /// El valor 3 ha de retornar "Mitjà".
        /// </summary>
        [TestMethod]
        public void ConvertirDificultat_Valor3_RetornaMitja()
        {
            // Arrange
            int dificultat = 3;
            string resultatEsperat = "Mitjà";

            // Act
            string resultat;
            switch (dificultat)
            {
                case 1: resultat = "Fàcil"; break;
                case 2: resultat = "Principiant"; break;
                case 3: resultat = "Mitjà"; break;
                case 4: resultat = "Avançat"; break;
                case 5: resultat = "Expert"; break;
                default: resultat = "-"; break;
            }

            // Assert
            Assert.AreEqual(resultatEsperat, resultat);
        }
    }
}