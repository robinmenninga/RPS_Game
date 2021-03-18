using System;
using System.Collections.Generic;
using System.Threading;
using RestSharp;

namespace RPS_Game
{
    class Game
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Starting game...");
            List<string> names = AskNames();
            string playerName = names[0];
            string computerName = names[1];
            string playerWeapon = AskWeapon();
            string computerWeapon = RandomWeapon();
            Console.Write(computerName + " chooses");
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.Write(".");
            Thread.Sleep(500);
            Console.WriteLine(computerWeapon + "!");
            Thread.Sleep(1000);
            string winner = CalcWinner(playerWeapon, computerWeapon);
            AnnounceWinner(playerName, computerName, winner);
        }

        private static string AskWeapon()
        {
            Console.WriteLine("Rock, Paper or Scissors?");
            while (true)
            {
                String input = Console.ReadLine().ToLower();
                if (input == "rock" || input == "r" || input == "1")
                    return "Rock";
                if (input == "paper" || input == "p" || input == "2")
                    return "Paper";
                if (input == "scissors" || input == "s" || input == "3")
                    return "Scissors";
                Console.WriteLine("Answer not recognized, please try again.");
            }
        }

        private static string RandomWeapon()
        {
            int randomint = new Random().Next(3);
            switch (randomint)
            {
                case 0: return "Rock";
                case 1: return "Paper";
                case 2: return "Scissors";

                //Shouldn't be possible, but visual studio wouldn't let me compile without it :-)
                default: return "RPG";
            }
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
            //hacky way of getting the name from json, but hey, it works.
            RestClient client = new RestClient("https://randomuser.me/api/");
            string jsonData = client.Get(new RestRequest("", DataFormat.Json)).Content;
            if (jsonData == "")
            {
                Console.WriteLine("Couldn't get a random name.");
                return "CPU";
            }
                
            string name = jsonData.Substring(jsonData.IndexOf("first") + 8);
            return name.Substring(0, name.IndexOf("\""));
        }

        private static void AnnounceWinner(String playerName, String computerName, String winner)
        {
            if (winner == "Player")
                Console.WriteLine("The winner is " + playerName + "!");
            if (winner == "Computer")
                Console.WriteLine("The winner is " + computerName + "!");
            if (winner == "Stalemate")
                Console.WriteLine("Stalemate!");
        }

    }
}
