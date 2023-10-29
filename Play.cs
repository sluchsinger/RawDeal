using RawDealView.Formatters;
using Cards;

namespace Plays {
    public class Play : IViewablePlayInfo
    {
        public Card Card;
        public IViewableCardInfo CardInfo { get; }
        public String PlayedAs { get; }
        public int Index;
        public Play(Card card, int index, string playedAs) {
            Card = card;
            Index = index;
            CardInfo = card;
            PlayedAs = playedAs.ToUpper();           
        }
    }
}