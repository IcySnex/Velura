using Velura.Helpers;

namespace Velura.Models.Attributes;

[AttributeUsage(AttributeTargets.All)]
public sealed class L10NDetailsAttribute(
	string nameKey,
	string descriptionKey) : DetailsAttribute(nameKey.L10N(), descriptionKey.L10N());