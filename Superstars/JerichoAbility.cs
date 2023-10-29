using Decks;
using RawDealView;
using CustomFormatters;

namespace SuperstarsAbilities {
    public class JerichoAbility : SuperstarAbility {
        public JerichoAbility(View view) {
            _view = view;
            _customFormatter = new CustomFormatter(view);
        }

        public override void Execute(Deck myDeck, Deck rivalsDeck) {
            string superstarName = myDeck.GetSuperstarName();
            string superstarAbility = myDeck.GetSuperstarAbility();
            _view.SayThatPlayerIsGoingToUseHisAbility(superstarName, superstarAbility);
            
            int indexMyCardToDiscard = _view.AskPlayerToSelectACardToDiscard(
                _customFormatter.GetFormattedCards(
                    myDeck.GetHandList()), myDeck.GetSuperstarName(), myDeck.GetSuperstarName(), 1
            );
            myDeck.DiscardCard(indexMyCardToDiscard);
            int indexRivalsCardToDiscard = _view.AskPlayerToSelectACardToDiscard(
                _customFormatter.GetFormattedCards(
                    rivalsDeck.GetHandList()), rivalsDeck.GetSuperstarName(), rivalsDeck.GetSuperstarName(), 1
            );
            rivalsDeck.DiscardCard(indexRivalsCardToDiscard);  
        } 
    }   
}