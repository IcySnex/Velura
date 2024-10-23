namespace Velura.iOS.Binding;

[Flags]
public enum BindingMode
{
	OneWay = 0,
	OneWayToSource = 1,
	TwoWay = 2,
	OneTime = 4
}