using Decks;
using Plays;
using Results;
using RawDealView;
using CustomFormatters;

namespace Reversals
{
    public class ReversalEffectHandler
    {
        private View _view;
        private Deck _myDeck;

        public ReversalEffectHandler(View view, Deck deck)
        {
            _view = view;
            _myDeck = deck;
        }

        public Result ExecuteReversalEffect(Play reversalPlay, Deck rivalsDeck)
        {
            
            switch (reversalPlay.Card.CardEffect)
            {
                case "Reverse any maneuver and end your opponent's turn. If played from your hand draw 1 card.":
                    ReverseManeuverAndDraw(rivalsDeck, 1);
                    break;

                case "Reverses any maneuver and ends your opponent's turn. If played from your hand, draw 2 cards.":
                    ReverseManeuverAndDraw(rivalsDeck, 2);
                    break;

                case "If played from your hand, may reverse the card titled Jockeying for Position. " +
                        "Opponent must discard 4 cards. End your opponent's turn. Draw 1 card.":
                    ReverseJockeyingOpponentDiscardAndIDraw(rivalsDeck);
                    break;


                case "As an action, if your next card played is a Grapple maneuver, " +
                        "declare whether it will be +4D or your opponent's reversal to it will be +8F. " +
                        "As a reversal, may only reverse the card titled Jockeying for Position. " +
                        "If so, you end opponent's turn; and if your next card played on your turn is a " +
                        "Grapple maneuver, declare whether it will be +4D " +
                        "or your opponent's reversal to it will be +8F.":
                    return Result.RivalJockeyingForPosition;

                default:
                    break;
            }

            return Result.None;
        }

        private void ReverseManeuverAndDraw(Deck rivalsDeck, int cardAmount) {
            string rivalSuperstarName = rivalsDeck.GetSuperstarName();
            _view.SayThatPlayerDrawCards(rivalSuperstarName, cardAmount);
            rivalsDeck.DrawCards(cardAmount);
        }

        private void ReverseJockeyingOpponentDiscardAndIDraw(Deck rivalsDeck) {
            string mySuperstarName = _myDeck.GetSuperstarName();
            string rivalSuperstarName = rivalsDeck.GetSuperstarName();

            _myDeck.ChooseToDiscard(mySuperstarName, 4);

            _view.SayThatPlayerDrawCards(rivalSuperstarName, 1);
            rivalsDeck.DrawCards(1);
        }
    }
}