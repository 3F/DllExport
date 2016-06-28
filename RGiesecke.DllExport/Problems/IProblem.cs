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
