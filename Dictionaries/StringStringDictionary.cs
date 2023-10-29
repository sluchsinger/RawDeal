namespace Dictionaries {
    public class StringStringDictionary {
        Dictionary<string, string> _dictionary;

        public StringStringDictionary() {
            _dictionary = new Dictionary<string, string>();
        }

        public bool ContainsKey(string key) {
            return _dictionary.ContainsKey(key);
        }

        public void Add(string key, string value) {
            _dictionary.Add(key, value);
        }

        public string GetValue(string key) {
            return _dictionary[key];
        }

        public void SetValue(string key, string value) {
            _dictionary[key] = value;
        }
    }
}