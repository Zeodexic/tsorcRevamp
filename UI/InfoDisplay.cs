using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Humanizer;
using System;

namespace tsorcRevamp.UI
{
	public class MinionInfoDisplay : InfoDisplay
	{
		public override bool Active() => true;
		public override string DisplayValue(ref Color displayColor)
		{
			/*float UsedMinion = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.minionSlots > 0f && proj.owner == Main.myPlayer)
				{
					UsedMinion += proj.minionSlots;
				}
			}*/

			double UsedMinion = Math.Round(Main.LocalPlayer.slotsMinions, 2);

			if (UsedMinion == 0)
			{
				displayColor = InactiveInfoTextColor;
			}

			return Language.GetTextValue("Mods.tsorcRevamp.UI.MinionInfoDisplay").FormatWith(UsedMinion, Main.LocalPlayer.maxMinions);
		}
	}

	public class SentryInfoDisplay : InfoDisplay
	{
		public override bool Active() => true;
		public override string DisplayValue(ref Color displayColor)
		{
			int UsedSentry = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.sentry && proj.owner == Main.myPlayer)
				{
					UsedSentry ++;
				}
			}

			if (UsedSentry == 0)
			{
				displayColor = InactiveInfoTextColor;
			}

			return Language.GetTextValue("Mods.tsorcRevamp.UI.SentryInfoDisplay").FormatWith(UsedSentry, Main.LocalPlayer.maxTurrets);
		}
	}
}