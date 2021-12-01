using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace GoldStandard.Items.FishingPoles
{
    class CloudFishingRod : BaseCloudFishingPole
    {
        public override int GoldStandardFishingPower => 20;
        public override int LineOffsetX => 54;
        public override int LineOffsestY => 34;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Cloud Fishing Rod");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = mod.ProjectileType("CloudBobber");
            item.shootSpeed = 7f;
        }
    }
}
