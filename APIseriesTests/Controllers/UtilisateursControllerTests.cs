using APIseries.Controllers;
using APIseries.Models.DataManager;
using APIseries.Models.EntityFramework;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;

namespace APIseries.Controllers.Tests
{
    [TestClass]
    public class UtilisateursControllerTests
    {
        private UtilisateursController controller;
        private SeriesDbContext context;
        private IDataRepository<Utilisateur> dataRepository;
        private Mock<IMapper> mockMapper;

        public UtilisateursControllerTests()
        {
            var builder = new DbContextOptionsBuilder<SeriesDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=APIseries;Username=postgres;Password=postgres");
            context = new SeriesDbContext(builder.Options);
            dataRepository = new UtilisateurManager(context);

            // Création du mock IMapper pour les tests avec le vrai repository
            mockMapper = new Mock<IMapper>();
            controller = new UtilisateursController(dataRepository, mockMapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.EntityFrameworkCore.DbUpdateException))]
        public void PostUtilisateur_DuplicateMail_ThrowsException()
        {
            var mail = "duplicationtest@gmail.com";
            var user1 = new Utilisateur { Nom = "Duplication", Prenom = "Test", Mail = mail, Pwd = "info" };
            context.Utilisateurs.Add(user1);
            context.SaveChanges();

            var user2 = new Utilisateur { Nom = "Duplication2", Prenom = "Test2", Mail = mail, Pwd = "info" };
            var result = controller.PostUtilisateur(user2).Result;
        }

        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void PostUtilisateur_MissingRequiredName_ThrowsException()
        {
            var user = new Utilisateur { Prenom = "Test", Mail = "missingname@gmail.com", Pwd = "info" };
            var result = controller.PostUtilisateur(user).Result;
        }

        [TestMethod]
        public void PostUtilisateur_InvalidMobile_AddModelError_ModelNotValid()
        {
            Utilisateur utilisateur = new Utilisateur()
            {
                Nom = "Test",
                Prenom = "Luc",
                Mobile = "1", // Non conforme à la regex
                Mail = "invalidmobile@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Rue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France"
            };

            string PhoneRegex = @"^0[0-9]{9}$";
            Regex regex = new Regex(PhoneRegex);
            if (!regex.IsMatch(utilisateur.Mobile))
            {
                controller.ModelState.AddModelError("Mobile", "Le numéro de mobile doit contenir 10 chiffres");
            }

            var result = controller.PostUtilisateur(utilisateur).Result;

            Assert.IsInstanceOfType(result.Result, typeof(Microsoft.AspNetCore.Mvc.BadRequestObjectResult));
            var userRecup = context.Utilisateurs.Where(u => u.Mail == utilisateur.Mail).FirstOrDefault();
            Assert.IsNull(userRecup);
        }

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK()
        {
            // Arrange
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc on concatène un random
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };

            // Act
            var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone

            // Assert
            Utilisateur? userRecupere = context.Utilisateurs
                .Where(u => u.Mail.ToUpper() == userAtester.Mail.ToUpper())
                .FirstOrDefault();

            Assert.IsNotNull(userRecupere, "L'utilisateur n'a pas été trouvé en base après création.");

            // Récupère l'id généré par la BD
            userAtester.UtilisateurId = userRecupere!.UtilisateurId;

            // Compare les champs essentiels plutôt que l'égalité d'objet (qui n'est pas surchargée)
            Assert.AreEqual(userAtester.UtilisateurId, userRecupere.UtilisateurId, "Ids différents");
            Assert.AreEqual(userAtester.Nom, userRecupere.Nom, "Nom différent");
            Assert.AreEqual(userAtester.Prenom, userRecupere.Prenom, "Prenom différent");
            Assert.AreEqual(userAtester.Mail.ToUpper(), userRecupere.Mail.ToUpper(), "Mail différent");
            Assert.AreEqual(userAtester.Mobile, userRecupere.Mobile, "Mobile différent");
            Assert.AreEqual(userAtester.Rue, userRecupere.Rue, "Rue différente");
            Assert.AreEqual(userAtester.CodePostal, userRecupere.CodePostal, "CodePostal différent");
            Assert.AreEqual(userAtester.Ville, userRecupere.Ville, "Ville différente");
            Assert.AreEqual(userAtester.Pays, userRecupere.Pays, "Pays différent");
            // Note : mot de passe (Pwd) peut être stocké/transformé différemment si tu as du hashing; adapte l'assertion si nécessaire.
        }

        [TestMethod]
        public void Postutilisateur_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mockMapperLocal = new Mock<IMapper>();
            var userController = new UtilisateursController(mockRepository.Object, mockMapperLocal.Object);

            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act
            var actionResult = userController.PostUtilisateur(user).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
            user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
            Assert.AreEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
        }

        [TestMethod]
        public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mockMapperLocal = new Mock<IMapper>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object, mockMapperLocal.Object);
            // Act
            var actionResult = userController.GetUtilisateurById(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value as Utilisateur);
        }

        //Test du GetUtilisateurById avec ID inconnu :
        [TestMethod]
        public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mockMapperLocal = new Mock<IMapper>();
            var userController = new UtilisateursController(mockRepository.Object, mockMapperLocal.Object);
            // Act
            var actionResult = userController.GetUtilisateurById(0).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetUtilisateurByEmail_WithMoq_Found()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mockMapperLocal = new Mock<IMapper>();
            var email = "poisson@example.com";
            var user = new Utilisateur
            {
                UtilisateurId = 42,
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "0600000000",
                Mail = email,
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France"
            };

            mockRepository
                .Setup(r => r.GetByStringAsync(It.Is<string>(s => s.ToUpper() == email.ToUpper())))
                .ReturnsAsync(new ActionResult<Utilisateur>(user));

            var userController = new UtilisateursController(mockRepository.Object, mockMapperLocal.Object);

            // Act
            var actionResult = await userController.GetUtilisateurByEmail(email);

            // Assert - la valeur doit être retournée dans .Value
            Assert.IsNotNull(actionResult, "ActionResult est null");
            Assert.IsNotNull(actionResult.Value, "La valeur retournée doit être un utilisateur");
            Assert.AreEqual(user.UtilisateurId, actionResult.Value.UtilisateurId);
            Assert.AreEqual(user.Nom, actionResult.Value.Nom);
            Assert.AreEqual(user.Mail.ToUpper(), actionResult.Value.Mail.ToUpper());
        }

        [TestMethod]
        public async Task GetUtilisateurByEmail_WithMoq_NotFound()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mockMapperLocal = new Mock<IMapper>();
            var emailInexistant = "inconnu@example.com";

            // Simuler un retour null depuis le repository (le controller vérifie 'if (utilisateur == null)')
            mockRepository
                .Setup(r => r.GetByStringAsync(It.Is<string>(s => s.ToUpper() == emailInexistant.ToUpper())))
                .ReturnsAsync((ActionResult<Utilisateur>)null);

            var userController = new UtilisateursController(mockRepository.Object, mockMapperLocal.Object);

            // Act
            var actionResult = await userController.GetUtilisateurByEmail(emailInexistant);

            // Assert - le controller doit renvoyer NotFoundResult via actionResult.Result
            Assert.IsNull(actionResult.Value, "La valeur doit être nulle pour un utilisateur introuvable");
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult), "Doit retourner NotFoundResult quand le repo renvoie null");
        }

        [TestMethod]
        public void DeleteUtilisateurTest_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mockMapperLocal = new Mock<IMapper>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(user);
            var userController = new UtilisateursController(mockRepository.Object, mockMapperLocal.Object);
            // Act
            var actionResult = userController.DeleteUtilisateur(1).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        [TestMethod]
        public async Task PutUtilisateur_AvecMoq_ExistingUser_UpdatesAndReturnsNoContent()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mockMapperLocal = new Mock<IMapper>();

            // utilisateur présent en base (valeurs avant modification)
            var existingUser = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France"
            };

            // même utilisateur mais avec des données modifiées (valeurs envoyées par le client)
            var modifiedUser = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida_Modified",
                Prenom = "Lilley_Modified",
                Mobile = "0600000000",
                Mail = "clilleymd@last.fm", // on garde le mail identique pour l'exemple
                Pwd = "NouveauPwd123!",
                Rue = "Nouvelle Rue",
                CodePostal = "74000",
                Ville = "VilleModif",
                Pays = "France"
            };

            // Mock : GetByIdAsync doit renvoyer l'utilisateur existant (ActionResult wrapper)
            mockRepository
                .Setup(r => r.GetByIdAsync(It.Is<int>(i => i == existingUser.UtilisateurId)))
                .ReturnsAsync(new ActionResult<Utilisateur>(existingUser));

            // Mock : UpdateAsync doit être appelé ; on retourne une task complétée
            mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Utilisateur>()))
                .Returns(Task.CompletedTask);

            var userController = new UtilisateursController(mockRepository.Object, mockMapperLocal.Object);

            // Act
            var actionResult = await userController.PutUtilisateur(modifiedUser.UtilisateurId, modifiedUser);

            // Assert
            // Le controller doit renvoyer NoContent pour une mise à jour réussie
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));

            // Vérifier que UpdateAsync a été appelé une fois avec l'utilisateur modifié (id + champ modifié)
            mockRepository.Verify(
                r => r.UpdateAsync(It.Is<Utilisateur>(u =>
                    u.UtilisateurId == modifiedUser.UtilisateurId &&
                    u.Nom == modifiedUser.Nom &&
                    u.Prenom == modifiedUser.Prenom &&
                    u.Mobile == modifiedUser.Mobile
                )),
                Times.Once
            );
        }

        //[TestMethod]
        //public async Task PatchUtilisateur_ValidPatch_UpdatesProperty()
        //{
        //    var user = new Utilisateur
        //    {
        //        Nom = "Randolph",
        //        Prenom = "Richings",
        //        Mail = "rrichings1@naver.com",
        //        Pwd = "bif02sy2003",
        //        Rue = "Route",
        //        CodePostal = "74200",
        //        Ville = "Bons",
        //        Pays = "France"
        //    };
        //    context.Utilisateurs.Add(user);
        //    context.SaveChanges();

        //    var patchDoc = new JsonPatchDocument<Utilisateur>();
        //    patchDoc.Replace(u => u.Prenom, "Vincent");

        //    var result = await controller.PatchUtilisateur(user.UtilisateurId, patchDoc);

        //    Assert.AreEqual("Vincent", result.Value.Prenom);
        //}
    }
}