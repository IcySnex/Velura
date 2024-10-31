using Velura.Enums;

namespace Velura.Services.Abstract;

public interface IThemeManager
{
	void SetMode(
		ThemeMode mode);
	
	
	void SetPreferLargeTitles(
		bool enable);
}