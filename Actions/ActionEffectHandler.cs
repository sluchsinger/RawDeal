using Decks;
using Cards;
using Plays;
using Results;
using RawDealView;

namespace Actions {
    public class ActionEffectHandler {
        private View _view;
        private Deck _myDeck;

        public ActionEffectHandler(View view, Deck deck) {
            _view = view;
            _myDeck = deck;
        }

        public Result ExecuteActionEffect(Play currentPlay, Deck rivalsDeck) {
            Card currentCard = currentPlay.Card;
            string currentCardEffect = currentCard.CardEffect;

            switch(currentCardEffect)
            {
                case "As an action, you may discard this card to draw 1 card. " +
                        "Doing this will not cause any damage to opponent.":
                    DiscardThisCardToDrawOne(currentPlay);
                    break;

                case "As an action, this card is -30F and -25D, discard this card and draw 1 card.":
                    DiscardThisCardToDrawOne(currentPlay);
                    break;

                case "Draw up to 3 cards, then discard 1 card.":
                    CanDrawAndMustDiscard(currentPlay, 3, 1);
                    break;
                
                case "Can only be played when you have 2 or more cards in your " +
                        "hand. Discard 1 card and then your opponent discards 4 cards.":
                    Discard1OpponentDiscard4(currentPlay, rivalsDeck);
                    break;
                
                case "Shuffle up to 5 cards from your Ringside pile into your Arsenal. " +
                        "Then draw 2 cards.":
                    _myDeck.PassCardFromHandToRingArea(currentPlay.Index);
                    ShuffleAndDraw(5, 2);
                    break;
                
                case "Shuffle any 2 cards from your Ringside pile back into your Arsenal. Then " +
                        "draw 1 card.":
                    _myDeck.PassCardFromHandToRingArea(currentPlay.Index);
                    ShuffleAndDraw(2, 1);
                    break;
                
                case "As an action, if your next card played is a Grapple maneuver, " +
                        "declare whether it will be +4D or your opponent's reversal to it will be +8F. " +
                        "As a reversal, may only reverse the card titled Jockeying for Position. " +
                        "If so, you end opponent's turn; and if your next card played on your turn is a " +
                        "Grapple maneuver, declare whether it will be +4D " +
                        "or your opponent's reversal to it will be +8F.":
                    _myDeck.PassCardFromHandToRingArea(currentPlay.Index);
                    return Result.MyselfJockeyingForPosition;

                default:
                    break;
            }

            return Result.None;
        }

        private void DiscardThisCardToDrawOne(Play currentPlay) {
            _view.SayThatPlayerMustDiscardThisCard(_myDeck.GetSuperstarName(), currentPlay.Card.Title);
            _myDeck.DiscardCard(currentPlay.Index);
            _view.SayThatPlayerDrawCards(_myDeck.GetSuperstarName(), 1);
            _myDeck.PickCardToTheHand();
        }

        private void CanDrawAndMustDiscard(Play currentPlay, int maxToDraw, int amountToDiscard) {
            string mySuperstarName = _myDeck.GetSuperstarName();
            int amountToDraw = _view.AskHowManyCardsToDrawBecauseOfACardEffect(mySuperstarName, maxToDraw);
            _view.SayThatPlayerDrawCards(mySuperstarName, amountToDraw);
            _myDeck.DrawCards(amountToDraw);
            _myDeck.PassCardFromHandToRingArea(currentPlay.Index);
            _myDeck.ChooseToDiscard(mySuperstarName, amountToDiscard);
        }

        private void Discard1OpponentDiscard4(Play currentPlay, Deck rivalsDeck) {
            string mySuperstarName = _myDeck.GetSuperstarName();
            string rivalSuperstarName = rivalsDeck.GetSuperstarName();
            _myDeck.PassCardFromHandToRingArea(currentPlay.Index);
            _myDeck.ChooseToDiscard(mySuperstarName, 1);
            rivalsDeck.ChooseToDiscard(rivalSuperstarName, 4);
        }

        private void ShuffleAndDraw(int amountToShuffle, int amountToDraw) {
            string mySuperstarName = _myDeck.GetSuperstarName();
            _myDeck.Shuffle(amountToShuffle);
            _view.SayThatPlayerDrawCards(mySuperstarName, amountToDraw);
            _myDeck.DrawCards(amountToDraw);
        }
    }
}