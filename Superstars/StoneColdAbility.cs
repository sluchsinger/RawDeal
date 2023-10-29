using Decks;
using RawDealView;
using CustomFormatters;

namespace SuperstarsAbilities {
    public class StoneColdAbility : SuperstarAbility {
        public StoneColdAbility(View view) {
            _view = view;
            _customFormatter = new CustomFormatter(view);
        }

        public override void Execute(Deck myDeck, Deck rivalsDeck) {
            string superstarName = myDeck.GetSuperstarName();
            string superstarAbility = myDeck.GetSuperstarAbility();
            _view.SayThatPlayerIsGoingToUseHisAbility(superstarName, superstarAbility);

            _view.SayThatPlayerDrawCards(myDeck.GetSuperstarName(), 1);
            myDeck.PickCardToTheHand();
            int indexCardToPass = _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(
                myDeck.GetSuperstarName(), _customFormatter.GetFormattedCards(myDeck.GetHandList())
            );
            myDeck.PassCardFromHandToArsenal(indexCardToPass); 
        } 
    }   
}