using Plays;
using Extra;
using Decks;
using Players;

namespace Reversals {
    public class ReversalChecker {
        private Player _player;

        public ReversalChecker(Player player) {
            _player = player;
        }

        public bool CheckIfCardIsPlayableBasedOnEffect(Play currentPlay, Play reversalPlay, Bonus bonus) {

            bool isCurrentPlayManeuver = currentPlay.PlayedAs == "MANEUVER";
            bool isCurrentPlayAction = currentPlay.PlayedAs == "ACTION";
            bool isCurrrentPlayStrike = currentPlay.Card.Subtypes.Contains("Strike");
            bool isCurrentPlayGrapple = currentPlay.Card.Subtypes.Contains("Grapple");
            bool isCurrentPlaySubmission = currentPlay.Card.Subtypes.Contains("Submission");
            bool isCurrentPlayJockeyingForPosition = currentPlay.Card.Title == "Jockeying for Position";

            int damageBonus = bonus.GetDamageBonus(currentPlay);
            int damageReduction = _player.GetDamageReduction();
            int currentDamage = Int32.Parse(currentPlay.Card.Damage) + damageBonus - damageReduction;
            
            bool isCurrentPlayDamageSevenOrLess = currentDamage <= 7;

            switch (reversalPlay.Card.CardEffect)
            {
                case "Reverse any Strike maneuver and end your opponent's turn.":
                    if (isCurrentPlayManeuver && isCurrrentPlayStrike) {
                        return true;
                    }
                    break;
                
                case "Reverse any Grapple maneuver and end your opponent's turn.":
                    if (isCurrentPlayManeuver && isCurrentPlayGrapple) {
                        return true;
                    }
                    break;

                case "Reverse any Submission maneuver and end your opponent's turn.":
                    if (isCurrentPlayManeuver && isCurrentPlaySubmission) {
                        return true;
                    }
                    break;

                case "Reverse any ACTION card being played by your opponent from his hand. " +
                        "It is unsuccessful, goes to his Ringside pile, has no effect and ends his turn.":
                    if (isCurrentPlayAction) {
                        return true;
                    }
                    break;

                case "Can only reverse a Grapple that does 7D or less. End your opponent's turn. " +
                        "# = D of maneuver card being reversed. Read as 0 when in your Ring area.":
                    if (isCurrentPlayGrapple && isCurrentPlayDamageSevenOrLess && isCurrentPlayManeuver) {
                        return true;
                    }
                    break;

                case "Can only reverse a Strike that does 7D or less. End your opponent's turn. " +
                        "# = D of maneuver card being reversed. Read as 0 when in your Ring area.":
                    if (isCurrrentPlayStrike && isCurrentPlayDamageSevenOrLess) {
                        return true;
                    }
                    break;
                
                case "May reverse any maneuver that does 7D or less. End your opponent's turn.":
                    if (isCurrentPlayManeuver && isCurrentPlayDamageSevenOrLess) {
                        return true;
                    }
                    break;

                case "Reverse any maneuver and end your opponent's turn. If played from your hand draw 1 card.":
                    if (isCurrentPlayManeuver) {
                        return true;
                    }
                    break;

                case "Reverses any maneuver and ends your opponent's turn. If played from your hand, draw 2 cards.":
                    if (isCurrentPlayManeuver) {
                        return true;
                    }
                    break;

                case "If played from your hand, may reverse the card titled Jockeying for Position. " +
                        "Opponent must discard 4 cards. End your opponent's turn. Draw 1 card.":
                    if (isCurrentPlayJockeyingForPosition) {
                        return true;
                    }
                    break;

                case "As an action, if your next card played is a Grapple maneuver, " +
                        "declare whether it will be +4D or your opponent's reversal to it will be +8F. " +
                        "As a reversal, may only reverse the card titled Jockeying for Position. " +
                        "If so, you end opponent's turn; and if your next card played on your turn is a " +
                        "Grapple maneuver, declare whether it will be +4D " +
                        "or your opponent's reversal to it will be +8F.":
                    if (isCurrentPlayJockeyingForPosition) {
                        return true;
                    }
                    break;

                default:
                    break;
            }
            return false;
        }    
    }    
}