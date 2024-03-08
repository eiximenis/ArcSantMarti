using SubC.AllegroDotNet;
// ...
Al.Init();
var display = Al.CreateDisplay(1280, 720) ?? throw new Exception();
Al.Rest(3);
Al.DestroyDisplay(display);
Al.UninstallSystem();