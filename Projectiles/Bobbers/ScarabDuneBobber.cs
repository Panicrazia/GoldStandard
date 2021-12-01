using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldStandard.Projectiles.Bobbers
{
    class ScarabDuneBobber : BaseDuneBobber
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Scarab Fishing Bobber");
        }
    }
}
