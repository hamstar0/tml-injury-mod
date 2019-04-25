using HamstarHelpers.Helpers.HudHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
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
			var myplayer = (InjuryPlayer)TmlHelpers.SafelyGetModPlayer( Main.LocalPlayer, mymod, "InjuryPlayer" );
			float percent = myplayer.Logic.ComputeHarmBufferPercent( Main.LocalPlayer );

			this.DrawSubHealthAtPercent( sb, percent );
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

		private void DrawSubHealthAtPercent( SpriteBatch sb, float percent ) {
			int width = this.HeartTex.Width;
			int height = (int)( (float)this.HeartTex.Height * percent );
			var srcRect = new Rectangle( 0, 0, width, height );
			int x = 0;
			int y = 0;

			HudHelpers.GetTopHeartPosition( Main.LocalPlayer, ref x, ref y );

			var destRect = new Rectangle( x, y, width, height );

			sb.Draw( this.HeartTex, destRect, srcRect, Color.Black * 0.5f );
		}


		private void DrawHeartDropAnimationFrame( SpriteBatch sb, int frame, int maxFrames ) {
			int x = 0;
			int y = 0;

			HudHelpers.GetTopHeartPosition( Main.LocalPlayer, ref x, ref y );
			y += frame * 2;

			var rect = new Rectangle( x, y, this.HeartTex.Width, this.HeartTex.Height );
			float percentProgress = frame / maxFrames;
			float alpha = 0.5f - (percentProgress * 0.5f);

			sb.Draw( this.HeartTex, rect, Color.White * alpha );
		}


		////////////////

		public void AnimateHeartDrop() {
			this.IsAnimatingHeartDrop = true;
		}
	}
}
