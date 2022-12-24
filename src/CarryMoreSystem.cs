using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

[assembly: ModInfo("Carry On More",
    Description = "Adds the capability to carry various things from other mods",
    Authors = new[] { "NerdScurvy" })]
[assembly: ModDependency("game", "1.17.0")]

namespace CarryOnMore
{
    /// <summary> Main system for the "Carry On More" mod. </summary>
    public class CarryMoreSystem : ModSystem
    {
        public static string ModId = "carryonmore";

        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

            ModConfig.ReadConfig(api);

            api.World.Logger.Event("started 'CarryOnMore' mod");
        }

        public override void AssetsFinalize(ICoreAPI api)
        {
            // Needs to be done before assets are ready because it rewrites Behavior

            foreach (Block block in api.World.Blocks)
            {
                if (block.Code == null || block.Id == 0) continue;

                block.BlockBehaviors = RemoveOveriddenCarryableBehaviours(block.BlockBehaviors.OfType<CollectibleBehavior>().ToArray()).OfType<BlockBehavior>().ToArray();
                block.CollectibleBehaviors = RemoveOveriddenCarryableBehaviours(block.CollectibleBehaviors);
            }
        }

        private CollectibleBehavior[] RemoveOveriddenCarryableBehaviours(CollectibleBehavior[] behaviours)
        {
            var behaviourList = behaviours.ToList();
            var carryableList = FindCarryables(behaviourList);
            if (carryableList.Count > 1)
            {
                var carryableOverride = carryableList.Find(p => p.propertiesAtString.Contains("override"));
                if (carryableOverride != null)
                {
                    carryableList.Remove(carryableOverride);
                    behaviourList.RemoveAll(r => carryableList.Contains(r));
                }
            }
            return behaviourList.ToArray();
        }

        private List<T> FindCarryables<T>(List<T> behaviours){
            return behaviours.Where(b => "CarryOn.Common.BlockBehaviorCarryable".Equals(b.GetType().ToString())).ToList();
        }
    }
}