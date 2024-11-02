// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;
using System.Text;
using Velura.Services.Abstract;

namespace Velura.iOS.Services;

public partial class SystemInfo : ISystemInfo
{
	[LibraryImport("libc", StringMarshalling = StringMarshalling.Utf8)]
	private static partial int sysctlbyname(
		[MarshalAs(UnmanagedType.LPStr)] string name,
		byte[] oldp,
		ref int oldlen,
		IntPtr newp,
		uint newlen);


	public string GetDeviceModel()
	{
		byte[] modelIdentifierBytes = new byte[256];
		int size = 256;

		string identifier = sysctlbyname("hw.machine", modelIdentifierBytes, ref size, IntPtr.Zero, 0) == 0 ? Encoding.ASCII.GetString(modelIdentifierBytes, 0, size).TrimEnd('\0') : "N/A";
		return $"{UIDevice.CurrentDevice.Model} ({identifier})";
	}

	public string GetOS() =>
		$"{UIDevice.CurrentDevice.SystemName} ({UIDevice.CurrentDevice.SystemVersion})";

	public int GetBatteryLevel()
	{
		UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
		float level = UIDevice.CurrentDevice.BatteryLevel;
		UIDevice.CurrentDevice.BatteryMonitoringEnabled = false;
		
		return (int)(level * 100);
	}
}