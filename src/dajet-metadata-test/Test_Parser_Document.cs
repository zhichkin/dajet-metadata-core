﻿using DaJet.Metadata.Core;
using DaJet.Metadata.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DaJet.Metadata.Test
{
    [TestClass] public class Test_Parser_Document
    {
        private const string MS_CONNECTION_STRING = "Data Source=ZHICHKIN;Initial Catalog=dajet-metadata-ms;Integrated Security=True;Encrypt=False;";
        private const string PG_CONNECTION_STRING = "Host=127.0.0.1;Port=5432;Database=dajet-metadata-pg;Username=postgres;Password=postgres;";

        private readonly MetadataService service = new();

        private InfoBase _infoBase;

        [TestMethod] public void MS_TEST()
        {
            service.OpenInfoBase(DatabaseProvider.SQLServer, MS_CONNECTION_STRING, out _infoBase);
            TEST();
        }
        [TestMethod] public void PG_TEST()
        {
            service.OpenInfoBase(DatabaseProvider.PostgreSQL, PG_CONNECTION_STRING, out _infoBase);
            TEST();
        }
        private void TEST()
        {
            string metadataName = "Документ.ОбычныйДокумент";

            MetadataObject @object = service.GetMetadataObject(in _infoBase, metadataName);

            if (@object == null)
            {
                Console.WriteLine($"Metadata object \"{metadataName}\" is not found.");
            }
            else
            {
                ShowMetadataObject((Document)@object);
            }

            Console.WriteLine();
            Console.WriteLine("Change table: " + service.GetChangeTableName(@object));

            Console.WriteLine();
            ShowDatabaseNames((Document)@object);
        }
        private void ShowDatabaseNames(Document catalog)
        {
            Console.WriteLine($"TableName: {catalog.TableName}");

            Console.WriteLine("Properties:");

            foreach (MetadataProperty property in catalog.Properties)
            {
                Console.WriteLine($"- {property.Name} ({property.DbName})");

                foreach (DatabaseField field in property.Fields)
                {
                    Console.WriteLine($"--- {field.Name} ({field.TypeName})");
                }
            }
        }
        private void ShowMetadataObject(Document @object)
        {
            Console.WriteLine($"Uuid: {@object.Uuid}");
            Console.WriteLine($"Name: {@object.Name}");
            Console.WriteLine($"Alias: {@object.Alias}");
            
            Console.WriteLine("Properties:");

            foreach (MetadataProperty property in @object.Properties)
            {
                ShowProperty(property);
            }

            Console.WriteLine("TableParts:");

            foreach (TablePart table in @object.TableParts)
            {
                ShowTablePart(table);
            }
        }
        private void ShowTablePart(TablePart table)
        {
            Console.WriteLine($"Uuid: {table.Uuid}");
            Console.WriteLine($"Name: {table.Name}");

            Console.WriteLine("Properties:");

            foreach (MetadataProperty property in table.Properties)
            {
                ShowProperty(property);
            }
        }
        private void ShowProperty(MetadataProperty property)
        {
            Console.WriteLine($"- {property.Purpose}: {property.Name} ({property.Alias}) {{{property.Uuid}}} {property.PropertyUsage}");

            DataTypeSet type = property.PropertyType;
            if (type == null)
            {
                return;
            }

            string name = string.Empty;
            
            //if (type.CanBeReference && type.Reference != Guid.Empty)
            //{
            //    MetadataObject @object = service.GetMetadataObjectByReference(in _infoBase, type.Reference);
            //    if (@object != null)
            //    {
            //        name = MetadataTypes.ResolveNameRu(@object.Uuid) + "." + @object.Name;
            //    }
            //}

            if (type.IsMultipleType)
            {
                Console.WriteLine($"  * MULTIPLE");
                if (type.CanBeString) Console.WriteLine($"    - String ({type.StringLength}) {type.StringKind}");
                if (type.CanBeBoolean) Console.WriteLine("    - Boolean");
                if (type.CanBeNumeric) Console.WriteLine($"    - Numeric ({type.NumericPrecision},{type.NumericScale}) {type.NumericKind}");
                if (type.CanBeDateTime) Console.WriteLine($"    - DateTime ({type.DateTimePart})");
                if (type.CanBeReference) Console.WriteLine($"    - Reference ({(type.Reference == Guid.Empty ? "multiple" : "single")}) {name}");
                return;
            }

            if (type.IsUuid)
            {
                Console.WriteLine("  * UUID");
            }
            else if (type.IsBinary)
            {
                Console.WriteLine("  * Binary");
            }
            else if (type.IsValueStorage)
            {
                Console.WriteLine("  * ValueStorage");
            }
            else if (type.CanBeBoolean)
            {
                Console.WriteLine("  * Boolean");
            }
            else if (type.CanBeString)
            {
                Console.WriteLine($"  * String ({type.StringLength}) {type.StringKind}");
            }
            else if (type.CanBeNumeric)
            {
                Console.WriteLine($"  * Numeric ({type.NumericPrecision},{type.NumericScale}) {type.NumericKind}");
            }
            else if (type.CanBeDateTime)
            {
                Console.WriteLine($"  * DateTime ({type.DateTimePart})");
            }
            else if (type.CanBeReference)
            {
                Console.WriteLine($"  * {type} [{name}]");
            }
        }
    }
}