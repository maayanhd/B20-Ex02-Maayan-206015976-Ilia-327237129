using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace B20_Ex02
{
    class Game
    {
        private Player m_Player1 = null;
        private Player m_Player2 = null;
        private Cell[,] m_Board = null;
        private List<Cell> m_AvailableCards = null;
        private Dictionary<int, List<Location>> m_SeenCards = null;
        
        // Constant values 
        const int k_HeightIndex = 0;
        const int k_WidthIndex = 1;

        public Game(int[] i_Measurements, List<int> i_Cards, string[] i_Names,Player.ePlayerType[] i_PlayerTypes)
        {
            m_Board = new Cell[i_Measurements[k_HeightIndex], i_Measurements[k_WidthIndex]];
            AvailableCards = new List<Cell>();
            m_Player1 = new Player(i_Names[0],i_PlayerTypes[0]);
            m_Player2 = new Player(i_Names[1],i_PlayerTypes[1]);
            m_SeenCards = new Dictionary<int, List<Location>>();

            initializeBoardAndAvailableCards(i_Cards);

        }
        private void initializeBoardAndAvailableCards(List<int> i_Cards)
        {
            int firstDimLength = m_Board.GetLength(0);
            int secondDimLength = m_Board.GetLength(1);
            Random randObj = new Random();

            for (int row = 0; row < firstDimLength; row++)
            {
                for (int col = 0; col < secondDimLength; col++)
                {
                    // Randomizing a card to put in the current location on the board 
                    int indexToAdd = randObj.Next(i_Cards.Count);

                    m_Board[row, col]= new Cell(i_Cards[indexToAdd],new Location(row,col));
                    // Updating boolean state indicates the card is flipped 
                    m_Board[row, col].IsFlipped = false;
                    i_Cards.RemoveAt(indexToAdd);
                    // Initializing available cards storage
                    this.AvailableCards.Add(m_Board[row, col]);
                }
            }
        }

        public void UpdateAvailableCards(bool i_IsAMatch, params Cell[] io_PairOfCards)
        {
            // in case there's a match- we'll erase the matching pair from the storage
            if (i_IsAMatch == true)
            {
                // Can be better implemented
                AvailableCards.Remove(io_PairOfCards[0]);
                AvailableCards.Remove(io_PairOfCards[1]);
            }

        }

        public void UpdateSeenCards(bool i_IsAMatch, params Cell[] io_PairOfCards)
        {
            if (i_IsAMatch == true)
            {
                // In case there's a match we need to remove the elements tha't have been seen
                SeenCards.Remove(io_PairOfCards[0].CellContent);
            }
            else
            {
                AddIfNotInSeenCards(io_PairOfCards[0]);
                AddIfNotInSeenCards(io_PairOfCards[1]);
            }
        }

        public void AddIfNotInSeenCards(Cell io_CardToAdd)
        {
            
            if (SeenCards.TryGetValue(io_CardToAdd.CellContent, out List<Location> keyList))
            {
                if (keyList.Contains(io_CardToAdd.Location) == false)
                {
                    keyList.Add(io_CardToAdd.Location);
                }
            }
            else
            {
                SeenCards.Add(io_CardToAdd.CellContent,new List<Location>());
                SeenCards[io_CardToAdd.CellContent].Add(io_CardToAdd.Location);
            }

        }

        public bool IsLocationInRange(Location i_LocationToCheck)
        {
            bool rowInRange = (i_LocationToCheck.Row < m_Board.GetLength(0) && i_LocationToCheck.Row >= 0);
            bool colInRange = (i_LocationToCheck.Col < m_Board.GetLength(1) && i_LocationToCheck.Col >= 0);
            return rowInRange && colInRange;
        }

        public void HandleAvailableRemoval(Location? i_CardLocation)
        {
            // Design need to be fixed
            int i = i_CardLocation.Value.Row, j = i_CardLocation.Value.Col;
            bool isDoneRemoving = false;

            for (int index = 0; i < AvailableCards.Count && isDoneRemoving == false; index++)
            {
                isDoneRemoving = AvailableCards.Equals(Board[i, j]);

                if (isDoneRemoving == true)
                {
                    AvailableCards.RemoveAt(index);
                }
            }
        }

        public bool IsThereAMatch(Player io_CurrentPlayer, params Cell[] io_Cards)
        {
            bool isAMatch = false;
            // Checking whether the player has found a pair of cards 
            // Need to implement == operator
            if (io_Cards[0].Equals(io_Cards[1]))
            {
                isAMatch = true;
                // Updating score 
                io_CurrentPlayer.Score++;
            }
            // Other wise, no need to
            // the score
            return isAMatch;
        }
        public bool IsTheGameEnded()
        {
            return (AvailableCards.Count == 0);
        }
        public Player Player1
        {
            get
            {
                return m_Player1;
            }
            set
            {
                m_Player1 = value;
            }
        }
        public Player Player2
        {
            get
            {
                return m_Player2;
            }
            set
            {
                m_Player2 = value;
            }
        }
        public Cell[,] Board
        {
            get
            {
                return m_Board;
            }
            set
            {
                m_Board = value;
            }
        }

        public Cell this[int i, int j]
        {
            get
            {
                return m_Board[i, j];
            }
            set
            {
                m_Board[i, j] = value;
            }
        }
        public List<Cell> AvailableCards
        {
            get
            {
                return m_AvailableCards;
            }
            set
            {
                m_AvailableCards = value;
            }
        }

        public Dictionary<int, List<Location>> SeenCards
        {
            get
            {
                return m_SeenCards;
            }
            set
            {
                m_SeenCards = value;
            }
        }

        public void FlipCard(Cell io_CardToFlip)
        {

            int i = io_CardToFlip.Location.Row,
                j = io_CardToFlip.Location.Col;

            // Flipping Card by logic
            m_Board[i, j].IsFlipped = !m_Board[i, j].IsFlipped;
        }
    }
}


