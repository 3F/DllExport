//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2019  Denis Kuzmin < entry.reg@gmail.com > GitHub/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport.Problems
{
    public class AccessDenied: IProblem
    {
        /// <summary>
        /// Initial exception.
        /// </summary>
        protected Exception raw;

        /// <summary>
        /// Human-readable message of problem.
        /// </summary>
        public string Message
        {
            get;
            protected set;
        }

        /// <summary>
        /// Short information about problem.
        /// </summary>
        public string Title
        {
            get;
            protected set;
        }

        /// <summary>
        /// Error code if known.
        /// </summary>
        public Int64? Code
        {
            get;
            protected set;
        }

        /// <summary>
        /// Human-readable message of how to solve it.
        /// </summary>
        public string HowToSolve
        {
            get;
            protected set;
        }

        /// <summary>
        /// Is it possible to solve it automatically.
        /// </summary>
        public bool AutoFix
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// Trying to solve it automatically.
        /// </summary>
        /// <returns>true if problem has been solved.</returns>
        public virtual bool tryToSolve()
        {
            // TODO:
            return false;
        }

        public AccessDenied(Exception ex)
        {
            raw = ex;
            init(ex);
        }

        protected void init(Exception ex)
        {
            Code        = 0x80070005;
            Title       = "Access is denied";
            Message     = "You do not have permission to requested operation above. Info: https://github.com/3F/DllExport/issues/1";
            HowToSolve  = "It possible when used IlAsm for signed assembly (Option: 'Sign the assembly'). Try to get administrative rights, or avoid .snk / .pfx key for this project.";
        }
    }
}
