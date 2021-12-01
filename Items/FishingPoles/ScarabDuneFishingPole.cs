using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Items.FishingPoles
{
    class ScarabDuneFishingPole : BaseDuneFishingPole
    {
        public override int GoldStandardFishingPower => 777;
        public override int LineOffsetX => 46;
        public override int LineOffsestY => 36;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Scarab Fishing Pole");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.value = 600;
            item.shoot = mod.ProjectileType("ScarabDuneBobber");
            item.shootSpeed = 10f;
        }
    }
}

