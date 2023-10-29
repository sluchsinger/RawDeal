using Decks;
using RawDealView;
using CustomFormatters;

namespace SuperstarsAbilities {
    public class TheRockAbility : SuperstarAbility {
        public TheRockAbility(View view) {
            _view = view;
            _customFormatter = new CustomFormatter(view);
        }

        public override void Execute(Deck myDeck, Deck rivalsDeck) {
            string superstarName = myDeck.GetSuperstarName();
            string superstarAbility = myDeck.GetSuperstarAbility();
            
            if (myDeck.GetRingSideCount() > 0) {
                bool  playerWantToUseHisAbility = _view.DoesPlayerWantToUseHisAbility("THE ROCK");
                if (playerWantToUseHisAbility) {
                    _view.SayThatPlayerIsGoingToUseHisAbility(superstarName, superstarAbility);
                    int indexCardToRecover = _view.AskPlayerToSelectCardsToRecover(
                        "THE ROCK", 1, _customFormatter.GetFormattedCards(myDeck.GetRingSideList()));
                    myDeck.RecoverCardFromRingSideToArsenal(indexCardToRecover);
                }
            }
        } 
    }   
}