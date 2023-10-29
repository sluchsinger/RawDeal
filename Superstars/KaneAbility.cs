using Decks;
using RawDealView;
using CustomFormatters;

namespace SuperstarsAbilities {
    public class KaneAbility : SuperstarAbility {
        public KaneAbility(View view) {
            _view = view;
            _customFormatter = new CustomFormatter(view);
        }

        public override void Execute(Deck myDeck, Deck rivalsDeck) {
            string superstarName = myDeck.GetSuperstarName();
            string superstarAbility = myDeck.GetSuperstarAbility();
            _view.SayThatPlayerIsGoingToUseHisAbility(superstarName, superstarAbility);

            _view.SayThatSuperstarWillTakeSomeDamage(rivalsDeck.GetSuperstarName(), 1);
            rivalsDeck.OverturnCard(1, 1);
        } 
    }   
}