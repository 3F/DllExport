/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2020  Denis Kuzmin < x-3F@outlook.com > GitHub/3F
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
        internal const string DEF_CFG_FILE = ".net.dllexport.targets";

        /// <summary>
        /// Full path to root solution directory.
        /// </summary>
        public override string SlnDir
        {
            get => _slndir;
        }
        private string _slndir;

        /// <summary>
        /// To configure project via specific action.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool Configure(ActionType type)
        {
            return Configure(type, null);
        }

        /// <summary>
        /// To configure external .targets through parent project.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool Configure(ActionType type, IProject parent)
        {
            if(type != ActionType.Update && type != ActionType.Configure) {
                return false;
            }

            Config = parent?.Config ?? throw new ArgumentNullException(nameof(parent));
            Log.send(this, $"Configuring TargetsFile '{parent.DxpIdent}' -> '{XProject.Project.FullPath}'", Message.Level.Debug);

            if(type == ActionType.Update && parent.Installed
                || type == ActionType.Configure && Config.Install)
            {
                Configure(parent);
            }

            Save(true);
            return true;
        }

        /// <summary>
        /// To export data via parent project into .targets file.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool Export(IProject parent)
        {
            Config = parent?.Config ?? throw new ArgumentNullException(nameof(parent));
            Log.send(this, $"Export data via TargetsFile '{parent.DxpIdent}'", Message.Level.Debug);

            if(parent.Installed || Config.Install) {
                Configure(parent);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Saves data to the file system.
        /// </summary>
        /// <param name="reevaluate">Try to reevaluate data of project before saving.</param>
        public void Save(bool reevaluate)
        {
            if(reevaluate) {
                XProject.Reevaluate();
            }
            Save();
        }

        public TargetsFile(string file, string rootpath)
            : this(file, rootpath, ActionType.Default)
        {

        }

        public TargetsFile(string file, string rootpath, ActionType type)
            : base(type == ActionType.Recover ? new XProject(file) : new XProject())
        {
            _slndir = rootpath;
            XProject.Project.FullPath = file;
        }

        protected void Configure(IProject parent)
        {
            CfgCommonData();

            var projectFile = MakeBasePath(parent.XProject.ProjectFullPath, false);

            ConfigProperties[MSBuildProperties.DXP_CFG_ID] = parent.DxpIdent;
            ConfigProperties[MSBuildProperties.DXP_PRJ_FILE] = projectFile;

            SetProperties(
                ConfigProperties, 
                $"'$({MSBuildProperties.DXP_ID})'=='{parent.DxpIdent}'",
                projectFile
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
            if(XProject?.Project != null && XProject.Project.FullPath != null) {
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
