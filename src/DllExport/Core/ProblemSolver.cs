using System;

namespace net.r_eg.DllExport
{
    public class ProblemSolver: IProblemSolver
    {
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
            Initialize(ex);
        }

        protected ProblemSolver()
        {

        }

        protected void Initialize(Exception ex)
        {
            Raw = ex;
            Check(ex);
        }

        protected void Check(Exception ex)
        {
            if(ex.GetType() == typeof(InvalidOperationException))
            {
                Check((InvalidOperationException)ex);
            }
        }

        protected void Check(InvalidOperationException ex)
        {
            if(ex.Message.Contains("0x80070005"))
            {
                Found = new Problems.AccessDenied(ex);
            }
        }
    }
}
