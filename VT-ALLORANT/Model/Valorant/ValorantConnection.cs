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
            Api = RiotApi.NewInstance("RGAPI-6ff86305-87df-4146-a254-2cdc2b673f4f");
        }

        public static string GetUserUUIDByNameAndTag(string name, string tag)
        {
            return Api.AccountV1.GetByRiotId(Region.Europe, tag, name).Puuid ?? throw new Exception("User not found");
        }
    }
}