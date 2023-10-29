using Cards;
using Decks;
using Dictionaries;

namespace DeckConstruction {
    public class DeckValidator {

        private List<string> _uniqueCardsInTheDeck = new List<string>();
        private StringIntDictionary _subtypesInTheDeck = new StringIntDictionary();
        private string _attitude;     
        private Deck _deck;
        private string[] _superstarsLogos = {"StoneCold", "Undertaker", "Mankind", "HHH", "TheRock", "Kane", "Jericho"};

        public DeckValidator(Deck deck) {
            _deck = deck;
        }

        public void AddCard(Card cardToAdd) {
            if (cardToAdd != null && CardIsValidToAdd(cardToAdd)) {
                _deck.AddCard(cardToAdd);
            }

        }   
        private bool CardIsValidToAdd(Card cardToCheck) {
            if (!LogoIsValid(cardToCheck, _superstarsLogos)) {
                return false;
            }

            if (!AttitudeIsValid(cardToCheck)) {
                return false;
            }

            bool subtypeIsValid = SubtypeIsValid(cardToCheck);

            if (!subtypeIsValid) {
                return false;
            } else if (subtypeIsValid) {
                return true;
            }
            
            return true;
        }

        private bool LogoIsValid(Card cardToCheck, string[] superstarLogos) {
            string superstarLogo = _deck.GetSuperstarLogo();

            foreach (var logo in _superstarsLogos) {
                if (cardToCheck.Subtypes.Contains(logo) && superstarLogo != logo) {
                    return false;
                }
            }
            return true;
        }

        private bool AttitudeIsValid(Card cardToCheck) {
            bool isHeel = cardToCheck.Subtypes.Contains("Heel");
            bool isFace = cardToCheck.Subtypes.Contains("Face");

            if (isFace && _attitude == "Heel") {
                return false;
            } else if (isHeel && _attitude == "Face") {
                return false;
            }

            if (isHeel) {
                _attitude = "Heel";
            } else if (isFace) {
                _attitude = "Face";
            }
            return true;
        }

        private bool SubtypeIsValid(Card cardToCheck) {
            bool isUnique = cardToCheck.Subtypes.Contains("Unique");
            bool isSetup = cardToCheck.Subtypes.Contains("SetUp");
            if (isSetup) {
                return true;
            } else if (isUnique) {
                bool isValid = UniqueCardIsNotInTheDeck(cardToCheck);
                return isValid;
            } else {
                bool isValid = IsThreeOrLessFromThisCard(cardToCheck);
                return isValid;
            }
        }

        private bool UniqueCardIsNotInTheDeck(Card cardToCheck) {
            if (!_uniqueCardsInTheDeck.Contains(cardToCheck.Title)) {
                _uniqueCardsInTheDeck.Add(cardToCheck.Title);
                return true;
            } else {
                return false;
            }
        }

        private bool IsThreeOrLessFromThisCard(Card cardToCheck) {
            if (_subtypesInTheDeck.ContainsKey(cardToCheck.Title)) {
                int amountOfCards = _subtypesInTheDeck.GetValue(cardToCheck.Title);
                if (amountOfCards < 3) {
                    _subtypesInTheDeck.SetValue(cardToCheck.Title, amountOfCards + 1);
                    return true;
                }
            } else {
                _subtypesInTheDeck.Add(cardToCheck.Title, 1);
                return true;
            }
            return false;            
        }
    }
}