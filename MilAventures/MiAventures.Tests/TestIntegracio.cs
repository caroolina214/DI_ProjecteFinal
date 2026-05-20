using Microsoft.VisualStudio.TestTools.UnitTesting;
using MilAventures.Model.Context;
using MilAventures.Model.Models;
using MilAventures.Model.Repositories;

namespace MilAventures.Tests
{
    [TestClass]
    public class TestIntegracio
    {
        /// <summary>
        /// Verifica que la connexió a la BD funciona i que es pot crear
        /// i recuperar una categoria correctament.
        /// </summary>
        [TestMethod]
        public void IntegracioBD_CrearIRecuperarCategoria_FuncionaCorrectament()
        {
            // Arrange
            var context = new MilAventuresContext();
            var repo = new CategoryRepository(context);
            var categoria = new Category
            {
                code = "TEST_INTEGRACIO",
                description = "Categoria creada pel test d'integració"
            };

            // Act
            repo.Add(categoria);
            var resultat = repo.GetById(categoria.id_category);

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("TEST_INTEGRACIO", resultat.code);

            // Cleanup
            repo.Delete(categoria.id_category);
        }
    }
}