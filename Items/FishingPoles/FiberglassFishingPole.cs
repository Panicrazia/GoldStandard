using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Items.FishingPoles
{
    class FiberglassFishingPole : BaseFishingPole
    {
        public override int GoldStandardFishingPower => 28;
        public override int LineOffsetX => 46;
        public override int LineOffsestY => 36;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fiberglass Fishing Pole");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 600;
            item.shoot = mod.ProjectileType("FiberglassBobber");
            item.shootSpeed = 10f;
        }
    }
}
