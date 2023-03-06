﻿using Terraria;
using Terraria.ModLoader;

namespace tsorcRevamp.Buffs.Runeterra
{
    class NightbringerThrustCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Thrusting Cooldown");
            // Description.SetDefault("You can't thrust until this runs out");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}
