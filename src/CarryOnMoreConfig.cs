namespace CarryOnMore
{
    class CarryOnMoreConfig
    {
        public bool AllowExtraChestsOnBack;

        public CarryOnMoreConfig()
        {
        }

        public CarryOnMoreConfig(CarryOnMoreConfig previousConfig)
        {
            AllowExtraChestsOnBack = previousConfig.AllowExtraChestsOnBack;
        }
    }
}