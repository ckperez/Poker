using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Card : IComparable
    {
        public char suit; //C H D S
        public int rank; // 2=2...K=13, A=14;

        public Card()
        {

        }

        public Card(string str)
        {
            str = str.ToUpper();
            foreach (char c in str)
            {
                switch (c)
                {
                    case 'C':
                    case 'H':
                    case 'S':
                    case 'D':
                        suit = c;
                        break;
                    case 'T':
                        rank = 10;
                        break;
                    case 'J':
                        rank = 11;
                        break;
                    case 'Q':
                        rank = 12;
                        break;
                    case 'K':
                        rank = 13;
                        break;
                    case 'A':
                        rank = 14;
                        break;
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        rank = c - '0';
                        break;
                }

            }

            if (rank == 0)
                Console.WriteLine("You forgot the rank for ", str);
            if (suit == 0)
                Console.WriteLine("You forgot the suit for", str);
        }

        bool IsValid()
        {
            return suit != '\0' && rank >= 2 && rank <= 14;
        }

        public int CompareTo(object obj)
        {
            Card c = obj as Card;
            return rank - c.rank;
        }
    }


    class Program
    {
        static int dealIndex = 0;
        public static Random rand = new Random();
        public static Card[] deck = null;

        static void Main(string[] args)
        {
            Card[] hand = GetHand(args);
      
            Array.Sort(hand);
            foreach(Card c in hand) { Console.WriteLine(c.rank.ToString() + c.suit); };
            if(IsStraightFlush(hand))
            {
                Console.WriteLine("STRAIGHT FLUSH!");//todo
            }

            else if (IsFourOfAKind(hand))
            {
                Console.WriteLine("FOUR OF A KIND!");
            }

            else if (IsFullHouse(hand))
            {
                Console.WriteLine("FULL HOUSE!");
            }

            else if (IsFlush(hand))
            {
                Console.WriteLine("FLUSH!");
            }

            else if (IsStraight(hand))
            {
                Console.WriteLine("STRAIGHT!");
            }

            else if (IsThreeOfAKind(hand))
            {
                Console.WriteLine("THREE OF A KIND!");
            }

            else if (IsTwoPair(hand))
            {
                Console.WriteLine("TWO PAIR!");
            }

            else if (IsPair(hand))
            {
                Console.WriteLine("PAIR!");
            }

            else
            {
                Console.WriteLine("HIGH CARD!");
            }
            Console.Read();
         }

        static Card[] GetHand(string[] args)
        {
            //take in string of cards and break up cards at spaces

            Card[] hand = new Card[5];
            int index = 0;

            foreach(string a in args)
            {
                if (index >= 5)
                    break;
                Card c = new Card(a);
                hand[index++] = c;
            }

            while(index < 5)
            {
                hand[index++] = Deal();
            }
            return hand;
        } // done

        static Card Deal()
        { 
            if (deck == null || dealIndex >= 52)
            {
                deck = new Card[52]; // declare array of 52 "blank cards"

                for (int i = 0; i < 52; i++) //build out deck
                {
                    if (i <= 12)  // 2-14 of Spades
                    {
                        deck[i] = new Card() { suit = 'S', rank = i + 2 };
                    } else if (i >= 13 && i <= 25) // 2-14 of Clubs
                    {
                        deck[i] = new Card() { suit = 'C', rank = i - 11 };
                    } else if (i >= 26 && i <= 38) // 2-14 of Hearts
                    {
                        deck[i] = new Card() { suit = 'H', rank = i - 24 };
                    } else if (i >= 39 && i <= 51) //2-14 of Diamonds
                    {
                        deck[i] = new Card() { suit = 'D', rank = i - 37 };
                    }
                }
            }       // end of if

            for (int j = deck.Length - 1; j > 0; --j) //shuffle
            {
                int k = rand.Next(j + 1);
                Card tmp = deck[j];
                deck[j] = deck[k];
                deck[k] = tmp;
            }

            return deck[dealIndex++];
        } // done

        static bool IsRoyalFlush(Card[] hand) // done
        {
            return (hand[0].rank == 10 && IsStraightFlush(hand));
        }

        static bool IsStraightFlush(Card[] hand) // done
        {
            return IsFlush(hand) && IsStraight(hand);
        }

        static Dictionary<int,int> HelperDict(Card[] hand) //helper
        {
            Dictionary<int, int> helperDict = new Dictionary<int, int>();

            foreach (var c in hand)
            {
                helperDict[c.rank] = (helperDict.ContainsKey(c.rank)) ? helperDict[c.rank]++ : 1;
            }
            return helperDict;
        } // FIX THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        static bool IsFourOfAKind (Card[] hand)
        {
            Dictionary<int, int> dict = HelperDict(hand);
            foreach (KeyValuePair<int, int> item in dict)
            {
                if(item.Value == 4)
                {
                    return true;
                }
            }
            return false;
            
        } // done

        static bool IsFullHouse(Card[] hand)
        {
            Dictionary<int, int> dict = HelperDict(hand);
            bool GotThree = false;
            bool GotTwo = false;
            foreach (KeyValuePair<int, int> item in dict)
            {
                if (item.Value == 3)
                {
                    GotThree = true;
                }
                if(item.Value == 2)
                {
                    GotTwo = true;
                }
            }
            return (GotTwo && GotThree);
        } // done

        static bool IsFlush(Card[] hand) // done
        {
            for (int i = 1; i < hand.Length; i++)
            {
                if (hand[i].suit != hand[0].suit)
                {
                    return false;
                }
                
            }
            return true;
        }

        static bool IsStraight(Card[] hand) // done
        {
            for (int i = 1; i < hand.Length; i++)
            {
                if (hand[i].rank != hand[i - 1].rank + 1)
                {
                    return false;
                }
            }
        return true;
        }
        
        static bool IsThreeOfAKind(Card[] hand)
        {
            Dictionary<int, int> dict = HelperDict(hand);
            bool GotThree = false;
            bool GotTwo = false;
            foreach (KeyValuePair<int, int> item in dict)
            {
                if (item.Value == 3)
                {
                    GotThree = true;
                }
                if (item.Value == 2)
                {
                    GotTwo = true;
                }
            }
            return (!GotTwo && GotThree);
        } // done

        static bool IsTwoPair(Card[] hand)
        {
            Dictionary<int, int> dict = HelperDict(hand);
            int numPairs = 0;
            foreach (KeyValuePair<int, int> item in dict)
            {
                
                if (item.Value == 2)
                {
                    numPairs++;
                }
            }
            return numPairs == 2;
        } // done

        static bool IsPair(Card[] hand)
        {
            Dictionary<int, int> dict = HelperDict(hand);
            int numPairs = 0;
            foreach (KeyValuePair<int, int> item in dict)
            {

                if (item.Value == 2)
                {
                    numPairs++;
                }
            }
            return numPairs == 1;
        } // done

        static string IsHighCard(Card[] hand)
        {
            int highCardRank = 0;
            char highCardSuit = 'o';
            //string Rank;
            //string Suit;
            string highCardMsg;

            foreach (Card c in hand)
            {
                if (c.rank <= highCardRank)
                {
                    highCardRank = c.rank;
                    highCardSuit = c.suit;
                }
            }

            

            highCardMsg = " " + highCardRank + " of " + highCardSuit;
            return highCardMsg;
        } // done

        
    }    
}
