using System.Collections.Generic;

namespace Droog.MonkeySpace2013.Mud {
    public class TheHouse {

        private int num = 0;
        public Dictionary<string, Player> Players = new Dictionary<string, Player>();
        public Room Porch;
        public Room Livingroom;
        public Room Coatcloset;
        public Room Kitchen;
        public Room TheVoid;
        public Room Den;
        public Room Basement;
        public Room Hallway;

        public TheHouse() {
            Porch = new Room() { Name = "Porch", Description = "A large porch." };
            Livingroom = new Room() { Name = "Living Room", Description = "A comfortable Living Room." };
            Coatcloset = new Room { Name = "Coat Closet", Description = "It's dark in here." };
            Kitchen = new Room() { Name = "Kitchen", Description = "A large kitchen." };
            TheVoid = new TheVoid() { Name = "Void", Description = "There's nothing here. There's slight metallic taste in your mouth.", Exit = Porch };
            Den = new Room() { Name = "Den", Description = "A smallish, walnut panelled room." };
            var stairsup = new Connector() { Name = "Staircase" };
            var stairsdown = new Connector() { Name = "Staircase" };
            Basement = new Room { Name = "Basement", Description = "A dank and dark place. It's creepy in here." };
            Hallway = new Room() { Name = "Upstairs Hallway", Description = "A hallway. All the doors appear to be locked" };
            Porch[Direction.East] = Livingroom;
            Livingroom[Direction.West] = Porch;
            Porch[Direction.West] = TheVoid;
            Porch[Direction.North] = TheVoid;
            Porch[Direction.South] = TheVoid;
            Livingroom[Direction.Up] = stairsup;
            stairsup.From = Livingroom;
            stairsup.To = Hallway;
            Livingroom[Direction.Down] = stairsdown;
            stairsdown.From = Livingroom;
            stairsdown.To = Basement;
            Livingroom[Direction.North] = Coatcloset;
            Coatcloset[Direction.South] = Livingroom;
            Livingroom[Direction.East] = Kitchen;
            Kitchen[Direction.West] = Livingroom;
            Livingroom[Direction.South] = Den;
            Den[Direction.North] = Livingroom;
            Den[Direction.East] = Kitchen;
            Kitchen[Direction.South] = Den;
        }

        public Player AddPlayer() {
            num++;
            var player = new Player() { Name = "Player" + num, TheHouse = this };
            Coatcloset.Enter(player);
            Players[player.Name] = player;
            return player;
        }
    }
}