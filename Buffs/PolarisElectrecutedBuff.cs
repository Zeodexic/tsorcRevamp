﻿using Terraria;
using Terraria.ModLoader;
using tsorcRevamp.NPCs;

namespace tsorcRevamp.Buffs
{
    public class PolarisElectrocutedBuff : ModBuff
    {
        //Generic texture since this buff is enemy-only
        public override string Texture => "tsorcRevamp/Buffs/ArmorDrug";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Electrocuted!");
            Description.SetDefault("Losing life");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<tsorcRevampGlobalNPC>().PolarisElectrocutedEffect = true;
        }
    }
}
