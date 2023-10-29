using Decks;
using Plays;
using RawDealView;
using Results;

namespace Maneuvers {
    public class ManeuverEffectHandler {
        private View _view;
        private Deck _myDeck;

        public ManeuverEffectHandler(View view, Deck deck) {
            _view = view;
            _myDeck = deck;
        }

        public Result ExecuteManeuverEffect(Play currentPlay, Deck rivalsDeck) {
            string currentCardEffect = currentPlay.Card.CardEffect;
            string mySuperstarName = _myDeck.GetSuperstarName();
            string rivalSuperstarName = rivalsDeck.GetSuperstarName();

            switch(currentCardEffect) 
            {
                case "When successfully played, discard 1 card of your choice from your hand.":
                    _myDeck.ChooseToDiscard(mySuperstarName, 1);
                    break;

                case "When successfully played, opponent must discard 1 card.":
                    rivalsDeck.ChooseToDiscard(rivalSuperstarName, 1);
                    break;
                
                case "When successfully played, opponent must discard 2 cards.":
                    rivalsDeck.ChooseToDiscard(rivalSuperstarName, 2);
                    break;
                
                case "When successfully played, discard 1 card of your choice from your hand. " +
                        "Look at opponent's hand, then choose and discard 1 card from his hand.":
                    _myDeck.ChooseToDiscard(mySuperstarName, 1);
                    rivalsDeck.ChooseToDiscard(mySuperstarName, 1);
                    break;

                case "When successfully played, you must take the top card of your Arsenal " +
                        "and put it into your Ringside pile.":
                    return _myDeck.DamageMyself(1);

                case "When successfully played, you may draw 1 card.":
                    MayDraw(_myDeck, 1);
                    break;
                
                case "When successfully played, opponent must draw 1 card.":
                    _view.SayThatPlayerDrawCards(rivalSuperstarName, 1);
                    rivalsDeck.DrawCards(1);
                    break;

                case "When successfully played, you must take the top card of your Arsenal and " +
                        "put it into your Ringside pile. Opponent must discard 2 cards.": 
                    Result resultDamageMyself = _myDeck.DamageMyself(1);
                    if (resultDamageMyself == Result.GameOver) {
                        return Result.GameOver; 
                    }
                    rivalsDeck.ChooseToDiscard(rivalSuperstarName, 2);     
                    break;      

                case "When successfully played, you must take the top card of your Arsenal and " +
                        "put it into your Ringside pile. You may draw 1 card.": 
                    resultDamageMyself = _myDeck.DamageMyself(1);
                    if (resultDamageMyself == Result.GameOver) {
                        return Result.GameOver; 
                    }
                    MayDraw(_myDeck, 1);
                    break;         

                case "When successfully played, opponent must discard 1 card and you may draw 1 card.":
                    rivalsDeck.ChooseToDiscard(rivalSuperstarName, 1);
                    MayDraw(_myDeck, 1);
                    break;      
                
                case "When successfully played, shuffle 2 cards from your Ringside pile into your Arsenal.":
                    _myDeck.Shuffle(2);
                    break;
                
                case "Can only be played after a 4D or greater maneuver. " +
                        "When successfully played, opponent must discard 1 card.":
                    rivalsDeck.ChooseToDiscard(rivalSuperstarName, 1);
                    break;

                case "May not be reversed. When successfully played, opponent must discard 2 cards.":
                    rivalsDeck.ChooseToDiscard(rivalSuperstarName, 2);
                    break;
            }

            return Result.None;
        }

        private void MayDraw(Deck myDeck, int amount) {
            string mySuperstarName = myDeck.GetSuperstarName();
            int amountOfCardsToBeDrawed = _view.AskHowManyCardsToDrawBecauseOfACardEffect(mySuperstarName, amount);
            _view.SayThatPlayerDrawCards(mySuperstarName, amountOfCardsToBeDrawed);
            myDeck.DrawCards(amountOfCardsToBeDrawed);
        }
    }
}