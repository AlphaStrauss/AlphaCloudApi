using System;
using System.Text;
using System.Threading.Tasks;

using Dropbox.Api;
using System.Diagnostics;
using System.IO;
using Dropbox.Api.Files;

namespace AlphaStrauss.AlphaCloudApi.Clouds
{
    public partial class DropboxApi : GeneralApi
    {
        private string redirectUri { get; set; } = "http://localhost:5000/Home/Auth";
        private string oauth2State { get; set; }
        private string AccessToken { get; set; }
        private string Uid { get; set; }

        public DropboxClient client { get; private set; }

        public DropboxApi()
        {

        }

        public override Uri Start()
        {
            this.oauth2State = Guid.NewGuid().ToString("N");
            Uri authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, appkey, redirectUri, state: oauth2State);
            Debug.WriteLine("AuthorizeURI: " + authorizeUri.AbsolutePath);

            return authorizeUri;
        }

        public override bool BrowserNavigating(Uri uri)
        {
            // @todo: looking later for better condition
            if (false) //!uri.ToString().StartsWith(redirectUri, StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine("Dropbox: BrowserNavigating > ignored\n"+redirectUri+" <=> "+uri.ToString());

                // we need to ignore all navigation that isn't to the redirect uri.
                return false;
            }

            try
            {
                OAuth2Response result = DropboxOAuth2Helper.ParseTokenFragment(uri);

                Debug.WriteLine("Dropbox: BrowserNavigating > passed ParseTokenFragment");

                if (result.State != this.oauth2State)
                {
                    // The state in the response doesn't match the state in the request.
                    return false;
                }

                this.AccessToken = result.AccessToken;
                this.Uid = result.Uid;

                this.client = new DropboxClient(AccessToken);

                Debug.WriteLine("Dropbox: BrowserNavigating > passed new DropboxClient");

                return true;
            }
            catch (ArgumentException)
            {
                // There was an error in the URI passed to ParseTokenFragment or new DropboxClient

                Debug.WriteLine("Dropbox: Error in ParseTokenFragment or new DropboxClient");

                return false;
            }
        }

        public override async Task<string> Download(string folder, string file)
        {
            using (var response = await client.Files.DownloadAsync(folder + "/" + file))
            {
                return await response.GetContentAsStringAsync();
            }
        }

        public override async Task Upload(string folder, string file, MemoryStream content)
        {
            Debug.WriteLine("Dropbox: upload file...");

            if (client == null)
                Debug.WriteLine("client is null!");

            if (client.Files == null)
                Debug.WriteLine("client.Files is null!");
                
            byte[] contentBytes = content.ToArray();
            Debug.WriteLine("Content ("+contentBytes.Length+"): "+Encoding.UTF8.GetString(contentBytes, 0, contentBytes.Length));

            Debug.WriteLine("Dropbox: create commitinfo...");

            CommitInfo info = new CommitInfo((folder[0] == '/' ? "":"/") + folder + "/" + file, WriteMode.Overwrite.Instance);

            Debug.WriteLine("Dropbox: file ready for upload...");

            FileMetadata updated = await client.Files.UploadAsync(info, content);

            Debug.WriteLine("Dropbox: successfully uploaded!");
        }
    }
}
