using Vintagestory.API.Common;

namespace CarryOnMore
{
    static class ModConfig
    {
        public static CarryOnMoreConfig ServerConfig;

        private const string ConfigFile = "CarryOnMoreConfig.json";

        public static void ReadConfig(ICoreAPI api)
        {
            if (api.Side != EnumAppSide.Server)
            {
                return;
            }

            try
            {
                ServerConfig = LoadConfig(api);

                if (ServerConfig == null)
                {
                    GenerateConfig(api);
                    ServerConfig = LoadConfig(api);
                }
                else
                {
                    GenerateConfig(api, ServerConfig);
                }
            }
            catch
            {
                GenerateConfig(api);
                ServerConfig = LoadConfig(api);
            }

            var worldConfig = api.World.Config;

            worldConfig.SetBool(CarryMoreSystem.ModId + ":AllowExtraChestsOnBack", ServerConfig.AllowExtraChestsOnBack);
        }

        private static CarryOnMoreConfig LoadConfig(ICoreAPI api)
        {
            return api.LoadModConfig<CarryOnMoreConfig>(ConfigFile);
        }

        private static void GenerateConfig(ICoreAPI api)
        {
            api.StoreModConfig(new CarryOnMoreConfig(), ConfigFile);
        }

        private static void GenerateConfig(ICoreAPI api, CarryOnMoreConfig previousConfig)
        {
            api.StoreModConfig(new CarryOnMoreConfig(previousConfig), ConfigFile);
        }
    }
}