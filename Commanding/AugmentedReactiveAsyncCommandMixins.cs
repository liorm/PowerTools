using System;
using ReactiveUI.Xaml;

namespace LiorTech.PowerTools.Commanding
{
	public static class AugmentedReactiveAsyncCommandMixins
	{
		public static AugmentedReactiveAsyncCommand Augment(this ReactiveAsyncCommand a_this, CommandDescriptionBase a_commandDescriptionBase)
		{
			return new AugmentedReactiveAsyncCommand(a_this, a_commandDescriptionBase);
		}

		public static AugmentedReactiveAsyncCommand Augment(this ReactiveAsyncCommand a_this, CommandDescriptionBase a_commandDescriptionBase, bool a_hasImageResource)
		{
			return new AugmentedReactiveAsyncCommand(a_this, a_commandDescriptionBase, a_hasImageResource);
		}

		public static AugmentedReactiveAsyncCommand Augment(this ReactiveAsyncCommand a_this, CommandDescriptionBase a_commandDescriptionBase, Uri a_imageUriOverride)
		{
			return new AugmentedReactiveAsyncCommand(a_this, a_commandDescriptionBase, a_imageUriOverride);
		}
	}
}