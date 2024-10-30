using Velura.Enums;

namespace Velura.Services.Abstract;

public interface IThemeManager
{
	void Set(
		ThemeMode mode);
	
	ThemeMode Get();
}