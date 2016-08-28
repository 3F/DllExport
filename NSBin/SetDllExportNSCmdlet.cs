using System.Management.Automation;
using System.Text;

namespace NSBin
{
    [Cmdlet(VerbsCommon.Set, "DllExportNS")]
    public class SetDllExportNSCmdlet: Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string Dll
        {
            get;
            set;
        }

        [Parameter(Mandatory = true)]
        public string Namespace
        {
            get;
            set;
        }

        protected override void ProcessRecord()
        {
            var dns = new DefNs(Dll) { encoding = Encoding.UTF8 };
            dns.Log.Received += onMsg;

            dns.setNamespace(Namespace);
        }

        private void onMsg(object sender, net.r_eg.Conari.Log.Message e)
        {
            WriteObject(e.content);
        }
    }
}
