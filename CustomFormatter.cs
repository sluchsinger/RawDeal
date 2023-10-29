using Cards;
using RawDealView;
using RawDealView.Formatters;
using Decks;
using Lists;

namespace CustomFormatters {
    public class CustomFormatter {
        private View _view;
        public CustomFormatter(View view) {
            _view = view;
        }

        public List<string> GetFormattedCards(CardList Cards) {
            List<string> FormattedCards = new List<string>();
            foreach (var card in Cards.GetList()) {
                FormattedCards.Add(Formatter.CardToString(card));
            }
            return FormattedCards;
        }

        public List<string> GetCardsThatUserWantToSee(Deck myDeck, Deck rivalsDeck) {
            int secondChoice = (int)_view.AskUserWhatSetOfCardsHeWantsToSee();
            List<string> FormattedCards = null;
            
            if (secondChoice == 0) {
                FormattedCards = GetFormattedCards(myDeck.GetHandList());
            } else if (secondChoice == 1) {
                FormattedCards = GetFormattedCards(myDeck.GetRingAreaList());
            } else if (secondChoice == 2) {
                FormattedCards = GetFormattedCards(myDeck.GetRingSideList());
            } else if (secondChoice == 3) {
                FormattedCards = GetFormattedCards(rivalsDeck.GetRingAreaList());
            } else if (secondChoice == 4) {
                FormattedCards = GetFormattedCards(rivalsDeck.GetRingSideList());
            }
            
            return FormattedCards;
        }

        public List<string> GetFormattedPlays(PlayList possiblePlaysList) {
            List<string> formattedPlaysList = new List<string>();
            foreach (var play in possiblePlaysList.GetList()) {
                if (play != null) {
                    string UppercaseType = play.Card.Types[0].ToUpper();
                    Card card = play.Card;
                    formattedPlaysList.Add(Formatter.PlayToString(play));                    
                }
            }  
            return formattedPlaysList;          
        }

    }
}