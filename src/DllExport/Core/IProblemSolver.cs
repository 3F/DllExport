using System;

namespace net.r_eg.DllExport
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
