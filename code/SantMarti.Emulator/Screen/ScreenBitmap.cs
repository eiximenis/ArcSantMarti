using SubC.AllegroDotNet;
using SubC.AllegroDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantMarti.Emulator.Screen;
class ScreenBitmap
{

    public AllegroBitmap BitmapForScreen()
    {
        var scale = 1;
        var flags = Al.GetNewBitmapFlags();
        Al.SetNewBitmapFlags(flags | SubC.AllegroDotNet.Enums.BitmapFlags.MemoryBitmap);
        var bmp = Al.CreateBitmap(256*scale, 192*scale);
        Al.SetNewBitmapFlags(flags);
        return bmp!;
    }

    public void SetPixel(int col, int row)
    {
    }


}
