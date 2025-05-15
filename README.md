# Worktastic-App
Worktastic ist eine Webanwendung, die mit dem ASP.NET-Core-Framework erstellt wurde. Sie ermöglicht Anwendern  sich zu registrieren, sich anzumelden und Stellenangebote zu erstellen, zu verwalten und mit anderen Anwendern zu teilen. 
Diese Anwendung wurde zu Lernzwecken erstellt, weshalb die Clean-Code-Regeln bewusst nicht hundertprozentig eingehalten wurden, um mehr Freiheit zu haben, bestimmte Dinge ausprobieren zu können.

## Funktionen und Besonderheiten
- unterschiedliche Ansichten und Berechtigungen für User, je nach Rolle und ob angemeldet oder nicht
- Tabelle mit Such- und Sortieralgorithmus und Detailansischt für einzelne Jobanzeigen
- Unterscheidung zwischen Admin und User
- es wird Registrierung- und Login-Funktionalität und Logik von ASP.NET-Framework genutzt 
- Benutzer hat die Möglichkeit:
  - Jobangebot erstellen
  - Jobangebot bearbeiten
  - Jobangebot löschen
  - Jobangebote aller Benutzer, ohne sich enzuloggen zu sehen
  - Admin kann alle Jobangebote sehen und bearbeiten
- Responsive Design
- außerdem benutzte Frameworks: Bootstrap, DataTable, SweetAlert
- besonderes Augenmerk auf: einheitliches Design und Benutzerfreundlichkeit
- diese Anwendung ist nur mit Microsoft SQL Server kompatibel

## Technische Voraussetzungen
- **.NET 8.0:** Die Anwendung wurde mit .NET 8.0 entwickelt
- **Visual Studio:** Visual Studio 2022 oder höher wird empfohlen
- **Microsoft SQL Server:** Die Anwendung verwendet standardmäßig MSSQL Server als Datenbank
- **Internet-Verbindung:** Für Bootstrap, jQuery und andere CDN-gehostete Ressourcen

## Installation und Konfiguration
### 1. Projekt klonen oder herunterladen
```
git clone https://github.com/juliatarasow/Worktastic_ASP.NET-Project.git
cd Worktastic_ASP.NET-Project
```
### 2. Datenbankverbindung konfigurieren

Öffnen Sie die appsettings.json-Datei und passen Sie den Connection String an:

```
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SERVER_NAME;Initial Catalog=ASPnet_Jobtastic;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;Application Intent=ReadWrite;MultipleActiveResultSets=True"
}
```
Ersetzen Sie:

YOUR_SERVER_NAME mit dem Namen Ihres SQL Servers
YOUR_USERNAME mit Ihrem SQL Server-Benutzernamen
YOUR_PASSWORD mit Ihrem SQL Server-Passwort

### 3. Datenbank migrieren
In Visual Studio: Öffnen Sie die Package Manager Console und führen Sie folgende Befehle aus:
```
Update-Database
```
Alternativ über die Befehlszeile:
```
dotnet ef database update
```
### 4. Anwendung starten
In Visual Studio:
- Klicken Sie auf **"Debuggen starten"** oder drücken Sie **F5**
  
Über die Befehlszeile:
```
dotnet run
```
Nach dem Start sollte die Anwendung auf dem **localhost** verfürbar sein.

## Benutzung der Anwendung
### Erste Schritte
  1. **Registrieren:** Erstellen Sie ein neues Benutzerkonto

  2. **Anmelden:** Melden Sie sich mit Ihrem Konto an

  3. **Stellenanzeige erstellen:** Klicken Sie auf "Neuen Job anlegen"

  4. **Stellenanzeigen verwalten:** Verwalten Sie Ihre Anzeigen über die "Deine Inserate"-Seite

### Administratorrolle einrichten
Um einen Benutzer zum Administrator zu machen:
1. Führen Sie eine SQL-Abfrage aus, um die Benutzer-ID zu ermitteln:
  ```
  SELECT Id FROM AspNetUsers WHERE UserName = 'gewünschter_benutzername';
  ```
2. Führen Sie eine SQL-Abfrage aus, um die Rollen-ID zu ermitteln:
  ```
  SELECT Id FROM AspNetRoles WHERE Name = 'Administrator';
  ```
3. Führen Sie eine SQL-Abfrage aus, um den Benutzer zur Administratorrolle hinzuzufügen:
  ```
  INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('benutzer_id', 'administrator_rollen_id');
  ```
## Rollen
**User:** Hat volle Kontrolle über seine Stellenanzeigen

**Administrator:** Hat Zugriff auf alle Stellenanzeigen im System

## Passwortrichtlinien
Die Passwortrichtlinien können in Program.cs angepasst werden:
```
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 0;
});
```
## Projektstruktur
```/Areas/Identity: ```Identity Framework-Komponenten für Authentifizierung

```/Authorization: ```Autorisierungslogik für die Ressourcenverwaltung

```/Controllers: ```MVC-Controller der Anwendung

```/Data: ```Datenbankkontext und Migrations-Dateien

```/Models: ```Datenmodelle der Anwendung

```/Views: ```MVC-Views für die Benutzeroberfläche

```/wwwroot: ```Statische Ressourcen (CSS, JavaScript, Bilder)

## Fehlerbehandlung
### Datenbankverbindungsfehler
- Stellen Sie sicher, dass Ihr SQL Server läuft
- Überprüfen Sie den Connection String in appsettings.json
- Stellen Sie sicher, dass der Benutzer die richtigen Berechtigungen hat
- Migrationsfehler
  
Bei Problemen mit der Datenbankmigration:
```
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```
