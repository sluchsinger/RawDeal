using RawDealView;
using RawDealView.Options;
using JsonDeserializer;
using SuperstarCards;
using Cards;
using DeckConstruction;
using Decks;
using Players;
using PlayerOrder;
using Results;
using Extra;
using Dictionaries;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private Card[] _cards;
    private SuperstarCard[] _superstars;
    private DeckConstructor _deckConstructor;
    private PlayerSorter _playerSorter;
    private bool _gameOver;
    private string _winner;
    private int _currentTurn;
    private bool _isTurnFirstLap;
    private bool _abilityWasUsed;
    private Bonus _bonus;
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        _cards = JSONCardDeserializer.Deserialize("cards.json");
        _superstars = JSONSuperstarDeserializer.Deserialize("superstar.json");
        _deckConstructor = new DeckConstructor(_view);
        _playerSorter = new PlayerSorter();
        _gameOver = false;
        _currentTurn = 0;
        _isTurnFirstLap = true;
        _abilityWasUsed = false;
        _bonus = new Bonus();
    }

    public void Play()
    {
        Player[] playerOrder = InitializeGame();
        if (playerOrder.Count() == 0) {
            return;
        }

        while (!_gameOver) {

            Player myself = playerOrder[_currentTurn];
            Player rival = playerOrder[1 - _currentTurn];

            PlayTurn(myself, rival);

        };    

        _view.CongratulateWinner(_winner);

    }

    private Player[] InitializeGame() {
        List<Player> players = new List<Player>();
        Player[] playerOrder = new Player[]{};

        for(int i = 0; i < 2; i++) {
            string filePlayer = _view.AskUserToSelectDeck(_deckFolder);
            Deck deckPlayer = _deckConstructor.CreateDeck(filePlayer, _superstars, _cards);
            Player player = new Player(deckPlayer, _view); 

            if (!player.isMyDeckValid()) {
                _view.SayThatDeckIsInvalid();
                return playerOrder;
            }

            players.Add(player);
            deckPlayer.AssignPlayer(player);
            player.InitializeHand();
        }

        playerOrder = _playerSorter.DefinePlayerOrder(players[0], players[1]);

        return playerOrder;
    }

    private void PlayTurn(Player myself, Player rival) {
        if (_isTurnFirstLap) {
            FirstLapActions(myself, rival);
        }

        PlayerInfo Player1 = myself.GetPlayerInfo();
        PlayerInfo Player2 = rival.GetPlayerInfo();
        _view.ShowGameInfo(Player1, Player2);

        NextPlay firstChoice;
        bool showAbilityOption = myself.CheckIfAbilityHasToBeSeenAsAnOption();

        if (showAbilityOption && !_abilityWasUsed) {
            firstChoice = _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        } else {
            firstChoice = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        }

        HandleOptions(myself, rival, firstChoice);
    }

    private void HandleOptions(Player myself, Player rival, NextPlay firstChoice) {
        if (firstChoice == NextPlay.UseAbility)
        {
            myself.UseAbility(rival);
            _abilityWasUsed = true;
            _isTurnFirstLap = false;
        }
        else if (firstChoice == NextPlay.ShowCards) 
        {
            myself.SeeCards(rival);
            _isTurnFirstLap = false;
        } 
        else if (firstChoice == NextPlay.PlayCard) 
        {
            Result result = myself.PlayACard(rival, _bonus); 
            HandleCardPlayingResult(result, myself, rival);
        } 
        else if (firstChoice == NextPlay.EndTurn) 
        {
            EndTurnActions(myself, rival);
            _bonus.RestoreValues();
        }
            else if (firstChoice == NextPlay.GiveUp)
        {
            _winner = rival.GetSuperstarName();
            _gameOver = true;
        }
    }

    private void HandleCardPlayingResult(Result result, Player myself, Player rival) {
        if (result == Result.GameOver) {
            SetWinner(myself, rival);
            _gameOver = true;
        } else if (result == Result.TurnEnding) {
            EndTurnActions(myself, rival);
            _bonus.RestoreValues();
        } else if (result == Result.MyselfJockeyingForPosition) {
            HandleJockeyingForPosition(myself);
            _isTurnFirstLap = false;
        } else if (result == Result.RivalJockeyingForPosition) {
            HandleJockeyingForPosition(rival);
            EndTurnActions(myself, rival);
        } else if (result == Result.NotPlayed) {
            _isTurnFirstLap = false;
        } else {
            _isTurnFirstLap = false;
            _bonus.RestoreValues();
        }      
    }

    private void HandleJockeyingForPosition(Player player) {
        _bonus.RestoreValues();
        SelectedEffect choice = _view.AskUserToSelectAnEffectForJockeyForPosition(player.GetSuperstarName());
        if (choice == SelectedEffect.NextGrappleIsPlus4D) {
            _bonus.SetManeuverGrappleDamage(4);
        } else if (choice == SelectedEffect.NextGrapplesReversalIsPlus8F) {
            _bonus.SetManeuverGrappleFortitude(8);

        }
    }

    private void FirstLapActions(Player myself, Player rival) {
        _view.SayThatATurnBegins(myself.GetDeck().GetSuperstarName());
        AbilityBeforeDrawing(myself, rival);
        myself.DrawTurnBeginning();
    }

    private void AbilityBeforeDrawing(Player myself, Player rival) {
        if (myself.GetSuperstarLogo() == "Kane" || myself.GetSuperstarLogo() == "TheRock") {
            myself.UseAbility(rival);
        }
    }

    private void EndTurnActions(Player myself, Player rival) {
        _currentTurn = (_currentTurn + 1) % 2;
        _isTurnFirstLap = true;
        _abilityWasUsed = false;
        bool isGameOver = CheckIfTheGameIsOverAfterTurnEnding(myself, rival);

        if (isGameOver) {
            _gameOver = true;
        }
    }

    private bool CheckIfTheGameIsOverAfterTurnEnding(Player myself, Player rival) {
        Deck myDeck = myself.GetDeck();
        Deck rivalsDeck = rival.GetDeck();

        if (rivalsDeck.GetArsenalCount() == 0) {
            _winner = myself.GetSuperstarName();
            return true;
        } else if (myDeck.GetArsenalCount() == 0) {
            _winner = rival.GetSuperstarName();
            return true;
        }   

        return false; 
   } 

   private void SetWinner(Player myself, Player rival) {
        Deck myDeck = myself.GetDeck();
        Deck rivalsDeck = rival.GetDeck();

        if (rivalsDeck.GetArsenalCount() == 0) {
            _winner = myself.GetSuperstarName();
        } else if (myDeck.GetArsenalCount() == 0) {
            _winner = rival.GetSuperstarName();
        }   
   }
}