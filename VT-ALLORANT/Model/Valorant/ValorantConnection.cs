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
            Api = RiotApi.NewInstance("RGAPI-2da73840-f5c6-4314-9c53-8a0d4f1f2311");
        }

        public static string GetUserUUIDByNameAndTag(string name, string tag)
        {
            return Api.AccountV1.GetByRiotId(Region.Europe, tag, name)?.Puuid ?? throw new Exception("User not found");
        }
    }
}