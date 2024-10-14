using SFMLGame;
using SFMLGame.pipeline;

var pipe = new Pipeline();
pipe.ConfigureFolders();

var app = new App();
app.ConfigureResources();
app.ConfigureListeners();
app.Start();
