/*!
 * Copyright (c) Robert Giesecke
 * Copyright (c) Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) DllExport contributors https://github.com/3F/DllExport/graphs/contributors
 * Licensed under the MIT License (MIT).
 * See accompanying LICENSE.txt file or visit https://github.com/3F/DllExport
*/

using System;

namespace net.r_eg.DllExport.Problems
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
        public virtual bool TryToSolve()
        {
            // TODO:
            return false;
        }

        public AccessDenied(Exception ex)
        {
            raw = ex;
            Initialize(ex);
        }

        protected void Initialize(Exception ex)
        {
            Code        = 0x80070005;
            Title       = "Access is denied";
            Message     = "You do not have permission to requested operation above. Info: https://github.com/3F/DllExport/issues/1";
            HowToSolve  = "It possible when used IlAsm for signed assembly (Option: 'Sign the assembly'). Try to get administrative rights, or avoid .snk / .pfx key for this project.";
        }
    }
}
