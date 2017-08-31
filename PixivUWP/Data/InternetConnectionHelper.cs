using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace PixivUWP.Data
{
    public class InternetConnectionHelper
    {
        public enum ConnectionType
        {
            WLan,
            WWan,
            Wired
        }

        public ConnectionType GetConnectionType()
        {
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile.IsWlanConnectionProfile)
                return ConnectionType.WLan;
            if (profile.IsWwanConnectionProfile)
                return ConnectionType.WWan;
            return ConnectionType.Wired;
        }
    }
}
