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

namespace net.r_eg.DllExport.Configurator.Dynamic
{
    internal interface IProject
    {
        /// <summary>
        /// Property name for store ddNS.
        /// </summary>
        string NamespacePropertyName { get; }

        /// <summary>
        /// Check if ddNS is not yet defined.
        /// </summary>
        bool IsDefinedNamespace { get; }

        /// <summary>
        /// Define namespace value for current EnvDTE.Project.
        /// </summary>
        /// <param name="name"></param>
        void defineNamespace(string name);

        /// <summary>
        /// To update namespace value for all available projects that contains this meta-library in references.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="metalibName">File name without extension of meta library.</param>
        /// <param name="metalibKey">PublicKeyToken of meta library.</param>
        void updateNamespaceForAllProjects(string name, string metalibName, string metalibKey);

        /// <summary>
        /// Get property value from current project.
        /// https://msdn.microsoft.com/en-us/library/microsoft.build.evaluation.project.getpropertyvalue.aspx
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string getPropertyValue(string name);

        /// <summary>
        /// Set property from current project.
        /// https://msdn.microsoft.com/en-us/library/microsoft.build.evaluation.project.setproperty.aspx
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        dynamic/*ProjectProperty*/ setProperty(string name, string val);

        /// <summary>
        /// To save project via EnvDTE.Project.
        /// </summary>
        void saveViaDTE();
    }
}
