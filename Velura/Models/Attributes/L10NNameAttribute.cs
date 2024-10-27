using Velura.Helpers;

namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class L10NNameAttribute(
	string name) : NameAttribute(name.L10N());