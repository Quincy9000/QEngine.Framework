using Microsoft.Xna.Framework;

namespace QEngine
{
	public struct QColor
	{
		Color value;

		public int A => value.A;

		public int R => value.R;

		public int G => value.G;

		public int B => value.B;

		public static implicit operator Color(QColor c)
		{
			return c.value;
		}

		public static implicit operator QColor(Color c)
		{
			return new QColor(c);
		}

		public static bool operator ==(QColor left, QColor r)
		{
			return left.value == r.value;
		}

		public static bool operator !=(QColor left, QColor r)
		{
			return left.value != r.value;
		}

		public static QColor operator *(QColor c, float f)
		{
			return new QColor(c.value * f);
		}

		internal QColor(Color c)
		{
			value = c;
		}

		internal QColor(uint u)
		{
			value = new Color(u);
		}

		public QColor(int r = 255, int g = 255, int b = 255, int a = 255)
		{
			value = new Color(r, g, b, a);
		}

		static QColor()
		{
			TransparentBlack = new QColor(0U);
			Transparent = new QColor(0U);
			AliceBlue = new QColor(4294965488U);
			AntiqueWhite = new QColor(4292340730U);
			Aqua = new QColor(4294967040U);
			Aquamarine = new QColor(4292149119U);
			Azure = new QColor(4294967280U);
			Beige = new QColor(4292670965U);
			Bisque = new QColor(4291093759U);
			Black = new QColor(4278190080U);
			BlanchedAlmond = new QColor(4291685375U);
			Blue = new QColor(4294901760U);
			BlueViolet = new QColor(4293012362U);
			Brown = new QColor(4280953509U);
			BurlyWood = new QColor(4287084766U);
			CadetBlue = new QColor(4288716383U);
			Chartreuse = new QColor(4278255487U);
			Chocolate = new QColor(4280183250U);
			Coral = new QColor(4283465727U);
			CornflowerBlue = new QColor(4293760356U);
			Cornsilk = new QColor(4292671743U);
			Crimson = new QColor(4282127580U);
			Cyan = new QColor(4294967040U);
			DarkBlue = new QColor(4287299584U);
			DarkCyan = new QColor(4287335168U);
			DarkGoldenrod = new QColor(4278945464U);
			DarkGray = new QColor(4289309097U);
			DarkGreen = new QColor(4278215680U);
			DarkKhaki = new QColor(4285249469U);
			DarkMagenta = new QColor(4287299723U);
			DarkOliveGreen = new QColor(4281297749U);
			DarkOrange = new QColor(4278226175U);
			DarkOrchid = new QColor(4291572377U);
			DarkRed = new QColor(4278190219U);
			DarkSalmon = new QColor(4286224105U);
			DarkSeaGreen = new QColor(4287347855U);
			DarkSlateBlue = new QColor(4287315272U);
			DarkSlateGray = new QColor(4283387695U);
			DarkTurquoise = new QColor(4291939840U);
			DarkViolet = new QColor(4292018324U);
			DeepPink = new QColor(4287829247U);
			DeepSkyBlue = new QColor(4294950656U);
			DimGray = new QColor(4285098345U);
			DodgerBlue = new QColor(4294938654U);
			Firebrick = new QColor(4280427186U);
			FloralWhite = new QColor(4293982975U);
			ForestGreen = new QColor(4280453922U);
			Fuchsia = new QColor(4294902015U);
			Gainsboro = new QColor(4292664540U);
			GhostWhite = new QColor(4294965496U);
			Gold = new QColor(4278245375U);
			Goldenrod = new QColor(4280329690U);
			Gray = new QColor(4286611584U);
			Green = new QColor(4278222848U);
			GreenYellow = new QColor(4281335725U);
			Honeydew = new QColor(4293984240U);
			HotPink = new QColor(4290013695U);
			IndianRed = new QColor(4284243149U);
			Indigo = new QColor(4286709835U);
			Ivory = new QColor(4293984255U);
			Khaki = new QColor(4287424240U);
			Lavender = new QColor(4294633190U);
			LavenderBlush = new QColor(4294308095U);
			LawnGreen = new QColor(4278254716U);
			LemonChiffon = new QColor(4291689215U);
			LightBlue = new QColor(4293318829U);
			LightCoral = new QColor(4286611696U);
			LightCyan = new QColor(4294967264U);
			LightGoldenrodYellow = new QColor(4292016890U);
			LightGray = new QColor(4292072403U);
			LightGreen = new QColor(4287688336U);
			LightPink = new QColor(4290885375U);
			LightSalmon = new QColor(4286226687U);
			LightSeaGreen = new QColor(4289376800U);
			LightSkyBlue = new QColor(4294626951U);
			LightSlateGray = new QColor(4288252023U);
			LightSteelBlue = new QColor(4292789424U);
			LightYellow = new QColor(4292935679U);
			Lime = new QColor(4278255360U);
			LimeGreen = new QColor(4281519410U);
			Linen = new QColor(4293325050U);
			Magenta = new QColor(4294902015U);
			Maroon = new QColor(4278190208U);
			MediumAquamarine = new QColor(4289383782U);
			MediumBlue = new QColor(4291624960U);
			MediumOrchid = new QColor(4292040122U);
			MediumPurple = new QColor(4292571283U);
			MediumSeaGreen = new QColor(4285641532U);
			MediumSlateBlue = new QColor(4293814395U);
			MediumSpringGreen = new QColor(4288346624U);
			MediumTurquoise = new QColor(4291613000U);
			MediumVioletRed = new QColor(4286911943U);
			MidnightBlue = new QColor(4285536537U);
			MintCream = new QColor(4294639605U);
			MistyRose = new QColor(4292994303U);
			Moccasin = new QColor(4290110719U);
			MonoGameOrange = new QColor(4278205671U);
			NavajoWhite = new QColor(4289584895U);
			Navy = new QColor(4286578688U);
			OldLace = new QColor(4293326333U);
			Olive = new QColor(4278222976U);
			OliveDrab = new QColor(4280520299U);
			Orange = new QColor(4278232575U);
			OrangeRed = new QColor(4278207999U);
			Orchid = new QColor(4292243674U);
			PaleGoldenrod = new QColor(4289390830U);
			PaleGreen = new QColor(4288215960U);
			PaleTurquoise = new QColor(4293848751U);
			PaleVioletRed = new QColor(4287852763U);
			PapayaWhip = new QColor(4292210687U);
			PeachPuff = new QColor(4290370303U);
			Peru = new QColor(4282353101U);
			Pink = new QColor(4291543295U);
			Plum = new QColor(4292714717U);
			PowderBlue = new QColor(4293320880U);
			Purple = new QColor(4286578816U);
			Red = new QColor(4278190335U);
			RosyBrown = new QColor(4287598524U);
			RoyalBlue = new QColor(4292962625U);
			SaddleBrown = new QColor(4279453067U);
			Salmon = new QColor(4285694202U);
			SandyBrown = new QColor(4284523764U);
			SeaGreen = new QColor(4283927342U);
			SeaShell = new QColor(4293850623U);
			Sienna = new QColor(4281160352U);
			Silver = new QColor(4290822336U);
			SkyBlue = new QColor(4293643911U);
			SlateBlue = new QColor(4291648106U);
			SlateGray = new QColor(4287660144U);
			Snow = new QColor(4294638335U);
			SpringGreen = new QColor(4286578432U);
			SteelBlue = new QColor(4290019910U);
			Tan = new QColor(4287411410U);
			Teal = new QColor(4286611456U);
			Thistle = new QColor(4292394968U);
			Tomato = new QColor(4282868735U);
			Turquoise = new QColor(4291878976U);
			Violet = new QColor(4293821166U);
			Wheat = new QColor(4289978101U);
			White = new QColor(uint.MaxValue);
			WhiteSmoke = new QColor(4294309365U);
			Yellow = new QColor(4278255615U);
			YellowGreen = new QColor(4281519514U);
		}

		public static QColor TransparentBlack { get; }

		public static QColor Transparent { get; }

		public static QColor AliceBlue { get; }

		public static QColor AntiqueWhite { get; }

		public static QColor Aqua { get; }

		public static QColor Aquamarine { get; }

		public static QColor Azure { get; }

		public static QColor Beige { get; }

		public static QColor Bisque { get; }

		public static QColor Black { get; }

		public static QColor BlanchedAlmond { get; }

		public static QColor Blue { get; }

		public static QColor BlueViolet { get; }

		public static QColor Brown { get; }

		public static QColor BurlyWood { get; }

		public static QColor CadetBlue { get; }

		public static QColor Chartreuse { get; }

		public static QColor Chocolate { get; }

		public static QColor Coral { get; }

		public static QColor CornflowerBlue { get; }

		public static QColor Cornsilk { get; }

		public static QColor Crimson { get; }

		public static QColor Cyan { get; }

		public static QColor DarkBlue { get; }

		public static QColor DarkCyan { get; }

		public static QColor DarkGoldenrod { get; }

		public static QColor DarkGray { get; }

		public static QColor DarkGreen { get; }

		public static QColor DarkKhaki { get; }

		public static QColor DarkMagenta { get; }

		public static QColor DarkOliveGreen { get; }

		public static QColor DarkOrange { get; }

		public static QColor DarkOrchid { get; }

		public static QColor DarkRed { get; }

		public static QColor DarkSalmon { get; }

		public static QColor DarkSeaGreen { get; }

		public static QColor DarkSlateBlue { get; }

		public static QColor DarkSlateGray { get; }

		public static QColor DarkTurquoise { get; }

		public static QColor DarkViolet { get; }

		public static QColor DeepPink { get; }

		public static QColor DeepSkyBlue { get; }

		public static QColor DimGray { get; }

		public static QColor DodgerBlue { get; }

		public static QColor Firebrick { get; }

		public static QColor FloralWhite { get; }

		public static QColor ForestGreen { get; }

		public static QColor Fuchsia { get; }

		public static QColor Gainsboro { get; }

		public static QColor GhostWhite { get; }

		public static QColor Gold { get; }

		public static QColor Goldenrod { get; }

		public static QColor Gray { get; }

		public static QColor Green { get; }

		public static QColor GreenYellow { get; }

		public static QColor Honeydew { get; }

		public static QColor HotPink { get; }

		public static QColor IndianRed { get; }

		public static QColor Indigo { get; }

		public static QColor Ivory { get; }

		public static QColor Khaki { get; }

		public static QColor Lavender { get; }

		public static QColor LavenderBlush { get; }

		public static QColor LawnGreen { get; }

		public static QColor LemonChiffon { get; }

		public static QColor LightBlue { get; }

		public static QColor LightCoral { get; }

		public static QColor LightCyan { get; }

		public static QColor LightGoldenrodYellow { get; }

		public static QColor LightGray { get; }

		public static QColor LightGreen { get; }

		public static QColor LightPink { get; }

		public static QColor LightSalmon { get; }

		public static QColor LightSeaGreen { get; }

		public static QColor LightSkyBlue { get; }

		public static QColor LightSlateGray { get; }

		public static QColor LightSteelBlue { get; }

		public static QColor LightYellow { get; }

		public static QColor Lime { get; }

		public static QColor LimeGreen { get; }

		public static QColor Linen { get; }

		public static QColor Magenta { get; }

		public static QColor Maroon { get; }

		public static QColor MediumAquamarine { get; }

		public static QColor MediumBlue { get; }

		public static QColor MediumOrchid { get; }

		public static QColor MediumPurple { get; }

		public static QColor MediumSeaGreen { get; }

		public static QColor MediumSlateBlue { get; }

		public static QColor MediumSpringGreen { get; }

		public static QColor MediumTurquoise { get; }

		public static QColor MediumVioletRed { get; }

		public static QColor MidnightBlue { get; }

		public static QColor MintCream { get; }

		public static QColor MistyRose { get; }

		public static QColor Moccasin { get; }

		public static QColor MonoGameOrange { get; }

		public static QColor NavajoWhite { get; }

		public static QColor Navy { get; }

		public static QColor OldLace { get; }

		public static QColor Olive { get; }

		public static QColor OliveDrab { get; }

		public static QColor Orange { get; }

		public static QColor OrangeRed { get; }

		public static QColor Orchid { get; }

		public static QColor PaleGoldenrod { get; }

		public static QColor PaleGreen { get; }

		public static QColor PaleTurquoise { get; }

		public static QColor PaleVioletRed { get; }

		public static QColor PapayaWhip { get; }

		public static QColor PeachPuff { get; }

		public static QColor Peru { get; }

		public static QColor Pink { get; }

		public static QColor Plum { get; }

		public static QColor PowderBlue { get; }

		public static QColor Purple { get; }

		public static QColor Red { get; }

		public static QColor RosyBrown { get; }

		public static QColor RoyalBlue { get; }

		public static QColor SaddleBrown { get; }

		public static QColor Salmon { get; }

		public static QColor SandyBrown { get; }

		public static QColor SeaGreen { get; }

		public static QColor SeaShell { get; }

		public static QColor Sienna { get; }

		public static QColor Silver { get; }

		public static QColor SkyBlue { get; }

		public static QColor SlateBlue { get; }

		public static QColor SlateGray { get; }

		public static QColor Snow { get; }

		public static QColor SpringGreen { get; }

		public static QColor SteelBlue { get; }

		public static QColor Tan { get; }

		public static QColor Teal { get; }

		public static QColor Thistle { get; }

		public static QColor Tomato { get; }

		public static QColor Turquoise { get; }

		public static QColor Violet { get; }

		public static QColor Wheat { get; }

		public static QColor White { get; }

		public static QColor WhiteSmoke { get; }

		public static QColor Yellow { get; }

		public static QColor YellowGreen { get; }
	}
}