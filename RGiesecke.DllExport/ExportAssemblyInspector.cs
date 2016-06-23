// [Decompiled] Assembly: RGiesecke.DllExport, Version=1.2.2.23706, Culture=neutral, PublicKeyToken=ad5f9f4a55b5020b
// Author of original assembly (MIT-License): Robert Giesecke
// Use Readme & LICENSE files for details.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Mono.Cecil;

namespace RGiesecke.DllExport
{
    internal class ExportAssemblyInspector: MarshalByRefObject, IExportAssemblyInspector
    {
        public string DllExportAttributeFullName
        {
            get {
                if(this.InputValues == null)
                {
                    return Utilities.DllExportAttributeFullName;
                }
                return this.InputValues.DllExportAttributeFullName;
            }
        }

        public IInputValues InputValues
        {
            get;
            set;
        }

        public AssemblyExports ExtractExports(AssemblyDefinition assemblyDefinition)
        {
            return this.ExtractExports(assemblyDefinition, new ExtractExportHandler(this.TryExtractExport));
        }

        public AssemblyExports ExtractExports()
        {
            return this.ExtractExports(this.LoadAssembly(this.InputValues.InputFileName));
        }

        public AssemblyExports ExtractExports(string fileName)
        {
            return this.ExtractExports(this.LoadAssembly(fileName));
        }

        public AssemblyExports ExtractExports(AssemblyDefinition assemblyDefinition, ExtractExportHandler exportFilter)
        {
            List<TypeDefinition> typeDefinitionList = new List<TypeDefinition>();
            foreach(ModuleDefinition module in assemblyDefinition.Modules)
            {
                typeDefinitionList.AddRange((IEnumerable<TypeDefinition>)module.Types);
            }
            AssemblyExports result = new AssemblyExports() {
                InputValues = this.InputValues
            };
            foreach(TypeDefinition td in typeDefinitionList)
            {
                List<ExportedMethod> exportMethods = new List<ExportedMethod>();
                foreach(MethodDefinition method in td.Methods)
                {
                    TypeDefinition typeRefCopy = td;
                    ExportAssemblyInspector.CheckForExportedMethods((Func<ExportedMethod>)(() => new ExportedMethod(this.GetExportedClass(typeRefCopy, result))), exportFilter, exportMethods, method);
                }
                foreach(ExportedMethod exportedMethod in exportMethods)
                {
                    this.GetExportedClass(td, result).Methods.Add(exportedMethod);
                }
            }
            result.Refresh();
            return result;
        }

        private ExportedClass GetExportedClass(TypeDefinition td, AssemblyExports result)
        {
            ExportedClass exportedClass;
            if(!result.ClassesByName.TryGetValue(td.FullName, out exportedClass))
            {
                exportedClass = new ExportedClass() {
                    FullTypeName = td.FullName
                };
                result.ClassesByName.Add(exportedClass.FullTypeName, exportedClass);
            }
            return exportedClass;
        }

        public AssemblyExports ExtractExports(string fileName, ExtractExportHandler exportFilter)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(fileName));
                return this.ExtractExports(this.LoadAssembly(fileName), exportFilter);
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDirectory);
            }
        }

        public AssemblyBinaryProperties GetAssemblyBinaryProperties(string assemblyFileName)
        {
            AssemblyBinaryProperties binaryProperties;
            if(!File.Exists(assemblyFileName))
            {
                binaryProperties = AssemblyBinaryProperties.GetEmpty();
            }
            else
            {
                AssemblyDefinition assemblyDefinition = this.LoadAssembly(assemblyFileName);
                ModuleDefinition mainModule = assemblyDefinition.MainModule;
                string keyFileName = (string)null;
                string keyContainer = (string)null;
                foreach(CustomAttribute customAttribute in assemblyDefinition.CustomAttributes)
                {
                    switch(customAttribute.Constructor.DeclaringType.FullName)
                    {
                        case "System.Reflection.AssemblyKeyFileAttribute":
                        keyFileName = Convert.ToString((object)customAttribute.ConstructorArguments[0], (IFormatProvider)CultureInfo.InvariantCulture);
                        break;

                        case "System.Reflection.AssemblyKeyNameAttribute":
                        keyContainer = Convert.ToString((object)customAttribute.ConstructorArguments[0], (IFormatProvider)CultureInfo.InvariantCulture);
                        break;
                    }
                    if(!string.IsNullOrEmpty(keyFileName))
                    {
                        if(!string.IsNullOrEmpty(keyContainer))
                        {
                            break;
                        }
                    }
                }
                binaryProperties = new AssemblyBinaryProperties(mainModule.Attributes, mainModule.Architecture, assemblyDefinition.Name.HasPublicKey, keyFileName, keyContainer);
            }
            return binaryProperties;
        }

        public AssemblyDefinition LoadAssembly(string fileName)
        {
            return AssemblyDefinition.ReadAssembly(fileName);
        }

        public bool SafeExtractExports(string fileName, Stream stream)
        {
            AssemblyExports exports = this.ExtractExports(fileName);
            bool flag;
            if(exports.Count == 0)
            {
                flag = false;
            }
            else
            {
                new BinaryFormatter().Serialize(stream, (object)exports);
                flag = true;
            }
            return flag;
        }

        private static void CheckForExportedMethods(Func<ExportedMethod> createExportMethod, ExtractExportHandler exportFilter, List<ExportedMethod> exportMethods, MethodDefinition mi)
        {
            IExportInfo exportInfo;
            if(!exportFilter(mi, out exportInfo))
            {
                return;
            }
            ExportedMethod exportedMethod = createExportMethod();
            exportedMethod.MemberName = mi.Name;
            exportedMethod.AssignFrom(exportInfo);
            if(string.IsNullOrEmpty(exportedMethod.ExportName))
            {
                exportedMethod.ExportName = mi.Name;
            }
            if(exportedMethod.CallingConvention == (CallingConvention)0)
            {
                exportedMethod.CallingConvention = CallingConvention.Winapi;
            }
            exportMethods.Add(exportedMethod);
        }

        public bool TryExtractExport(ICustomAttributeProvider memberInfo, out IExportInfo exportInfo)
        {
            exportInfo = (IExportInfo)null;
            foreach(CustomAttribute customAttribute in memberInfo.CustomAttributes)
            {
                if(customAttribute.Constructor.DeclaringType.FullName == this.DllExportAttributeFullName)
                {
                    int num = customAttribute.IsResolved ? 1 : 0;
                    exportInfo = (IExportInfo)new ExportInfo();
                    IExportInfo ei = exportInfo;
                    int index = -1;
                    foreach(CustomAttributeArgument constructorArgument in customAttribute.ConstructorArguments)
                    {
                        ++index;
                        ParameterDefinition parameterDefinition = customAttribute.Constructor.Parameters[index];
                        ExportAssemblyInspector.SetParamValue(ei, parameterDefinition.ParameterType.FullName, constructorArgument.Value);
                    }
                    using(var enumerator = customAttribute.Fields.Concat<CustomAttributeNamedArgument>((IEnumerable<CustomAttributeNamedArgument>)customAttribute.Properties).Select(arg => new {
                        Name = arg.Name,
                        Value = arg.Argument.Value
                    }).Distinct().GetEnumerator())
                    {
                        while(enumerator.MoveNext())
                        {
                            var current = enumerator.Current;
                            ExportAssemblyInspector.SetFieldValue(ei, current.Name, current.Value);
                        }
                        break;
                    }
                }
            }
            return exportInfo != null;
        }

        private static void SetParamValue(IExportInfo ei, string name, object value)
        {
            if(name == null)
            {
                return;
            }
            if(name != "System.String")
            {
                if(!(name == "System.Runtime.InteropServices.CallingConvention"))
                {
                    return;
                }
                ei.CallingConvention = (CallingConvention)value;
            }
            else
            {
                ei.ExportName = value.NullSafeCall<object, string>((Func<object, string>)(v => v.ToString()));
            }
        }

        private static void SetFieldValue(IExportInfo ei, string name, object value)
        {
            string upperInvariant = name.NullSafeToUpperInvariant();
            if(upperInvariant == null)
            {
                return;
            }
            if(upperInvariant != "NAME" && upperInvariant != "EXPORTNAME")
            {
                if(!(upperInvariant == "CALLINGCONVENTION") && !(upperInvariant == "CONVENTION"))
                {
                    return;
                }
                ei.CallingConvention = (CallingConvention)value;
            }
            else
            {
                ei.ExportName = value.NullSafeToString();
            }
        }
    }
}
