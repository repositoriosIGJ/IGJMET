using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Utils
{
    public class PermissionHelper 
    {
        private readonly string _resourcePath;
        private PermissionStructure _permissionStructure;
        private readonly object _sync;
        private DateTime _lastDate = DateTime.MinValue;

        public PermissionStructure Permission
        {
            get { return GetPermissionStructure(); }

            set { Serialize(value); }
        }

        #region Singleton
        private static PermissionHelper _instance;

        public static PermissionHelper Instance
        {
            get 
            {
                if (_instance == null)
                    _instance = new PermissionHelper();
                return _instance;
            }
        }
        #endregion

        public PermissionHelper()
        {
            _sync = new object();
            _resourcePath = HttpContext.Current.Server.MapPath("~/App_Data/Permisos.xml");
            //_resourcePath = "C:\\Dev\\Permisos.xml";
        }


        public IList<string> GetActionRoles(string controllerName, string actionName)
        {
            var group = Permission.Groups.SingleOrDefault(g => g.ControllerName == controllerName);

            if (group != null)
            {
                var action = group.Items.SingleOrDefault(g => g.Action.ToLower() == actionName.ToLower());

                if (action != null)
                    return action.Roles;
            }

            return new List<string>();
        }

        private bool IsExpired()
        {
            return _lastDate.AddHours(1) < DateTime.Now;
        }

        private PermissionStructure GetPermissionStructure()
        {
            lock (_sync)
            {
                if (IsExpired())
                {
                    _permissionStructure = Deserialize();
                    _lastDate = DateTime.Now;
                }
                return _permissionStructure;
            }
        }

        private void Serialize(PermissionStructure permission)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_resourcePath))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(PermissionStructure));
                    ser.Serialize(writer, permission);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private PermissionStructure Deserialize()
        {
            try
            {
                using (StreamReader reader = new StreamReader(_resourcePath))
                { 
                    XmlSerializer ser = new XmlSerializer(typeof(PermissionStructure));
                    return (PermissionStructure)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    
    [Serializable]
    [XmlInclude(typeof(PGroup))]
    public class PermissionStructure
    {
        public PermissionStructure()
        {
            Groups = new List<PGroup>();
        }

        [XmlElement("Group")]
        public List<PGroup> Groups { get; set; }
    }

    [Serializable]
    [XmlInclude(typeof(PGroupItem))]
    public class PGroup
    {
        public PGroup()
        {
            Items = new List<PGroupItem>();
        }

        [XmlAttribute("Name")]
        public string Name { get; set; }
        
        [XmlAttribute("Controller")]
        public string ControllerName { get; set; }

        [XmlElement("Item")]
        public List<PGroupItem> Items { get; set; }
    }

    [Serializable]
    public class PGroupItem
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Action")]
        public string Action { get; set; }

        [XmlElement("Rol")]
        public List<string> Roles { get; set; }

        [XmlIgnore]
        public bool Current;
    }
}
