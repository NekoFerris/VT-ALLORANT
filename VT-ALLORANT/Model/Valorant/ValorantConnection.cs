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
            Api = RiotApi.NewInstance("RGAPI-81cdcc1c-ea9d-498a-bcb8-b67a94690aad");
        }

        public static string GetUserUUIDByNameAndTag(string name, string tag)
        {
            return Api.AccountV1.GetByRiotId(Region.Europe, tag, name)?.Puuid ?? throw new Exception("User not found");
        }
    }
}