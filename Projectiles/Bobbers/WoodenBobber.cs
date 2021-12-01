using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Projectiles.Bobbers
{
    class WoodenBobber : BaseBobber
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Bobber");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            drawOriginOffsetY = -10;
        }
    }
}
