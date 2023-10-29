using Cards;
using Decks;
using RawDealView;
using SuperstarCards;

namespace DeckConstruction {

    public class DeckConstructor {
        private View _view;

        public DeckConstructor(View view) {
            _view = view;
        }

        public Deck CreateDeck(string file, SuperstarCard[] allSuperstarCards, Card[] allCards) {
            string[] fileLines = File.ReadAllLines(file);
            string superstarCardName = fileLines[0].Substring(0, fileLines[0].IndexOf("(") - 1);
            SuperstarCard superstarCard = GetSuperstarCardFromFile(superstarCardName, allSuperstarCards);
            Deck Deck = new Deck(superstarCard, _view);
            Deck = AddCardsFromFileToDeck(Deck, allCards, fileLines);
            return Deck;
        }

        private SuperstarCard GetSuperstarCardFromFile(
            string superstarCardName, SuperstarCard[] allSuperstarCards) {
            SuperstarCard superstarCard = null;
            foreach (var superstar in allSuperstarCards) {
                if (superstar.Name == superstarCardName) {
                    superstarCard = superstar;
                }
            }
            return superstarCard;            
        }

        private Deck AddCardsFromFileToDeck(Deck deck, Card[] allCards, string[] fileLines) {
            DeckValidator deckValidator = new DeckValidator(deck);
            for (int i = 0; i < fileLines.Length; i++) {
                fileLines[i] = fileLines[i].Trim();
                Card card = null;
                foreach (var item in allCards) {
                    if (item.Title == fileLines[i]) {
                        card = item;
                    }
                }
                deckValidator.AddCard(card);
            }
            return deck;            
        }
    }
}