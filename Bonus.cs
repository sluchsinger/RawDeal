using Plays;

namespace Extra {
    public class Bonus {
        private int _maneuverGrappleDamage;
        private int _maneuverGrappleFortitude;
        public Bonus() {
            _maneuverGrappleDamage = 0;
            _maneuverGrappleFortitude = 0;
        }  

        public void RestoreValues() {
            _maneuverGrappleDamage = 0;
            _maneuverGrappleFortitude = 0;
        }

        public void SetManeuverGrappleDamage(int value) {
            _maneuverGrappleDamage = value;
        }

        public void SetManeuverGrappleFortitude(int value) {
            _maneuverGrappleFortitude = value;
        }

        public int GetDamageBonus(Play currentPlay) {
            if (currentPlay.Card.Subtypes.Contains("Grapple") && currentPlay.PlayedAs == "MANEUVER") {
                return _maneuverGrappleDamage;
            }
            return 0;
        }

        public int GetFortitudeBonus(Play currentPlay) {
            if (currentPlay.Card.Subtypes.Contains("Grapple") && currentPlay.PlayedAs == "MANEUVER") {
                return _maneuverGrappleFortitude;
            }
            return 0;
        }
    }
}