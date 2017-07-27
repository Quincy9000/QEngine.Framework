using System;

namespace QEngine
{
	public enum QSortOrder
	{
		/// <summary>
		/// Starts from Zero, works way up to One
		/// </summary>
		StartAtZero,

		/// <summary>
		/// Starts from One and works down to Zero
		/// </summary>
		StartAtOne,

		/// <summary>
		/// Dont care what order the sprites are rendered in, more efficient but layer issues
		/// </summary>
		DontCare
	}

	public enum QCollisionDirection
	{
		/// <summary>
		/// a is above b
		/// </summary>
		Top,
		/// <summary>
		/// a is to the left of b
		/// </summary>
		Left,
		/// <summary>
		/// a is to the right of b
		/// </summary>
		Right,
		/// <summary>
		/// a is below b
		/// </summary>
		Bottom
	}
	
	[Flags]
	public enum QCollisionCategory
	{
		None = 0,
		All = int.MaxValue,
		Cat1 = 1,
		Cat2 = 2,
		Cat3 = 4,
		Cat4 = 8,
		Cat5 = 16,
		Cat6 = 32,
		Cat7 = 64,
		Cat8 = 128,
		Cat9 = 256,
		Cat10 = 512,
		Cat11 = 1024,
		Cat12 = 2048,
		Cat13 = 4096,
		Cat14 = 8192,
		Cat15 = 16384,
		Cat16 = 32768,
		Cat17 = 65536,
		Cat18 = 131072,
		Cat19 = 262144,
		Cat20 = 524288,
		Cat21 = 1048576,
		Cat22 = 2097152,
		Cat23 = 4194304,
		Cat24 = 8388608,
		Cat25 = 16777216,
		Cat26 = 33554432,
		Cat27 = 67108864,
		Cat28 = 134217728,
		Cat29 = 268435456,
		Cat30 = 536870912,
		Cat31 = 1073741824
	}

	/// <summary>
	/// Determines how the pixels are filtered in the screen, aka ansiotrophic filtering
	/// </summary>
	public enum QFilteringState
	{
		Point,
		Ansiotrophic,
		Linear
	}

	public enum QSoundStates
	{
		Playing,
		Paused,
		Stopped,
		Idle
	}

	public enum QBodyType
	{
		/// <summary>
		/// Zero velocity, may be manually moved. Note: even static bodies have mass.
		/// </summary>
		Static,

		/// <summary>
		/// Zero mass, non-zero velocity set by user, moved by solver
		/// </summary>
		Kinematic,

		/// <summary>
		/// Positive mass, non-zero velocity determined by forces, moved by solver
		/// </summary>
		Dynamic
	}

	[Flags]
	public enum QRenderEffects
	{
		None = 0,
		FlipHorizontally = 1,
		FlipVertically = 2,
	}

	public enum QMouseStates
	{
		/// <summary>
		/// Left Mouse Button
		/// </summary>
		Left,
		/// <summary>
		/// Right Mouse Button
		/// </summary>
		Right,
		/// <summary>
		/// Middle Mouse Button, if it is clicked
		/// </summary>
		Middle,
		/// <summary>
		/// Forward Browser Mouse Button
		/// </summary>
		Forward,
		/// <summary>
		/// Backward Browser Mouse Button
		/// </summary>
		Backward,
		/// <summary>
		/// Scoll Up Mouse Wheel
		/// </summary>
		Up, 
		/// <summary>
		/// Scroll Down Mouse Wheel
		/// </summary>
		Down,
	}

	public enum QKeyStates
	{
		None = 0,
		Back = 8,
		Tab = 9,
		Enter = 13,
		Pause = 19,
		CapsLock = 20,
		Kana = 21,
		Kanji = 25,
		Escape = 27,
		ImeConvert = 28,
		ImeNoConvert = 29,
		Space = 32,
		PageUp = 33,
		PageDown = 34,
		End = 35,
		Home = 36,
		Left = 37,
		Up = 38,
		Right = 39,
		Down = 40,
		Select = 41,
		Print = 42,
		Execute = 43,
		PrintScreen = 44,
		Insert = 45,
		Delete = 46,
		Help = 47,
		D0 = 48,
		D1 = 49,
		D2 = 50,
		D3 = 51,
		D4 = 52,
		D5 = 53,
		D6 = 54,
		D7 = 55,
		D8 = 56,
		D9 = 57,
		A = 65,
		B = 66,
		C = 67,
		D = 68,
		E = 69,
		F = 70,
		G = 71,
		H = 72,
		I = 73,
		J = 74,
		K = 75,
		L = 76,
		M = 77,
		N = 78,
		O = 79,
		P = 80,
		Q = 81,
		R = 82,
		S = 83,
		T = 84,
		U = 85,
		V = 86,
		W = 87,
		X = 88,
		Y = 89,
		Z = 90,
		LeftWindows = 91,
		RightWindows = 92,
		Apps = 93,
		Sleep = 95,
		NumPad0 = 96,
		NumPad1 = 97,
		NumPad2 = 98,
		NumPad3 = 99,
		NumPad4 = 100,
		NumPad5 = 101,
		NumPad6 = 102,
		NumPad7 = 103,
		NumPad8 = 104,
		NumPad9 = 105,
		Multiply = 106,
		Add = 107,
		Separator = 108,
		Subtract = 109,
		Decimal = 110,
		Divide = 111,
		F1 = 112,
		F2 = 113,
		F3 = 114,
		F4 = 115,
		F5 = 116,
		F6 = 117,
		F7 = 118,
		F8 = 119,
		F9 = 120,
		F10 = 121,
		F11 = 122,
		F12 = 123,
		F13 = 124,
		F14 = 125,
		F15 = 126,
		F16 = 127,
		F17 = 128,
		F18 = 129,
		F19 = 130,
		F20 = 131,
		F21 = 132,
		F22 = 133,
		F23 = 134,
		F24 = 135,
		NumLock = 144,
		Scroll = 145,
		LeftShift = 160,
		RightShift = 161,
		LeftControl = 162,
		RightControl = 163,
		LeftAlt = 164,
		RightAlt = 165,
		BrowserBack = 166,
		BrowserForward = 167,
		BrowserRefresh = 168,
		BrowserStop = 169,
		BrowserSearch = 170,
		BrowserFavorites = 171,
		BrowserHome = 172,
		VolumeMute = 173,
		VolumeDown = 174,
		VolumeUp = 175,
		MediaNextTrack = 176,
		MediaPreviousTrack = 177,
		MediaStop = 178,
		MediaPlayPause = 179,
		LaunchMail = 180,
		SelectMedia = 181,
		LaunchApplication1 = 182,
		LaunchApplication2 = 183,
		OemSemicolon = 186,
		OemPlus = 187,
		OemComma = 188,
		OemMinus = 189,
		OemPeriod = 190,
		OemQuestion = 191,
		OemTilde = 192,
		ChatPadGreen = 202,
		ChatPadOrange = 203,
		OemOpenBrackets = 219,
		OemPipe = 220,
		OemCloseBrackets = 221,
		OemQuotes = 222,
		Oem8 = 223,
		OemBackslash = 226,
		ProcessKey = 229,
		OemCopy = 242,
		OemAuto = 243,
		OemEnlW = 244,
		Attn = 246,
		Crsel = 247,
		Exsel = 248,
		EraseEof = 249,
		Play = 250,
		Zoom = 251,
		Pa1 = 253,
		OemClear = 254,
	}
}

//	public enum QKeys
//	{
//		A,
//		B,
//		C,
//		D,
//		E,
//		F,
//		G,
//		H,
//		I,
//		J,
//		K,
//		L,
//		M,
//		N,
//		O,
//		P,
//		Q,
//		R,
//		S,
//		T,
//		U,
//		V,
//		W,
//		X,
//		Y,
//		Z,
//		One,
//		Two,
//		Three,
//		Four,
//		Five,
//		Six,
//		Seven,
//		Eight,
//		Nine,
//		Zero,
//		Minus,
//		Equals,
//		Backspace,
//		Tilde,
//		Tab,
//		Caps,
//		ShiftLeft,
//		ControlLeft,
//		Super,
//		AltLeft,
//		Space,
//		AltRight,
//		ControlRight,
//		Up,
//		Down,
//		Left,
//		Right,
//		ShiftRight,
//		SemiColon,
//		Comma,
//		Period,
//		SlashForward,
//		SlashBackward,
//		Apostrophe,
//		SquareBracketLeft,
//		SquareBracketRight,
//		Delete,
//		End,
//		PageDown,
//		PageUp,
//		Home,
//		Insert,
//		Escape,
//		F1,
//		F2,
//		F3,
//		F4,
//		F5,
//		F6,
//		F7,
//		F8,
//		F9,
//		F10,
//		F11,
//		F12,
//		F13,
//		F14,
//		F15,
//		F16,
//		F17,
//		F18,
//		F19,
//		F20,
//		None,
//		NumOne,
//		NumTwo,
//		NumThree,
//		NumFour,
//		NumFive,
//		NumSix,
//		NumSeven,
//		NumEight,
//		NumNine,
//		NumZero,
//	}