using System;

namespace RGiesecke.DllExport
{
    public interface IProblemSolver
    {
        /// <summary>
        /// Found problem.
        /// </summary>
        Problems.IProblem Found { get; }

        /// <summary>
        /// Formatted message.
        /// </summary>
        string FMsg { get; }

        /// <summary>
        /// Initial exception.
        /// </summary>
        Exception Raw { get; }
    }
}
