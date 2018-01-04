using System.ComponentModel;
using System.Configuration.Install;

namespace AceAgent
{
    [RunInstaller(true)]
    public partial class WinServiceInstaller : Installer
    {
        public WinServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
