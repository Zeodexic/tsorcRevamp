﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace tsorcRevamp.Items.Weapons.Melee {
    class BarrowBlade : ModItem {

        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Wrought with spells of a fierce power." +
                                "\nDispels the defensive shields of Artorias and the Witchking");
        }
        public override void SetDefaults() {
            item.rare = ItemRarityID.Quest; //so people know it's important
            item.damage = 26;
            item.height = 32;
            item.knockBack = 5;
            item.maxStack = 1;
            item.melee = true;
            item.scale = .9f;
            item.useAnimation = 21;
            item.UseSound = SoundID.Item1;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 21;
            item.value = PriceByRarity.Blue_1;
            item.width = 32;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {            
            target.AddBuff(ModContent.BuffType<Buffs.DispelShadow>(), 36000);
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.AddNPCBuff, number: target.whoAmI, number2: ModContent.BuffType<Buffs.DispelShadow>(), number3: 36000);
                ModPacket shadowPacket = ModContent.GetInstance<tsorcRevamp>().GetPacket();
                shadowPacket.Write((byte)tsorcPacketID.DispelShadow);
                shadowPacket.Write(target.whoAmI);
                shadowPacket.Send();
            }            
        }
        
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) {
            target.AddBuff(ModContent.BuffType<Buffs.DispelShadow>(), 36000);
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.AddNPCBuff, number: target.whoAmI, number2: ModContent.BuffType<Buffs.DispelShadow>(), number3: 36000);
                ModPacket shadowPacket = ModContent.GetInstance<tsorcRevamp>().GetPacket();
                shadowPacket.Write((byte)tsorcPacketID.DispelShadow);
                shadowPacket.Write(target.whoAmI);
                shadowPacket.Send();
            }            
        }
    }
}
