using AlphaStrauss.AlphaCloudApi.Clouds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaStrauss.AlphaCloudApi
{
    public class AlphaCloudApi : IGeneralApi
    {
        public CloudType cloud;
        private IGeneralApi cloudApi;

        public bool HasAccessToken
        {
            get
            {
                return cloudApi.HasAccessToken;
            }
        }

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

        public Uri Start()
        {
            return cloudApi.Start();
        }

        public bool BrowserNavigating(Uri uri)
        {
            return cloudApi.BrowserNavigating(uri);
        }

        public Task<string> Download(string folder, string file)
        {
            if (HasAccessToken)
            {
                return cloudApi.Download(folder, file);
            }
            else
            {
                throw new MissingCloudAccessTokenException();
            }
        }

        public Task Upload(string folder, string file, MemoryStream content)
        {
            if (HasAccessToken)
            {
                return cloudApi.Upload(folder, file, content);
            }
            else
            {
                throw new MissingCloudAccessTokenException();
            }
        }

        #endregion
    }
}