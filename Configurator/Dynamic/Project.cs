/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016  Denis Kuzmin <entry.reg@gmail.com>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using net.r_eg.Conari.Log;
using net.r_eg.DllExport.Configurator.Exceptions;

namespace net.r_eg.DllExport.Configurator.Dynamic
{
    internal class Project: IProject
    {
        public const string NS_PROP_NAME = "DllExportNamespace";

        /// <summary>
        /// EnvDTE.Project
        /// </summary>
        protected dynamic pdte;

        /// <summary>
        /// Microsoft.Build.Evaluation.ProjectCollection
        /// </summary>
        protected dynamic pmbe;

        /// <summary>
        /// Property name for store ddNS.
        /// </summary>
        public string NamespacePropertyName
        {
            get {
                return NS_PROP_NAME;
            }
        }

        /// <summary>
        /// Check if ddNS is not yet defined.
        /// </summary>
        public bool IsDefinedNamespace
        {
            get {
                string vNamespace = MBEProject.GetPropertyValue(NamespacePropertyName);
                return !String.IsNullOrEmpty(vNamespace);
            }
        }

        protected dynamic MBEProjects
        {
            get {
                return pmbe.LoadedProjects;
            }
        }

        protected dynamic MBEProject
        {
            get
            {
                foreach(var prj in loadedMBEProjects(pdte.FullName)) {
                    return prj;
                }
                throw new InvalidObjectException("Projects is invalid or empty");
            }
        }

        protected dynamic DTEProjects
        {
            get {
                return pdte.Collection;
            }
        }

        /// <summary>
        /// Define namespace value for current EnvDTE.Project.
        /// </summary>
        /// <param name="name"></param>
        public void defineNamespace(string name)
        {
            checkName(ref name);

            MBEProject.SetProperty(NamespacePropertyName, name);
            saveViaDTE();
        }

        /// <summary>
        /// To update namespace value for all available projects that contains this meta-library in references.
        /// TODO: another way
        /// </summary>
        /// <param name="name"></param>
        /// <param name="metalibName">File name without extension of meta library.</param>
        /// <param name="metalibKey">PublicKeyToken of meta library.</param>
        public void updateNamespaceForAllProjects(string name, string metalibName, string metalibKey)
        {
            checkName(ref name);

            foreach(var dtePrj in DTEProjects)
            {
                if(!hasReference(dtePrj.Object.References, metalibName, metalibKey)) {
                    continue;
                }

                foreach(var ePrj in loadedMBEProjects(dtePrj.FullName)) {
                    if(!ePrj.FullPath.EndsWith(".user", StringComparison.OrdinalIgnoreCase)) {
                        ePrj.SetProperty(NamespacePropertyName, name);
                    }
                }
                dtePrj.Save(dtePrj.FullName); // save it with EnvDTE to avoid 'modified outside the environment'
            }
        }

        /// <summary>
        /// To save project via EnvDTE.Project.
        /// </summary>
        public void saveViaDTE()
        {
            pdte.Save();
        }

        /// <summary>
        /// Get property value from current project.
        /// https://msdn.microsoft.com/en-us/library/microsoft.build.evaluation.project.getpropertyvalue.aspx
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string getPropertyValue(string name)
        {
            return MBEProject.GetPropertyValue(name);
        }

        /// <summary>
        /// Set property from current project.
        /// https://msdn.microsoft.com/en-us/library/microsoft.build.evaluation.project.setproperty.aspx
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public dynamic/*ProjectProperty*/ setProperty(string name, string val)
        {
#if DEBUG
            LSender._.send(this, $"Set property for project: '{name}' = '{val}'");
#endif
            return MBEProject.SetProperty(name, val);
        }

        /// <summary>
        /// Removes an property from the project.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <returns></returns>
        public bool removeProperty(string name)
        {
#if DEBUG
            LSender._.send(this, $"Remove property from project: '{name}'");
#endif
            if(String.IsNullOrWhiteSpace(name)) {
                return false;
            }

            // https://msdn.microsoft.com/en-us/library/microsoft.build.evaluation.project.getproperty.aspx
            var prop = MBEProject.GetProperty(name);

            // https://msdn.microsoft.com/en-us/library/microsoft.build.evaluation.project.removeproperty.aspx
            return (prop != null) ? MBEProject.RemoveProperty(prop) : false;
        }

        public Project(dynamic pdte, dynamic pmbe)
        {
            this.pdte = pdte;
            this.pmbe = pmbe;
        }

        protected dynamic loadedMBEProjects(string fullname)
        {
            return pmbe.GetLoadedProjects(fullname);
        }

        protected bool hasReference(dynamic pRefs, string name, string pubkey)
        {
            foreach(var pRef in pRefs) {
                // https://msdn.microsoft.com/en-us/library/vslangproj.reference.aspx
                if(pRef.Name == name && pRef.PublicKeyToken == pubkey) {
                    return true;
                }
            }
            return false;
        }

        protected void checkName(ref string name)
        {
            if(String.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("Namespace cannot be null.");
            }
        }
    }
}
