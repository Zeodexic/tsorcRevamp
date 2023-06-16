using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;

namespace tsorcRevamp.Projectiles.Ranged.Runeterra
{
	public class NuclearMushroom: ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
            Main.projFrames[Projectile.type] = 3;
        }

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;

			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 100 * 60;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
            Projectile.knockBack = 0f;
            Projectile.ContinuouslyUpdateDamageStats = true;
		}
        public override void OnSpawn(IEntitySource source)
        {
            Player owner = Main.player[Projectile.owner];
            //Insert Sound
        }
        public override void AI()
        {
            int frameSpeed = 5;

            Projectile.frameCounter++;

            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            // Some visuals here
            Lighting.AddLight(Projectile.Center, Color.GreenYellow.ToVector3() * 1f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[Projectile.owner];
            SoundEngine.PlaySound(SoundID.DD2_KoboldExplosion, Projectile.Center);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NuclearMushroomExplosion>(), Projectile.damage / 2, Projectile.knockBack * 10, Projectile.owner);
        }
    }
}