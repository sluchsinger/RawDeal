using RawDealView;
using CustomFormatters;
using Decks;

namespace SuperstarsAbilities {
    public class SuperstarAbility {
        protected View _view;
        protected CustomFormatter _customFormatter;
        public virtual void Execute(Deck myDeck, Deck rivalsDeck) {}
    }
}