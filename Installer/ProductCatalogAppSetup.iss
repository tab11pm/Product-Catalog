#define MyAppName "Список товаров"
#define MyAppVersion "1.0"
#define MyAppPublisher "Tabek"
#define MyAppExeName "ProductCatalogApp.exe"

[Setup]
AppId={{8D6D5E5E-2D7A-4A7F-9D80-1E9C12345678}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=Output
OutputBaseFilename=ProductCatalogAppSetup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
SetupIconFile=app.ico
[Languages]
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "Создать ярлык на рабочем столе"; GroupDescription: "Дополнительные задачи:"; Flags: unchecked

[Files]
Source: "..\bin\Release\net10.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Product Catalog App"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Удалить Product Catalog App"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Product Catalog App"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Запустить Product Catalog App"; Flags: nowait postinstall skipifsilent