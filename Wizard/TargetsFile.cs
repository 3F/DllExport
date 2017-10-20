/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2017  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
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
using System.Collections.Generic;
using net.r_eg.MvsSln.Core;
using net.r_eg.MvsSln.Extensions;
using net.r_eg.MvsSln.Log;

namespace net.r_eg.DllExport.Wizard
{
    public class TargetsFile: Project, IProject, ITargetsFile, IDisposable
    {
        /// <summary>
        /// To configure external .targets through parent project.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        public void Configure(ActionType type, IProject parent)
        {
            if(type != ActionType.Update && type != ActionType.Configure) {
                return;
            }

            Config = parent?.Config ?? throw new ArgumentNullException(nameof(parent));
            Log.send(this, $"Configuring TargetsFile '{parent.DxpIdent}' -> '{XProject.Project.FullPath}'", Message.Level.Debug);

            if(type == ActionType.Update && parent.Installed
                || type == ActionType.Configure && Config.Install)
            {
                Configure(parent);
            }

            XProject.Reevaluate();
            XProject.Save();
        }

        public TargetsFile(string file)
            : base(new XProject(new ProjectItemCfg(), new Microsoft.Build.Evaluation.Project()))
        {
            XProject.Project.FullPath = file;
        }

        public TargetsFile(IWizardConfig cfg)
           : this(cfg?.StoragePath)
        {

        }

        protected void Configure(IProject parent)
        {
            CfgCommonData();

            SetProperties(
                ConfigProperties, 
                $"'$(DllExportIdent)'=='{parent.DxpIdent}'",
                MakeBasePath(parent.XProject.ProjectFullPath)
            );
            XProject.Reevaluate();

            if(XProject.GetImport().project == null) {
                AddDllExportLib();
            }
        }

        protected void SetProperties(IEnumerable<KeyValuePair<string, string>> properties, string condition, string label)
        {
            var group = XProject.Project.Xml.AddPropertyGroup();
            group.Condition = condition;
            group.Label     = label;

            properties.ForEach(p => group.SetProperty(p.Key, p.Value));
        }

        private void Free()
        {
            if(XProject?.Project != null && XProject?.Project.FullPath != null) {
                Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection?.UnloadProject(XProject.Project);
            }
        }

        #region IDisposable

        // To detect redundant calls
        private bool disposed = false;

        // To correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;

            Free();
        }

        #endregion
    }
}
