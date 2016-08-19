using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaStrauss.AlphaCloudApi.Clouds
{
    public abstract class GeneralApi
    {
        public abstract Uri Start();

        public abstract bool BrowserNavigating(Uri uri);

        public abstract Task<string> Download(string folder, string file);

        public abstract Task Upload(string folder, string file, byte[] content);
    }
}
