using System;

namespace RGiesecke.DllExport
{
    public class ProblemSolver: IProblemSolver
    {
        /// <summary>
        /// Found problem.
        /// </summary>
        public Problems.IProblem Found
        {
            get;
            protected set;
        }

        /// <summary>
        /// Initial exception.
        /// </summary>
        public Exception Raw
        {
            get;
            protected set;
        }

        /// <summary>
        /// Formatted message.
        /// </summary>
        public string FMsg
        {
            get
            {
                var p = Found;
                if(p == null || (p.Message == null && p.HowToSolve == null)) {
                    return null;
                }

                if(p.Message == null) {
                    return $"To solve problem: {p.HowToSolve}";
                }

                var L = Environment.NewLine;
                return $"{p.Title} ::::::{L}{p.Message}{ (p.HowToSolve != null ? L + p.HowToSolve : "") }";
            }
        }

        public ProblemSolver(Exception ex)
        {
            init(ex);
        }

        protected void init(Exception ex)
        {
            Raw = ex;
            selector(ex);
        }

        protected void selector(Exception ex)
        {
            if(ex.GetType() == typeof(InvalidOperationException)) {
                exinit((InvalidOperationException)ex);
                return;
            }
        }

        protected void exinit(InvalidOperationException ex)
        {
            if(ex.Message.Contains("0x80070005")) {
                Found = new Problems.AccessDenied(ex);
                return;
            }
        }
    }
}
