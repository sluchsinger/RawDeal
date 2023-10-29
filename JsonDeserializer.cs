using System.Text.Json;
using SuperstarCards;
using Cards;
 
namespace JsonDeserializer {
    public class JSONSuperstarDeserializer {
        public static SuperstarCard[] Deserialize(string file) {
            string path = Path.Combine("data", file);
            string myJson = File.ReadAllText(path);
            var cards = JsonSerializer.Deserialize <SuperstarCard[]>(myJson);
            return cards;
        }
    }
    public class JSONCardDeserializer {
        public static Card[] Deserialize(string file) {
            string path = Path.Combine("data", file);
            string myJson = File.ReadAllText(path);
            var cards = JsonSerializer.Deserialize <Card[]>(myJson);
            return cards;
        } 
    }
}