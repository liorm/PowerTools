using System;
using ReactiveUI.Xaml;

namespace LiorTech.PowerTools.Commanding
{
	public static class AugmentedReactiveCommandMixins
    {
        public static AugmentedReactiveCommand Augment(this ReactiveCommand a_this, CommandDescriptionBase a_commandDescriptionBase)
        {
            var newCmd = new AugmentedReactiveCommand(a_commandDescriptionBase, a_this.CanExecuteObservable);
            a_this.Subscribe(newCmd.Execute);
            return newCmd;
        }

        public static AugmentedReactiveCommand Augment(this ReactiveCommand a_this, CommandDescriptionBase a_commandDescriptionBase, bool a_hasImageResource)
        {
            var newCmd = new AugmentedReactiveCommand(a_commandDescriptionBase, a_hasImageResource, a_this.CanExecuteObservable);
            a_this.Subscribe(newCmd.Execute);
            return newCmd;
        }

        public static AugmentedReactiveCommand Augment(this ReactiveCommand a_this, CommandDescriptionBase a_commandDescriptionBase, Uri a_imageUriOverride)
        {
            var newCmd = new AugmentedReactiveCommand(a_commandDescriptionBase, a_imageUriOverride, a_this.CanExecuteObservable);
            a_this.Subscribe(newCmd.Execute);
            return newCmd;
        }
    }
}