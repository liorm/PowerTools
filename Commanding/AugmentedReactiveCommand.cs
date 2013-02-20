using System;
using ReactiveUI.Xaml;

namespace LiorTech.PowerTools.Commanding
{
    public class AugmentedReactiveCommand : ReactiveCommand, ICommandDescriptionProvider, ICommandImageProvider
    {
        public AugmentedReactiveCommand(CommandDescriptionBase a_commandDescriptionBase, IObservable<bool> a_canExecute = null) :
            this(a_commandDescriptionBase, false, a_canExecute)
        {
        }

        public AugmentedReactiveCommand(CommandDescriptionBase a_commandDescriptionBase, bool a_hasImageResource, IObservable<bool> a_canExecute = null) :
            base(a_canExecute)
        {
            Description = a_commandDescriptionBase;
            HasImageResource = a_hasImageResource;
        }

        public AugmentedReactiveCommand(CommandDescriptionBase a_commandDescriptionBase, Uri a_imageUriOverride, IObservable<bool> a_canExecute = null) :
            base(a_canExecute)
        {
            Description = a_commandDescriptionBase;
            ImageUriOverride = a_imageUriOverride;
        }

        #region Implementation of ICommandDescriptionProvider

        public CommandDescriptionBase Description { get; private set; }

        #endregion

        #region Implementation of ICommandImageProvider

        public bool HasImageResource { get; private set; }
        public Uri ImageUriOverride { get; private set; }

        #endregion

        public override string ToString()
        {
            // For debug purposes.
            return Description.Name;
        }
    }
}
