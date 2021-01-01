//# Author of original code ([Decompiled] MIT-License): Copyright (c) 2009-2015  Robert Giesecke
//# Use Readme & LICENSE files for details.

//# Modifications: Copyright (c) 2016-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
//$ Distributed under the MIT License (MIT)

using System;

namespace RGiesecke.DllExport.Problems
{
    public interface IProblem
    {
        /// <summary>
        /// Human-readable message of problem.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Short information about problem.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Error code if known.
        /// </summary>
        Int64? Code { get; }

        /// <summary>
        /// Human-readable message of how to solve it.
        /// </summary>
        string HowToSolve { get; }

        /// <summary>
        /// Is it possible to solve it automatically.
        /// </summary>
        bool AutoFix { get; }

        /// <summary>
        /// Trying to solve it automatically.
        /// </summary>
        /// <returns>true if problem has been solved.</returns>
        bool tryToSolve();
    }
}
