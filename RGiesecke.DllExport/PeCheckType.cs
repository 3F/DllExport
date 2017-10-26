
namespace RGiesecke.DllExport
{
    public enum PeCheckType: int
    {
        None,

        /// <summary>
        /// PE Check 1:1.
        /// Will check count of all planned exports from final PE32/PE32+ module.
        /// </summary>
        Pe1to1 = 0x01,

        /// <summary>
        /// PE Check IL code.
        /// Will check existence of all planned exports (IL code) in actual PE32/PE32+ module.
        /// </summary>
        PeIl = 0x02,
    }
}
