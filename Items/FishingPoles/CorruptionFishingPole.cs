using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Items.FishingPoles
{
    class CorruptionFishingPole : BaseCloudFishingPole
    {
        public override int GoldStandardFishingPower => 33;
        public override int LineOffsetX => 42;
        public override int LineOffsestY => 32;

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 60;
            item.shoot = mod.ProjectileType("CorruptionBobber");
            item.shootSpeed = 8f;
        }
    }
}
