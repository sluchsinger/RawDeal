using Cards;

namespace Lists {
    public class CardList {
        private List<Card> _list;
        public CardList() {
            _list = new List<Card>();
        }

        public void Add(Card card) {
            _list.Add(card);
        }

        public Card GetByIndex(int index) {
            return _list[index];
        }

        public void RemoveAt(int index) {
            _list.RemoveAt(index);
        }

        public int Count() {
            return _list.Count();
        }

        public List<Card> GetList() {
            return _list;
        }

        public Card Last() {
            return _list.Last();
        }

        public void Insert(int index, Card card) {
            _list.Insert(index, card);
        }
    }
}