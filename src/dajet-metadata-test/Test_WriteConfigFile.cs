﻿using DaJet.Metadata.Core;
using DaJet.Metadata.Model;
using DaJet.Metadata.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace DaJet.Metadata.Test
{
    [TestClass] public class Test_WriteConfigFile
    {
        private const string MS_CONNECTION_STRING = "Data Source=ZHICHKIN;Initial Catalog=cerberus;Integrated Security=True;Encrypt=False;";
        [TestMethod] public void WriteRowDataToFile()
        {
            // Предопределённые значения "Справочник.СправочникПредопределённые"
            //string fileName = "29f879f3-b889-4745-8dec-c3e18da8f84c.1c"; // dajet-metadata-ms
            
            string fileName = "21642324-2e5a-4fff-ad60-10249ec30090.1c"; // cerberus

            using (ConfigFileReader reader = new(DatabaseProvider.SQLServer, MS_CONNECTION_STRING, ConfigTables.Config, fileName))
            {
                using (StreamWriter stream = new StreamWriter("C:\\temp\\bbb_utf8.txt", false, Encoding.UTF8))
                {
                    stream.Write(reader.Stream.ReadToEnd());
                }
            }
        }
        [TestMethod] public void WriteConfigObjectToFile()
        {
            // Предопределённые значения "Справочник.СправочникПредопределённые"
            //string fileName = "29f879f3-b889-4745-8dec-c3e18da8f84c.1c"; // dajet-metadata-ms

            string fileName = "21642324-2e5a-4fff-ad60-10249ec30090.1c"; // cerberus

            using (ConfigFileReader reader = new(DatabaseProvider.SQLServer, MS_CONNECTION_STRING, ConfigTables.Config, fileName))
            {
                ConfigObject configObject = new ConfigFileParser().Parse(reader);

                new ConfigFileWriter().Write(configObject, "C:\\temp\\aaa.txt");
            }
        }
    }
}