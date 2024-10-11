using Velura.Models;

namespace Velura.iOS.Helpers;

public static class Extensions
{
	public static UIColor ToUIColor(
		this Color color) =>
		UIColor.FromRGBA(color.Red, color.Green, color.Blue, color.Alpha);
}