using Plays;

namespace Lists {
    public class PlayList {
        private List<Play> _list;
        public PlayList() {
            _list = new List<Play>();
        }

        public void Add(Play play) {
            _list.Add(play);
        }

        public Play GetByIndex(int index) {
            return _list[index];
        }

        public void RemoveAt(int index) {
            _list.RemoveAt(index);
        }

        public int Count() {
            return _list.Count();
        }

        public List<Play> GetList() {
            return _list;
        }

        public Play Last() {
            return _list.Last();
        }

        public void Insert(int index, Play play) {
            _list.Insert(index, play);
        }
    }
}