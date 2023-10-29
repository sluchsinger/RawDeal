using Decks;
using Lists;
using Players;
using Plays;

namespace Actions {
    public class ActionChecker {
        private Player _player;
        public ActionChecker(Player player) {
            _player = player;
        }

        public bool CheckIfCardIsPlayable(Play currentPlay) {
            Deck myDeck = _player.GetDeck();
            CardList hand = myDeck.GetHandList();
            int myFortitude = _player.GetFortitude();
            int cardFortitude = Int32.Parse(currentPlay.Card.Fortitude);
            bool myFortitudeIsValid = cardFortitude <= myFortitude;
            
            switch (currentPlay.Card.CardEffect)
            {
                case "As an action, this card is -30F and -25D, discard this card and draw 1 card.":
                    return true;

                case "Can only be played when you have 2 or more cards in your " +
                        "hand. Discard 1 card and then your opponent discards 4 cards.":
                    int handCount = hand.Count();
                    if (handCount >= 2 && myFortitudeIsValid) {
                        return true;
                    }
                    break;
                
                default:
                    if (myFortitudeIsValid) {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}