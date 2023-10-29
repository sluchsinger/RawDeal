using Players;

namespace PlayerOrder 
{
    public class PlayerSorter 
    {
        public Player[] DefinePlayerOrder(Player player1, Player player2) {
            Player[] playerOrder = new Player[2];

            if (player1.GetDeck().GetSuperstarValue() >= player2.GetDeck().GetSuperstarValue()) {
                playerOrder[0] = player1;
                playerOrder[1] = player2;
            } else {
                playerOrder[0] = player2;
                playerOrder[1] = player1;
            }

            return playerOrder;
        }        
    }
}