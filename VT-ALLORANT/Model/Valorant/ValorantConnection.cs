using MingweiSamuel.Camille.Enums;
using MingweiSamuel.Camille;

namespace VT_ALLORANT.Model.Valorant
{

    public static class ValorantConnection
    {
        public static RiotApi Api { get; set; }

        // Constructor
        static ValorantConnection()
        {
            Config config = new();
            Api = RiotApi.NewInstance(config.ValorantApiKey);
        }

        public static string GetUserUUIDByNameAndTag(string name, string tag)
        {
            return Api.AccountV1.GetByRiotId(Region.Europe, tag, name)?.Puuid ?? throw new Exception("User not found");
        }
    }
}