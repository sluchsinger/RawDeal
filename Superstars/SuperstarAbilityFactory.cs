using Decks;
using RawDealView;

namespace SuperstarsAbilities {
    public class SuperstarAbilityFactory {
        public static SuperstarAbility Create(Deck deck, View view) {
            string superstarLogo = deck.GetSuperstarLogo();

            switch (superstarLogo)
            {
                case "Kane":
                    return new KaneAbility(view);
                case "TheRock":
                    return new TheRockAbility(view);
                case "Jericho":
                    return new JerichoAbility(view);
                case "StoneCold":
                    return new StoneColdAbility(view);
                case "Undertaker":
                    return new UndertakerAbility(view);
                default:
                    return new SuperstarAbility();
            }
        }
    }
}