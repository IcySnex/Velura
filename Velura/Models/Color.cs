namespace Velura.Models;

public readonly struct Color(
	byte red,
	byte green,
	byte blue,
	byte alpha = 255)
{
	public static Color FromHex(string hex)
	{
		byte r = Convert.ToByte(hex[1..3], 16);
		byte g = Convert.ToByte(hex[3..5], 16);
		byte b = Convert.ToByte(hex[5..7], 16);
		byte a = hex.Length == 9 ? Convert.ToByte(hex[7..9], 16) : (byte)255;

		return new(r, g, b, a);
	}
	
	
	public byte Red { get; } = red;
	
	public byte Green { get; } = green;
	
	public byte Blue { get; } = blue;
	
	public byte Alpha { get; } = alpha;
}