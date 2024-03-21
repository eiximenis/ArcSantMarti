

using SFML.Window;

var mode = new SFML.Window.VideoMode(800, 600);
var window = new SFML.Graphics.RenderWindow(mode, "SFML works!");
window.KeyPressed += Window_KeyPressed;

while (window.IsOpen)
{
    // Process events
    window.DispatchEvents();

    // Finally, display the rendered frame on screen
    window.Display();
}

void Window_KeyPressed(object? sender, KeyEventArgs e)
{
    var window = (SFML.Window.Window)sender;
    if (e.Code == SFML.Window.Keyboard.Key.Escape)
    {
        window.Close();
    }
}