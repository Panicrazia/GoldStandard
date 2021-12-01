using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace GoldStandard.Items.FishingPoles
{
    class CageFishingRod : BaseCageFishingPole
    {
        public override int GoldStandardFishingPower => 20;
        public override int LineOffsetX => 40;
        public override int LineOffsestY => 20;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Cage Fishing Rod");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = mod.ProjectileType("CageBobber");
        }
    }
}
