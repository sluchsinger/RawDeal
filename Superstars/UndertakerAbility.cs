using Decks;
using RawDealView;
using CustomFormatters;

namespace SuperstarsAbilities {
    public class UndertakerAbility : SuperstarAbility {
        public UndertakerAbility(View view) {
            _view = view;
            _customFormatter = new CustomFormatter(view);
        }

        public override void Execute(Deck myDeck, Deck rivalsDeck) {
            string superstarName = myDeck.GetSuperstarName();
            string superstarAbility = myDeck.GetSuperstarAbility();
            _view.SayThatPlayerIsGoingToUseHisAbility(superstarName, superstarAbility);

            int indexFirstCardToDiscard = _view.AskPlayerToSelectACardToDiscard(
                _customFormatter.GetFormattedCards(
                    myDeck.GetHandList()), myDeck.GetSuperstarName(), myDeck.GetSuperstarName(), 2
            );
            myDeck.DiscardCard(indexFirstCardToDiscard);
            int indexSecondCardToDiscard = _view.AskPlayerToSelectACardToDiscard(
                _customFormatter.GetFormattedCards(
                    myDeck.GetHandList()), myDeck.GetSuperstarName(), myDeck.GetSuperstarName(), 1
            );
            myDeck.DiscardCard(indexSecondCardToDiscard);
            int indexCardToRecover = _view.AskPlayerToSelectCardsToPutInHisHand(
                myDeck.GetSuperstarName(), 1, _customFormatter.GetFormattedCards(myDeck.GetRingSideList())
            );
            myDeck.RecoverCardFromRingSideToHand(indexCardToRecover); 
        } 
    }   
}