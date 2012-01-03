using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lextm.SharpSnmpLib.Properties;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Default object registry.
    /// </summary>
    [Obsolete("Use SimpleObjectRegistry instead.")]
    public sealed class DefaultObjectRegistry : ObjectRegistryBase
    {
        private static volatile DefaultObjectRegistry _instance;
        private static readonly object Locker = new object();
        
        private DefaultObjectRegistry()
        {
            Tree = new ObjectTree(LoadDefaultModules());
        }

        /// <summary>
        /// Default instance.
        /// </summary>
        [CLSCompliant(false)]
        public static DefaultObjectRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new DefaultObjectRegistry();
                        }
                    }
                }
                
                return _instance;
            }
        }

        private static IList<ModuleLoader> LoadDefaultModules()
        {
            IList<ModuleLoader> result = new List<ModuleLoader>(5)
                                             {
                                                 LoadSingle(Encoding.ASCII.GetString(Resources.SNMPV2_SMI), "SNMPv2-SMI"),
                                                 LoadSingle(Encoding.ASCII.GetString(Resources.SNMPV2_CONF), "SNMPv2-CONF"),
                                                 LoadSingle(Encoding.ASCII.GetString(Resources.SNMPV2_TC), "SNMPv2-TC"),
                                                 LoadSingle(Encoding.ASCII.GetString(Resources.SNMPV2_MIB), "SNMPv2-MIB"),
                                                 LoadSingle(Encoding.ASCII.GetString(Resources.SNMPV2_TM), "SNMPv2-TM")
                                             };
            return result;
        }

        private static ModuleLoader LoadSingle(string mibFileContent, string name)
        {
            ModuleLoader result;
            using (TextReader reader = new StringReader(mibFileContent))
            {
                result = new ModuleLoader(reader, name);
            }
            
            return result;
        }
    }
}