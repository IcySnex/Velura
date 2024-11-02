using Velura.Enums;

namespace Velura.Models;

public class FormattedText(
	string text,
	FormattedTextType type = FormattedTextType.Content)
{
	public string Text { get; } = text;

	public FormattedTextType Type { get; } = type;
}