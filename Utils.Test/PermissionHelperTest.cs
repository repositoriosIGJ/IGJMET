using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Test
{
    [TestClass]
    public class PermissionHelperTest
    {
        List<string> indexRol = new List<string>() { "IGJ-AltaSociedadComercial-InspectorPreca-BPM", "IGJ-AltaSociedadComercial-AgenteCertificante-BPM" };

        [TestInitialize]
        public void Initialize()
        {
            PGroupItem item1 = new PGroupItem { Name = "Index", Action = "Index", Roles = indexRol };
            PGroupItem item2 = new PGroupItem { Name = "TramitePago", Action = "TramitePago", Roles = new List<string>() { "IGJ-AltaSociedadComercial-MesaEntradas-BPM" } };
            List<PGroupItem> items = new List<PGroupItem>();
            items.Add(item1);
            items.Add(item2);
            PGroup group = new PGroup { Name = "Usuarios-MET", ControllerName = "Home", Items = items };
            List<PGroup> groups = new List<PGroup>();
            groups.Add(group);
            PermissionStructure permission = new PermissionStructure {Groups = groups };
            PermissionHelper.Instance.Permission = permission;
        }

        [TestMethod]
        public void GetPermissionTest()
        {
            IList<string> roles = PermissionHelper.Instance.GetActionRoles("Home", "Index");

            Assert.IsTrue(roles.Any(r => indexRol.Contains(r)));
        }
    }
}
