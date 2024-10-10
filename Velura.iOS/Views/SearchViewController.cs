using Velura.iOS.Views.Abstract;
using Velura.ViewModels;

namespace Velura.iOS.Views;

public sealed class SearchViewController() : TabbedViewController<SearchViewModel>("Search", "magnifyingglass", "text.magnifyingglass")
{
}