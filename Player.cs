using Decks;
using RawDealView;
using CustomFormatters;
using Plays;
using RawDealView.Formatters;
using Cards;
using Actions;
using Results;
using SuperstarsAbilities;
using Reversals;
using Extra;
using Lists;
using Maneuvers;

namespace Players {
    public class Player {
        private Deck _deck;
        private View _view;
        private int _fortitude;
        private CustomFormatter _customFormatter;
        private ManeuverEffectHandler _maneuverEffectHandler;
        private ActionEffectHandler _actionEffectHandler;
        private SuperstarAbility _superstarAbility;
        private ReversalEffectHandler _reversalEffectHandler;

        public Player(Deck deck, View view) {
            _deck = deck;
            _view = view;
            _fortitude = 0;
            _customFormatter = new CustomFormatter(_view);
            _maneuverEffectHandler = new ManeuverEffectHandler(_view, _deck);
            _actionEffectHandler = new ActionEffectHandler(_view, _deck);
            _reversalEffectHandler = new ReversalEffectHandler(_view, _deck);
            _superstarAbility = SuperstarAbilityFactory.Create(_deck, _view);
        }

        public Deck GetDeck() {
            return _deck;
        }

        public PlayerInfo GetPlayerInfo() {
            PlayerInfo info = new PlayerInfo(
                _deck.GetSuperstarName(), _fortitude, _deck.GetHandList().Count(), _deck.GetArsenalCount());
            return info;
        }

        public int GetFortitude() {
            return _fortitude;
        }

        public void AddFortitude(int value) {
            _fortitude += value;
        }

        public bool isMyDeckValid() {
            if (_deck.GetArsenalCount() != 60) {
                return false;
            }
            return true;
        }

        public void InitializeHand() {
            _deck.InitializeHand();
        }

        public void DrawTurnBeginning() {
            string superstarLogo = _deck.GetSuperstarLogo();
            int arsenalCount = _deck.GetArsenalCount();

            if (superstarLogo == "Mankind" && arsenalCount >= 2) {
                _deck.PickCardToTheHand();
                _deck.PickCardToTheHand();
            } else {
                _deck.PickCardToTheHand();
            }
        }

        public int GetDamageReduction() {
            string superstarLogo = _deck.GetSuperstarLogo();

            if (superstarLogo == "Mankind") {
                return 1;
            }

            return 0;
        }

        public bool CheckIfAbilityHasToBeSeenAsAnOption() {
            string superstarLogo = GetSuperstarLogo();
            int arsenalCount = _deck.GetArsenalCount();
            int handCount = _deck.GetHandList().Count();
            
            if (superstarLogo == "Undertaker" && handCount >= 2) {
                return true;
            } else if (superstarLogo == "Jericho" && handCount >= 1) {
                return true;
            } else if (superstarLogo == "StoneCold" && arsenalCount > 0) {
                return true;
            }
            return false;
        }


        public string GetSuperstarName() {
            return _deck.GetSuperstarName();
        }

        public string GetSuperstarLogo() {
            return _deck.GetSuperstarLogo();
        }

        public void SeeCards(Player rival) {
            Deck rivalsDeck = rival.GetDeck();
            List<string> formattedCards = _customFormatter.GetCardsThatUserWantToSee(_deck, rivalsDeck);
            _view.ShowCards(formattedCards);
        }

        public Result PlayACard(Player rival, Bonus bonus) {
            PlayList possiblePlaysList = _deck.GetPossiblePlays();
            List<string> formattedPlaysList = _customFormatter.GetFormattedPlays(possiblePlaysList);
            int playSelected = _view.AskUserToSelectAPlay(formattedPlaysList);

            if (playSelected == -1) {
                return Result.NotPlayed;
            }

            Play currentPlay = possiblePlaysList.GetByIndex(playSelected);
            _view.SayThatPlayerIsTryingToPlayThisCard(_deck.GetSuperstarName(),
            Formatter.PlayToString(currentPlay));

            Result resultFromHandReversal = HandleHandReverseOption(currentPlay, rival, bonus);
            if (resultFromHandReversal != Result.None) {
                return resultFromHandReversal;
            }

            _view.SayThatPlayerSuccessfullyPlayedACard();

            Result result = Result.None;

            if (currentPlay.PlayedAs == "MANEUVER") {
                result = PlayManeuver(currentPlay, rival, bonus);
            } else if (currentPlay.PlayedAs == "ACTION") {
                result = PlayAction(currentPlay, rival);
            }

            return result;
        }

        private Result PlayManeuver(Play currentPlay, Player rival, Bonus bonus) {
            Card currentCard = currentPlay.Card;
            Deck rivalsDeck = rival.GetDeck();

            _deck.PassCardFromHandToRingArea(currentPlay.Index);
            AddFortitude(Int32.Parse(currentCard.Damage));

            Result effectResult = _maneuverEffectHandler.ExecuteManeuverEffect(currentPlay, rivalsDeck);
            if (effectResult == Result.GameOver) {
                return Result.GameOver;
            }

            Result result = rival.TakeManeuverDamage(currentPlay, bonus);

            if (result == Result.DrawStunValue) {
                return HandleStunValue(currentPlay);
            }

            return result;
        }

        private Result PlayAction(Play currentPlay, Player rival) {
            Deck rivalsDeck = rival.GetDeck();
            Result result = _actionEffectHandler.ExecuteActionEffect(currentPlay, rivalsDeck);
            return result;
        }

        private Result RivalPlayReversalFromHand(int currentPlayDamage, Play reversalPlay, Player rival) {
            _view.SayThatPlayerReversedTheCard(rival.GetSuperstarName(), Formatter.PlayToString(reversalPlay));

            Deck rivalsDeck = rival.GetDeck();
            Result result = _reversalEffectHandler.ExecuteReversalEffect(reversalPlay, rivalsDeck);

            rivalsDeck.PassCardFromHandToRingArea(reversalPlay.Index);

            if (result != Result.RivalJockeyingForPosition) {
                result = TakeReversalDamage(currentPlayDamage, reversalPlay, rival);
            }
            
            return result;
        }

        private Result HandleHandReverseOption(Play currentPlay, Player rival, Bonus bonus) {
            PlayList rivalHandReversalOptions = rival.GetHandReversalOptions(currentPlay, bonus);
            if (rivalHandReversalOptions.Count() > 0) {
                int reversalChoice = _view.AskUserToSelectAReversal(rival.GetSuperstarName(),
                 _customFormatter.GetFormattedPlays(rivalHandReversalOptions));

                if (reversalChoice != -1) {
                    Play reversalPlay = rivalHandReversalOptions.GetByIndex(reversalChoice);
                    _deck.DiscardCard(currentPlay.Index);
                    int currentBonusDamage = bonus.GetDamageBonus(currentPlay);
                    int currentPlayDamage = Int32.Parse(currentPlay.Card.Damage) + currentBonusDamage;
                    return RivalPlayReversalFromHand(currentPlayDamage, reversalPlay, rival);
                }
            }

            return Result.None;
        }

        private Result HandleStunValue(Play currentPlay) {
            int stunValue = Int32.Parse(currentPlay.Card.StunValue);

            if (stunValue > 0) {
                int amountOfCardsToBeDrawed = _view.AskHowManyCardsToDrawBecauseOfStunValue(
                    GetSuperstarName(), stunValue);
                _view.SayThatPlayerDrawCards(GetSuperstarName(), amountOfCardsToBeDrawed);
                _deck.DrawCards(amountOfCardsToBeDrawed);
            }

            return Result.TurnEnding;
        }

        private PlayList GetHandReversalOptions(Play currentPlay, Bonus bonus) {
            PlayList reversalOptions = _deck.GetHandReversalOptions(currentPlay, bonus);
            return reversalOptions;
        }

        private Result TakeManeuverDamage(Play currentPlay, Bonus bonus) {
            int damageReduction = GetDamageReduction();
            int damageBonus = bonus.GetDamageBonus(currentPlay);
            int damage = Int32.Parse(currentPlay.Card.Damage) - damageReduction + damageBonus;
            
            if (damage > 0) {
                _view.SayThatSuperstarWillTakeSomeDamage(GetSuperstarName(), damage);
            }

            Result resultFromOverturn = _deck.OverturnCards(damage, currentPlay, bonus);

            return resultFromOverturn;
        }

        private Result TakeReversalDamage(int currentPlayDamage, Play reversalPlay, Player rival) {
            int damageReduction = GetDamageReduction();
            int rivalDamageReduction = rival.GetDamageReduction();
            int damage;
            int reversalPlayDamage;

            if (reversalPlay.Card.Damage == "#") {
                reversalPlayDamage = 0;
                damage = currentPlayDamage - damageReduction - rivalDamageReduction;
            } else {
                reversalPlayDamage = Int32.Parse(reversalPlay.Card.Damage);
                damage = reversalPlayDamage - damageReduction;
            }
            
            if (damage > 0) {
                _view.SayThatSuperstarWillTakeSomeDamage(GetSuperstarName(), damage);
            }

            rival.AddFortitude(reversalPlayDamage);

            return _deck.OverturnCardsWithoutReversalOption(damage);
        }

        public void UseAbility(Player rival) {
            Deck myDeck = _deck;
            Deck rivalsDeck = rival.GetDeck();
            _superstarAbility.Execute(myDeck, rivalsDeck);
        }
    }
}