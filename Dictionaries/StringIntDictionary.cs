namespace Dictionaries {
    public class StringIntDictionary {
        Dictionary<string, int> _dictionary;

        public StringIntDictionary() {
            _dictionary = new Dictionary<string, int>();
        }

        public bool ContainsKey(string key) {
            return _dictionary.ContainsKey(key);
        }

        public void Add(string key, int value) {
            _dictionary.Add(key, value);
        }

        public int GetValue(string key) {
            return _dictionary[key];
        }

        public void SetValue(string key, int value) {
            _dictionary[key] = value;
        }
    }
}