namespace Velura.Services.Abstract;

public interface ISystemInfo
{
	string GetDeviceModel();
	
	string GetOS();
	
	int GetBatteryLevel();
}