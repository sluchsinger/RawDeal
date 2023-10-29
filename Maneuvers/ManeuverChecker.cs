using Players;
using Plays;

namespace Maneuvers {
    public class ManeuverChecker {
        private Player _player;
        
        public ManeuverChecker(Player player) {
            _player = player;
        }
        
        public bool CheckIfCardIsPlayableBasedOnEffect(Play currentPlay, Dictionary<string, string> lastPlay) {
            string currentCardEffect = currentPlay.Card.CardEffect;
            string lastPlayTitle = lastPlay["title"];
            int lastPlayDamage = Int32.Parse(lastPlay["damageDone"]);
            bool isLastPlayManeuver = lastPlay["playedAs"] == "MANEUVER";

            switch(currentCardEffect) {
                case "Can only be played after a 4D or greater maneuver. " +
                        "When successfully played, opponent must discard 1 card.":
                    if (isLastPlayManeuver && lastPlayDamage >= 4) {
                        return true;
                    }
                    return false;
                
                case "May not be reversed. Can only be played after a maneuver that does 5D or greater.":
                    if (isLastPlayManeuver && lastPlayDamage >= 5) {
                        return true;
                    }
                    return false;

                default:
                    return true;
            }
        }
    }
}