using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomb_Finder.Managers
{
    public class GameManager
    {
        public bool isGameRunning = false;
        public Tilemap gameMap;
        


        public GameManager() { }


        public void StartGame() 
        { 
            Globals.GameStopwatch.Start();

            Globals.GameState = GameState.Game;

            //gameMap = new Tilemap(new Vector2(40, 40), 9, 9, 40);    // Beginner
            //gameMap = new Tilemap(new Vector2(40, 40), 16, 16, 10);  // Intermediate
            gameMap = new Tilemap(new Vector2(40, 40), 30, 16, 99);  // Expert
        }

        public void EndGame()
        {
            Globals.GameStopwatch.Stop();

            isGameRunning = false;
        }


        public void Update()
        {
            if (Globals.GameState == GameState.Game)
            {
                gameMap.Update();
            }
        }

        public void Draw()
        {
            if(Globals.GameState == GameState.Game)
            {
                gameMap.Draw();
            }
        }
    }
}
