using Class_Lib.Backend.Services;
using Class_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject_TestProject
{
    [TestClass]
    public class AccessServiceTests
    {
        [DataTestMethod]
        // PACKAGE
        [DataRow("ReadPackage", typeof(Employee), true)]
        [DataRow("CreatePackage", typeof(Employee), true)]
        [DataRow("UpdatePackage", typeof(Employee), false)]
        [DataRow("DeletePackage", typeof(Employee), false)]

        [DataRow("ReadPackage", typeof(Manager), true)]
        [DataRow("CreatePackage", typeof(Manager), true)]
        [DataRow("UpdatePackage", typeof(Manager), true)]
        [DataRow("DeletePackage", typeof(Manager), true)]

        [DataRow("ReadPackage", typeof(Administrator), true)]
        [DataRow("CreatePackage", typeof(Administrator), true)]
        [DataRow("UpdatePackage", typeof(Administrator), true)]
        [DataRow("DeletePackage", typeof(Administrator), true)]

        // EVENT
        [DataRow("ReadEvent", typeof(Employee), true)]
        [DataRow("CreateEvent", typeof(Employee), true)]
        [DataRow("UpdateEvent", typeof(Employee), false)]
        [DataRow("DeleteEvent", typeof(Employee), false)]

        [DataRow("ReadEvent", typeof(Manager), true)]
        [DataRow("CreateEvent", typeof(Manager), true)]
        [DataRow("UpdateEvent", typeof(Manager), true)]
        [DataRow("DeleteEvent", typeof(Manager), false)]

        [DataRow("ReadEvent", typeof(Administrator), true)]
        [DataRow("CreateEvent", typeof(Administrator), true)]
        [DataRow("UpdateEvent", typeof(Administrator), true)]
        [DataRow("DeleteEvent", typeof(Administrator), true)]


        // CONTENT
        [DataRow("ReadContent", typeof(Employee), true)]
        [DataRow("CreateContent", typeof(Employee), true)]
        [DataRow("UpdateContent", typeof(Employee), true)]
        [DataRow("DeleteContent", typeof(Employee), false)]

        [DataRow("ReadContent", typeof(Manager), true)]
        [DataRow("CreateContent", typeof(Manager), true)]
        [DataRow("UpdateContent", typeof(Manager), true)]
        [DataRow("DeleteContent", typeof(Manager), true)]

        [DataRow("ReadContent", typeof(Administrator), true)]
        [DataRow("CreateContent", typeof(Administrator), true)]
        [DataRow("UpdateContent", typeof(Administrator), true)]
        [DataRow("DeleteContent", typeof(Administrator), true)]


        // PERSON
        [DataRow("ReadPerson", typeof(Employee), true)]
        [DataRow("CreatePerson", typeof(Employee), false)]
        [DataRow("UpdatePerson", typeof(Employee), false)]
        [DataRow("DeletePerson", typeof(Employee), false)]

        [DataRow("ReadPerson", typeof(Manager), true)]
        [DataRow("CreatePerson", typeof(Manager), true)]
        [DataRow("UpdatePerson", typeof(Manager), true)]
        [DataRow("DeletePerson", typeof(Manager), true)]

        [DataRow("ReadPerson", typeof(Administrator), true)]
        [DataRow("CreatePerson", typeof(Administrator), true)]
        [DataRow("UpdatePerson", typeof(Administrator), true)]
        [DataRow("DeletePerson", typeof(Administrator), true)]


        // LOCATION
        [DataRow("ReadLocation", typeof(Employee), true)]
        [DataRow("CreateLocation", typeof(Employee), false)]
        [DataRow("UpdateLocation", typeof(Employee), false)]
        [DataRow("DeleteLocation", typeof(Employee), false)]

        [DataRow("ReadLocation", typeof(Manager), true)]
        [DataRow("CreateLocation", typeof(Manager), true)]
        [DataRow("UpdateLocation", typeof(Manager), true)]
        [DataRow("DeleteLocation", typeof(Manager), true)]

        [DataRow("ReadLocation", typeof(Administrator), true)]
        [DataRow("CreateLocation", typeof(Administrator), true)]
        [DataRow("UpdateLocation", typeof(Administrator), true)]
        [DataRow("DeleteLocation", typeof(Administrator), true)]


        // REPORT
        [DataRow("ReadReport", typeof(Employee), true)]
        [DataRow("CreateReport", typeof(Employee), false)]
        [DataRow("UpdateReport", typeof(Employee), false)]
        [DataRow("DeleteReport", typeof(Employee), false)]

        [DataRow("ReadReport", typeof(Manager), true)]
        [DataRow("CreateReport", typeof(Manager), true)]
        [DataRow("UpdateReport", typeof(Manager), true)]
        [DataRow("DeleteReport", typeof(Manager), true)]

        [DataRow("ReadReport", typeof(Administrator), true)]
        [DataRow("CreateReport", typeof(Administrator), true)]
        [DataRow("UpdateReport", typeof(Administrator), true)]
        [DataRow("DeleteReport", typeof(Administrator), true)]

        // USER
        [DataRow("ReadUser", typeof(Employee), false)]
        [DataRow("CreateUser", typeof(Employee), false)]
        [DataRow("UpdateUser", typeof(Employee), false)]
        [DataRow("DeleteUser", typeof(Employee), false)]

        [DataRow("ReadUser", typeof(Manager), false)]
        [DataRow("CreateUser", typeof(Manager), false)]
        [DataRow("UpdateUser", typeof(Manager), false)]
        [DataRow("DeleteUser", typeof(Manager), false)]

        [DataRow("ReadUser", typeof(Administrator), true)]
        [DataRow("CreateUser", typeof(Administrator), true)]
        [DataRow("UpdateUser", typeof(Administrator), true)]
        [DataRow("DeleteUser", typeof(Administrator), true)]

        public void CanPerformAction_Permissions(string action, Type roleType, bool expected)
        {
            var result = AccessService.CanPerformAction(roleType, action);
            Assert.AreEqual(expected, result, $"Role: {roleType.Name}, Action: {action}");
        }
    }
}
