using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace GoldStandard.Items
{
    class BaseBaitItem : ModItem
    {
		private string displayName;
		private string texturePath;

		public override bool CloneNewInstances => true;
		public override string Texture => texturePath;

		public override bool Autoload(ref string name)
		{
			return false;
		}

		public BaseBaitItem(int baitNumber, string displayName, string texturePath)
		{
			item.bait = baitNumber;
			this.displayName = displayName;
			this.texturePath = texturePath;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(displayName);
		}
	}
}
