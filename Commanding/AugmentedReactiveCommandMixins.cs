using System;
using ReactiveUI.Xaml;

namespace LiorTech.PowerTools.Commanding
{
	public static class AugmentedReactiveCommandMixins
    {
        public static AugmentedReactiveCommand Augment(this ReactiveCommand a_this, CommandDescriptionBase a_commandDescriptionBase)
        {
            return new AugmentedReactiveCommand(a_this, a_commandDescriptionBase);
        }

        public static AugmentedReactiveCommand Augment(this ReactiveCommand a_this, CommandDescriptionBase a_commandDescriptionBase, bool a_hasImageResource)
        {
            return new AugmentedReactiveCommand(a_this, a_commandDescriptionBase, a_hasImageResource);
        }

        public static AugmentedReactiveCommand Augment(this ReactiveCommand a_this, CommandDescriptionBase a_commandDescriptionBase, Uri a_imageUriOverride)
        {
            return new AugmentedReactiveCommand(a_this, a_commandDescriptionBase, a_imageUriOverride);
        }
    }
}