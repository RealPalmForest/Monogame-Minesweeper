using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomb_Finder.Managers
{
    public class UIManager
    {


        public UIManager() { }


        public void Update()
        {

        }

        public void Draw()
        {
            if (Globals.GameState == GameState.Game)
            {
                //TODO - Draw the game UI
            }
            else if (Globals.GameState == GameState.GameOver)
            {
                //TODO - Draw the game over screen
            }
            else if (Globals.GameState == GameState.Options)
            {
                //TODO - Draw the game start options screen
            }

        }
    }
}
