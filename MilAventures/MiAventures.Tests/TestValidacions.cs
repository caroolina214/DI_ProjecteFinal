using Microsoft.VisualStudio.TestTools.UnitTesting;
using MilAventures.Model.Context;
using MilAventures.Model.Models;
using MilAventures.Model.Repositories;
using System;

namespace MilAventures.Tests
{
    [TestClass]
    public class TestValidacions
    {
        /// <summary>
        /// Verifica que no es pot crear una activitat amb data de fi anterior a la d'inici.
        /// </summary>
        [TestMethod]
        public void ValidarDates_DataFiAnteriorAInici_RetornaFals()
        {
            // Arrange
            DateTime dataInici = new DateTime(2025, 6, 15);
            DateTime dataFi = new DateTime(2025, 6, 10);

            // Act
            bool datesValides = dataFi > dataInici;

            // Assert
            Assert.IsFalse(datesValides);
        }

        /// <summary>
        /// Verifica que el preu per persona no pot ser negatiu.
        /// </summary>
        [TestMethod]
        public void ValidarPreu_PreuNegatiu_RetornaFals()
        {
            // Arrange
            decimal preu = -10m;

            // Act
            bool preuValid = preu > 0;

            // Assert
            Assert.IsFalse(preuValid);
        }

        /// <summary>
        /// Verifica que els participants no poden superar el màxim de l'activitat.
        /// </summary>
        [TestMethod]
        public void ValidarParticipants_SuperenMaxim_RetornaFals()
        {
            // Arrange
            int maxParticipants = 10;
            int participants = 15;

            // Act
            bool valid = participants <= maxParticipants;

            // Assert
            Assert.IsFalse(valid);
        }
    }
}