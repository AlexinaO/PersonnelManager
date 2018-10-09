using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonnelManager.Business.Exceptions;
using PersonnelManager.Business.Services;
using PersonnelManager.Dal.Data;
using PersonnelManager.Dal.Entites;
using System;
using System.ComponentModel.DataAnnotations;

namespace PersonnelManager.Business.Tests
{
    [TestClass]
    public class ServiceEmployeTest
    {
        [TestMethod]
        public void ValiderNomEtPrenomRequis()
        {
            Assert.IsTrue(
                TestsHelper.HasAttribute<Employe, RequiredAttribute>(x => x.Nom));
            Assert.IsTrue(
                TestsHelper.HasAttribute<Employe, RequiredAttribute>(x => x.Prenom));
        }

        [TestMethod]
        public void DateEmbaucheOuvrierPosterieureA1920()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(1920, 12, 31),
                TauxHoraire = 12
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });

            Assert.AreEqual("La date d'embauche doit être > 1920",
                exception.Message);
        }

        [TestMethod]
        public void DateEmbaucheCadrePosterieureA1920()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerCadre(It.IsAny<Cadre>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var cadre = new Cadre
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(1920, 12, 31),
                SalaireMensuel = 1500
            };

            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
            });

            Assert.AreEqual("La date d'embauche doit être > 1920",
                exception.Message);
        }

        [TestMethod]
        public void DateEmbaucheCadreAnterieureAujourdhuiPlus3Mois()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            fauxDataEmploye.Setup(x => x.EnregistrerCadre(It.IsAny<Cadre>()));
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var cadre = new Cadre
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = (DateTime.Today).AddMonths(3)
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
            });

            Assert.AreEqual("La date d'embauche ne doit pas être au-delà de 3 mois",
                exception.Message);
        }

        [TestMethod]
        public void DateEmbaucheOuvrierAnterieureAujourdhuiPlus3Mois()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = (DateTime.Today).AddMonths(3),
                TauxHoraire = 12
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });
            Assert.AreEqual("La date d'embauche ne doit pas être au-delà de 3 mois",
                exception.Message);
        }

        [TestMethod]
        public void SalaireCadrePositif()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            fauxDataEmploye.Setup(x => x.EnregistrerCadre(It.IsAny<Cadre>()));
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var cadre = new Cadre
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = (DateTime.Today),
                SalaireMensuel = -2500
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
            });
            Assert.AreEqual("Le salaire d'un cadre ne peut pas être négatif", exception.Message);
        }

        [TestMethod]
        public void TauxHoraireOuvrierPositif()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = (DateTime.Today).AddMonths(3),
                TauxHoraire = -12
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });
            Assert.AreEqual("Le taux horaire d'un ouvrier ne peut pas être négatif", exception.Message);
        }

        [TestMethod]
        public void InterdireCaracteresSpeciauxDansNomEtPrenomCadre()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void InterdireCaracteresSpeciauxDansNomEtPrenomOuvrier()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void OuvrierEstNonNull()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            Assert.ThrowsException<InvalidOperationException>(
                () => serviceEmploye.EnregistrerOuvrier(null));
        }

        [TestMethod]
        public void CadreEstNonNull()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            Assert.ThrowsException<InvalidOperationException>(
                () => serviceEmploye.EnregistrerCadre(null));
        }
    }
}