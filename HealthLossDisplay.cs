using HamstarHelpers.HudHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;


namespace Injury {
	class HealthLossDisplay {
		public Texture2D HeartTex { get; private set; }
		public bool IsAnimatingHeartDrop { get; private set; }
		private int HeartDropAnimation = 0;

		
		////////////////

		public HealthLossDisplay() {
			this.IsAnimatingHeartDrop = false;
			
			if( Main.netMode != 2 ) {   // Not server
				this.HeartTex = ModLoader.GetTexture( "Terraria/Heart" );
			}
		}

		////////////////

		public void DrawSubHealth( InjuryMod mymod, SpriteBatch sb ) {
			int x = 0;
			int y = 0;
			var myplayer = Main.LocalPlayer.GetModPlayer<InjuryPlayer>();
			float percent = myplayer.ComputeHarmBufferPercent();
			var src_rect = new Rectangle( 0, 0, this.HeartTex.Width, (int)((float)this.HeartTex.Height * percent) );
			var dest_rect = new Rectangle( x, y, this.HeartTex.Width, this.HeartTex.Height );

			HudHelpers.GetTopHeartPosition( Main.LocalPlayer, ref x, ref y );

			sb.Draw( this.HeartTex, dest_rect, src_rect, Color.Black );
		}


		public void DrawCurrentHeartDropAnimation( InjuryMod mymod, SpriteBatch sb ) {
			if( !this.IsAnimatingHeartDrop ) { return; }

			this.DrawHeartDropAnimationFrame( sb, this.HeartDropAnimation++, 32 );

			if( this.HeartDropAnimation > 16 ) {
				this.HeartDropAnimation = 0;
				this.IsAnimatingHeartDrop = false;
			}
		}


		////////////////

		private void DrawHeartDropAnimationFrame( SpriteBatch sb, int frame, int maxframes ) {
			int x = 0;
			int y = 0;

			HudHelpers.GetTopHeartPosition( Main.LocalPlayer, ref x, ref y );
			y += frame * 2;

			var rect = new Rectangle( x, y, this.HeartTex.Width, this.HeartTex.Height );
			float percent_progress = frame / maxframes;
			float alpha = 0.5f - (percent_progress * 0.5f);

			sb.Draw( this.HeartTex, rect, Color.White * alpha );
		}


		////////////////

		public void AnimateHeartDrop() {
			this.IsAnimatingHeartDrop = true;
		}
	}
}
