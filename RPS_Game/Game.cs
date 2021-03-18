using System;
using System.Collections.Generic;
using System.Threading;
using RestSharp;

namespace RPS_Game
{
    class Game
    {
        static readonly List<string> rockWords = new List<string>() { "rock", "r", "1", "stone" };
        static readonly List<string> paperWords = new List<string>() { "paper", "p", "2", "sheet" };
        static readonly List<string> scissorsWords = new List<string>() { "scissors", "s", "3", "cut" };
        static readonly List<string> yesWords = new List<string>() { "yes", "y", "true", "ye" };
        static readonly List<string> noWords = new List<string>() { "no", "n", "nah", "non" };

        static string playerName;
        static string computerName;

        static int playerPoints = 0;
        static int computerPoints = 0;
        static void Main()
        {
            bool stop = false;
            List<string> names = AskNames();
            playerName = names[0];
            computerName = names[1];
            
            while (!stop)
            {
                PlayGame();
                Console.WriteLine("Play again?");
                while (true)
                {
                    string answer = Console.ReadLine();
                    if (yesWords.Contains(answer))
                    {
                        Console.Clear();
                        break;
                    }
                    if (noWords.Contains(answer))
                    {
                        Console.WriteLine("Quitting...");
                        stop = true;
                        break;
                    }
                    Console.WriteLine("Answer not recognized, please try again.");
                }
            }
        }

        private static void PlayGame()
        {
            string playerWeapon = AskWeapon();
            string computerWeapon = RandomWeapon();
            Console.Write(computerName + " chooses");
            WriteDots(3);
            Console.WriteLine(" " + computerWeapon + "!");
            Thread.Sleep(1000);
            string winner = CalcWinner(playerWeapon, computerWeapon);
            AnnounceWinner(winner);
        }

        private static string AskWeapon()
        {
            Console.WriteLine("Rock, Paper or Scissors?");
            while (true)
            {
                String input = Console.ReadLine().ToLower();
                if (rockWords.Contains(input))
                    return "Rock";
                if (paperWords.Contains(input))
                    return "Paper";
                if (scissorsWords.Contains(input))
                    return "Scissors";
                Console.WriteLine("Answer not recognized, please try again.");
            }
        }

        private static string RandomWeapon()
        {
            int randomint = new Random().Next(3);
            return randomint switch
            {
                0 => "Rock",
                1 => "Paper",
                2 => "Scissors",

                //Shouldn't be possible, but visual studio is complaining that it could happen :|
                _ => "RPG",
            };
        }

        private static string CalcWinner(string playerWeapon, string computerWeapon)
        { 
            if (playerWeapon == computerWeapon)
                return "Stalemate";

            if (playerWeapon == "Rock")
            {
                if (computerWeapon == "Paper")
                    return "Computer";
                return "Player";
            }

            if (playerWeapon == "Paper")
            {
                if (computerWeapon == "Rock")
                    return "Player";
                return "Computer";
            }

            if (playerWeapon == "Scissors")
            {
                if (computerWeapon == "Rock")
                    return "Computer";
                return "Player";
            }
            return "Something went wrong";

        }

        private static List<string> AskNames()
        {
            List<string> names = new List<string>();
            Console.WriteLine("What's your name?");
            string playername = Console.ReadLine();
            Console.WriteLine("What would you like to call your opponent? (type random for a random name)");
            string computername = Console.ReadLine();
            if (computername.ToLower() == "random")
            {
                computername = RandomName();
                Console.WriteLine("The computers name is " + computername);
            }

            names.Add(playername);
            names.Add(computername);
            return names;
        }

        private static string RandomName()
        {
            RestClient client = new RestClient("https://randomuser.me/api/");
            string jsonData = client.Get(new RestRequest("", DataFormat.Json)).Content;
            if (jsonData == "")
            {
                Console.WriteLine("Couldn't get a random name.");
                return "CPU";
            }

            //hacky way of getting the name from json, but hey, it works.    
            //string name = jsonData.Substring(jsonData.IndexOf("first") + 8);
            string name = jsonData[(jsonData.IndexOf("first") + 8)..];
            return name.Substring(0, name.IndexOf("\""));
        }

        private static void AnnounceWinner(String winner)
        {
            if (winner == "Player")
            {
                Console.WriteLine("The winner is " + playerName + "!");
                playerPoints++;
            }
            if (winner == "Computer")
            {
                Console.WriteLine("The winner is " + computerName + "!");
                computerPoints++;
            }
            if (winner == "Stalemate")
                Console.WriteLine("Stalemate!");

            PrintScoreboard();
        }

        private static void PrintScoreboard()
        {
            Console.WriteLine();
            Console.WriteLine("Scoreboard");
            Console.WriteLine(playerName + ": " + playerPoints);
            Console.WriteLine(computerName + ": " + computerPoints);
        }
        public static void WriteDots(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
        }
    }
}
