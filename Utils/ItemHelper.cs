using Microsoft.Xna.Framework;
using Terraria;


namespace Utils {
	public static class ItemHelper {
		public static int CreateItem( Vector2 pos, int type, int stack, int width, int height, int prefix = 0 ) {
			int number = Item.NewItem( (int)pos.X, (int)pos.Y, width, height, type, stack, false, prefix, true, false );
			if( Main.netMode == 1 ) {
				NetMessage.SendData( 21, -1, -1, "", number, 1f, 0f, 0f, 0, 0, 0 );
			}
			return number;
		}
	}
}
