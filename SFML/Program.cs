App.SubscribeGlobalListeners();

var pipe = new Pipeline();
pipe.ConfigureFolders();
pipe.ConfigureOptions();

var app = new App();
app.ConfigureResources();
app.ConfigureListeners();
app.Start();
