using Cards;
using Players;
using SuperstarCards;
using Plays;
using RawDealView;
using RawDealView.Formatters;
using Reversals;
using Results;
using Lists;
using Extra;
using CustomFormatters;
using Actions;
using Maneuvers;

namespace Decks {
    public class Deck {
        private CardList _arsenal = new CardList();
        private SuperstarCard _superstar;
        private Player _player;
        private CardList _hand = new CardList();
        private CardList _ringSide = new CardList();
        private CardList _ringArea = new CardList();
        private View _view;
        private CustomFormatter _customFormatter;
        private ManeuverChecker _maneuverChecker;
        private ActionChecker _actionChecker;
        private ReversalChecker _reversalChecker;
        public Deck(SuperstarCard superstar, View view) {
            _superstar = superstar;
            _view = view;
            _customFormatter = new CustomFormatter(_view);
        }

        public void AssignPlayer(Player player) {
            _player = player;
            InitializeCheckers();
        }

        private void InitializeCheckers() {
            _maneuverChecker = new ManeuverChecker(_player);
            _actionChecker = new ActionChecker(_player);
            _reversalChecker = new ReversalChecker(_player);
        }

        public int GetArsenalCount() {
            return _arsenal.Count();
        }

        public int GetRingSideCount() {
            return _ringSide.Count();
        }

        public CardList GetRingSideList() {
            return _ringSide;
        }

        public CardList GetRingAreaList() {
            return _ringArea;
        }

        public CardList GetHandList() {
            return _hand;
        }

        public string GetSuperstarName() {
            return _superstar.Name;
        }

        public string GetSuperstarLogo() {
            return _superstar.Logo;
        }

        public string GetSuperstarAbility() {
            return _superstar.SuperstarAbility;
        }

        public int GetSuperstarValue() {
            return _superstar.SuperstarValue;
        }

        public void AddCard(Card cardToAdd) {
            _arsenal.Add(cardToAdd);
        }

        public void InitializeHand() {
            for (int i = 0; i < _superstar.HandSize; i++) {
                Card card = _arsenal.Last();
                _hand.Add(card);
                _arsenal.RemoveAt(_arsenal.Count() - 1);
            }
        }

        public void PickCardToTheHand() {
            Card card = _arsenal.Last();
            _hand.Add(card);
            _arsenal.RemoveAt(_arsenal.Count() - 1);
        }

        public void DrawCards(int amount) {
            for(int i = 0; i < amount; i++) {
                PickCardToTheHand();
            }
        }

        public void Shuffle(int amount) {
            string superstarName = GetSuperstarName();
            for(int i = amount; i > 0; i--) {
                List<string> formattedRingSide = _customFormatter.GetFormattedCards(_ringSide);
                int indexCardToRecover = _view.AskPlayerToSelectCardsToRecover(superstarName, i, formattedRingSide);
                RecoverCardFromRingSideToArsenal(indexCardToRecover);
            }
        }

        public void PassCardFromHandToArsenal(int index) {
            Card cardToPass = _hand.GetByIndex(index);
            _hand.RemoveAt(index);
            _arsenal.Insert(0, cardToPass);
        }


        public void PassCardFromHandToRingArea(int index) {
            Card card = _hand.GetByIndex(index);
            _hand.RemoveAt(index);
            _ringArea.Add(card);            
        }

        public void PassLastCardOfRingAreaToRingSide() {
            Card lastCardRingArea = _ringArea.Last();
            _ringArea.RemoveAt(_ringArea.Count() - 1);
            _ringSide.Add(lastCardRingArea);
        }

        public void PassCardFromArsenalToRingSide() {
            _ringSide.Add(_arsenal.Last());
            _arsenal.RemoveAt(_arsenal.Count() - 1);
        }

        public void DiscardCard(int indexCardToDiscard) {
            Card cardToDiscard = _hand.GetByIndex(indexCardToDiscard);
            _hand.RemoveAt(indexCardToDiscard);
            _ringSide.Add(cardToDiscard);
        }

        public void ChooseToDiscard(string superstarWhoChoose, int amountToDiscard) {
            string mySuperstarName = GetSuperstarName();
            int handCount = _hand.Count();

            if (handCount == 0) {
                return;
            }

            for (int i = amountToDiscard; i > 0; i--)
            {
                int cardToDiscard = _view.AskPlayerToSelectACardToDiscard(
                    _customFormatter.GetFormattedCards(
                        _hand), mySuperstarName, superstarWhoChoose, i);
                DiscardCard(cardToDiscard);
                handCount = _hand.Count();
                if (handCount == 0) {
                    return;
                }
            }            
        }

        public Result DamageMyself(int damage) {
            string superstarName = GetSuperstarName();
            _view.SayThatPlayerDamagedHimself(superstarName, damage);
            _view.SayThatSuperstarWillTakeSomeDamage(superstarName, damage);

            if (_arsenal.Count() == 0) {
                _view.SayThatPlayerLostDueToSelfDamage(superstarName);
                return Result.GameOver;
            }

            OverturnCardsWithoutReversalOption(damage);

            return Result.None;
        }

        public void RecoverCardFromRingSideToArsenal(int indexCardToRecover) {
            Card cardToRecover = _ringSide.GetByIndex(indexCardToRecover);
            _ringSide.RemoveAt(indexCardToRecover);
            _arsenal.Insert(0, cardToRecover);
        }

        public void RecoverCardFromRingSideToHand(int indexCardToRecover) {
            Card cardToRecover = _ringSide.GetByIndex(indexCardToRecover);
            _ringSide.RemoveAt(indexCardToRecover);
            _hand.Add(cardToRecover);
        }


        public PlayList GetPossiblePlays() {
            PlayList possiblePlaysList = new PlayList();
            for (int i = 0; i < _hand.Count(); i++) {
                foreach (var type in _hand.GetByIndex(i).Types)
                {
                    Play possiblePlay = new Play(_hand.GetByIndex(i), i, type);
                    bool isPlayValid = PossiblePlayIsValid(possiblePlay);

                    if (isPlayValid) {
                        possiblePlaysList.Add(possiblePlay);
                    }
                }
            }
            return possiblePlaysList;
        }

        private bool PossiblePlayIsValid(Play possiblePlay) {
            string type = possiblePlay.PlayedAs;
            bool isFortitudeValid = Int32.Parse(possiblePlay.Card.Fortitude) <= _player.GetFortitude();

            if (type == "MANEUVER") {
                if (isFortitudeValid) {
                    return true;
                }
            } else if (type == "ACTION") {
                return ActionIsPlayable(possiblePlay);
            }          

            return false;
        }

        private bool ActionIsPlayable(Play currentPlay) {
            return _actionChecker.CheckIfCardIsPlayable(currentPlay);
        }

        public Card OverturnCard(int currentDamage, int totalDamage) {
            Card cardToOverturn = _arsenal.Last();
            _view.ShowCardOverturnByTakingDamage(Formatter.CardToString(cardToOverturn), currentDamage, totalDamage);
            PassCardFromArsenalToRingSide();
            return cardToOverturn;
        }

        public Result OverturnCards(int damage, Play currentPlay, Bonus bonus) {
            for (int i = 1; i <= damage; i++) {
                if (GetArsenalCount() == 0) {
                    return Result.GameOver;
                }
                Card cardOverturned = OverturnCard(i, damage);

                bool damageStopped = ReversalFromDeckIsValid(cardOverturned, currentPlay, bonus);
                if (damageStopped) {
                    return ReverseFromDeck(i, damage);
                }
            }
            
            return Result.None;
        }

        public Result OverturnCardsWithoutReversalOption(int damage) {
            for (int i = 1; i <= damage; i++) {
                if (GetArsenalCount() == 0) {
                    return Result.GameOver;
                }
                OverturnCard(i, damage);
            }
            
            return Result.TurnEnding;
        }

        private bool ReversalFromDeckIsValid(Card cardOverturned, Play currentPlay, Bonus bonus) {
            bool currentPlayIsNotReversable = currentPlay.Card.CardEffect.Contains("May not be reversed");
            if (cardOverturned.Types.Contains("Reversal") && !currentPlayIsNotReversable) {
                Play reversalPlay = new Play(cardOverturned, -1, "Reversal");
                return ReversalIsPlayable(currentPlay, reversalPlay, bonus);
            }
            return false;
        }

        private Result ReverseFromDeck(int currentDamage, int totalDamage) {
            _view.SayThatCardWasReversedByDeck(GetSuperstarName());
            if (currentDamage < totalDamage) {
                return Result.DrawStunValue;
            }
            return Result.TurnEnding;       
        }

        public PlayList GetHandReversalOptions(Play currentPlay, Bonus bonus) {
            PlayList reversalOptions = new PlayList();

            bool currentPlayIsNotReversable = currentPlay.Card.CardEffect.Contains("May not be reversed");
            if (currentPlayIsNotReversable) {
                return reversalOptions;
            }

            for (int i = 0; i < _hand.Count(); i++) {
                if (_hand.GetByIndex(i).Types.Contains("Reversal")) {
                    Play reversalPlay = new Play(_hand.GetByIndex(i), i, "Reversal");
                    bool isPlayable = ReversalIsPlayable(currentPlay, reversalPlay, bonus);
                    if (isPlayable) {

                        reversalOptions.Add(reversalPlay);
                    }
                }               
            }
            return reversalOptions;            
        }

        private bool ReversalIsPlayable(Play currentPlay, Play reversalPlay, Bonus bonus) {
            int reversalCardFortitude = Int32.Parse(reversalPlay.Card.Fortitude);

            bool isEffectValid = _reversalChecker.CheckIfCardIsPlayableBasedOnEffect(
                currentPlay, reversalPlay, bonus);

            int fortitudeBonus = bonus.GetFortitudeBonus(currentPlay);
            int fortitudeRequired = reversalCardFortitude + fortitudeBonus;
                
            bool isFortitudeValid = _player.GetFortitude() >= fortitudeRequired;

            if (isEffectValid && isFortitudeValid) {
                return true;
            }
            return false;
        }        
    }
}