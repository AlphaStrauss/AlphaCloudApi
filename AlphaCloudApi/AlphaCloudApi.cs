using AlphaStrauss.AlphaCloudApi.Clouds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaStrauss.AlphaCloudApi
{
    public class AlphaCloudApi : GeneralApi
    {
        public CloudType cloud;
        public GeneralApi cloudApi;

        public AlphaCloudApi(CloudType type)
        {
            Initialize();

            Start();
        }

        public void Initialize()
        {
            switch (cloud)
            {
                case CloudType.Dropbox:
                    cloudApi = new DropboxApi();
                    break;
            }
        }

        #region implementation of abstract functions

        public override Uri Start()
        {
            return cloudApi.Start();
        }

        public override bool BrowserNavigating(Uri uri)
        {
            return cloudApi.BrowserNavigating(uri);
        }

        public override Task<string> Download(string folder, string file)
        {
            return cloudApi.Download(folder, file);
        }

        public override Task Upload(string folder, string file, MemoryStream content)
        {
            return cloudApi.Upload(folder, file, content);
        }

        #endregion
    }
}