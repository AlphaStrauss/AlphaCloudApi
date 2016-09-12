using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaStrauss.AlphaCloudApi.Clouds
{
    interface IGeneralApi
    {
        bool HasAccessToken { get; }

        Uri Start();

        bool BrowserNavigating(Uri uri);

        Task<string> Download(string folder, string file);

        Task Upload(string folder, string file, MemoryStream content);
    }
}
