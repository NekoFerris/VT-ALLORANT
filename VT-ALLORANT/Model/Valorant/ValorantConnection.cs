using MingweiSamuel.Camille.Enums;
using MingweiSamuel.Camille;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using System.Text;

namespace VT_ALLORANT.Model.Valorant
{

    public static class ValorantConnection
    {
        public static RiotApi Api { get; set; }
        public static string password { get; set; }
        public static string accessToken { get; set; }
        public static string idToken { get; set; }
        public static int port { get; set; } = 0;

        public static HttpClient httpClient = new();

        // Constructor
        static ValorantConnection()
        {
            Config config = new();
            Api = RiotApi.NewInstance(config.ValorantApiKey);
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            httpClient = new HttpClient(handler);
            if(isValorantRunning())
            {
                GetPasswordAndPort();
                //GetTokens();
            }
        }

        public static string GetUserUUIDByNameAndTag(string name, string tag)
        {
            return Api.AccountV1.GetByRiotId(Region.Europe, tag, name)?.Puuid ?? throw new Exception("User not found");
        }

        public static bool isValorantRunning()
        {
            Process[] pname = Process.GetProcessesByName("VALORANT-Win64-Shipping");
            return pname.Length > 0;
        }
        public static void GetPasswordAndPort()
        {
            if(File.Exists("lockfile"))
            {
                File.Delete("lockfile");
            }
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Config", "lockfile");
            if(!File.Exists(path))
            {
                return;
            }
            File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Config", "lockfile"), "lockfile");
            string[] data = File.ReadAllText("lockfile").Split(':');
            password = data[3];
            port = int.Parse(data[2]);
        }
        public static void GetTokens()
        {
            RestClient client = new(httpClient, false);
            client.AddDefaultHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("riot:" + password)));
            RestRequest request = new();
            //RestResponse response = client.Get($"https://127.0.0.1:{port}/entitlements/v1/token", request);
            //accessToken = JObject.Parse(response.Content)[nameof(accessToken)]!.ToString();
            //idToken = JObject.Parse(response.Content)[nameof(idToken)]!.ToString();
        }
        public static void SendFriendRequest(string name, string tag)
        {
            RestClient client = new(httpClient, false);
            client.AddDefaultHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("riot:" + password)));
            FriendRequest friendRequest = new()
            {
                game_name = name,
                game_tag = tag,
            };
            Debug.WriteLine(JsonConvert.SerializeObject(friendRequest).ToString());
            Debug.WriteLine(client.PostJson<FriendRequest>($"https://127.0.0.1:{port}/chat/v4/friendrequests", friendRequest));
        }
    }
}