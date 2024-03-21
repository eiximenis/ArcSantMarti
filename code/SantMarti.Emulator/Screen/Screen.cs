using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Emulator.Screen;
 class Screen
{
    private readonly Texture _screenData;

    public Screen()
    {
        _screenData = new Texture(256, 192);


    }
}
