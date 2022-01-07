﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Steamworks;

namespace tsorcRevamp.Tiles
{
	public class FlameJet : ModTile
	{
		public override void SetDefaults()
		{
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Flame Jet");
			AddMapEntry(Color.Orange, name);
			dustType = DustID.OrangeTorch;
			Main.tileLighted[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileWaterDeath[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileSolid[Type] = true;
			Main.tileFrameImportant[Type] = true;
			drop = ModContent.ItemType<Tiles.FlameJetItem>();
		}

		public int flameJetDamage = 20;
		public int maxHeight = 32;
		
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Vector3 color = Color.Orange.ToVector3();
			r = color.X;
			g = color.Y;
			b = color.Z;
		}

        public override bool Dangersense(int i, int j, Player player)
        {
            return true;
        }

		int countdown = 0;
		//Called every 5 frames, though *which* frame it's called on is offset randomly...
        public override void NearbyEffects(int i, int j, bool closer)
		{
			if (!Main.gamePaused)
			{
				if (countdown <= 0)
				{
					int projSize = 0;
					for (int index = 1; index < maxHeight; index++)
					{
						if (!Main.tile[i, j - index].active() || (!Main.tileSolid[Main.tile[i, j - index].type]))
						{
							projSize++;
						}
						else
						{
							break;
						}
					}
					Projectile.NewProjectile(new Vector2((i * 16) + 8, (j * 16) + 8 - ((projSize * 16) / 2)), Vector2.Zero, ModContent.ProjectileType<Projectiles.FlameJet>(), flameJetDamage, 0, Main.myPlayer, 0, projSize);
					countdown = 3;
				}
				else
                {
					countdown--;
                }					
			}
        }



        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (!Main.gamePaused && Main.instance.IsActive && (!Lighting.UpdateEveryFrame || Main.rand.NextBool(4)))
			{				
				Dust.NewDust(new Vector2(i * 162, j * 16), 16, 16, DustID.OrangeTorch, 0f, 0f, 100, default(Color), .7f);							
			}
		}
	}
	public class FlameJetItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flame Jet");
			Tooltip.SetDefault("Creates a jet of flame that players can only pass by dodging");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ArmorStatue);
			item.createTile = ModContent.TileType<FlameJet>();
			item.placeStyle = 0;
		}
	}
}