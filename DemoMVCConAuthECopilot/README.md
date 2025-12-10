# DemoMVCConAuthECopilot

## Descrizione
DemoMVCConAuthECopilot è una applicazione ASP.NET Core MVC che implementa autenticazione, autorizzazione e gestione clienti, seguendo i principi di Clean Architecture e le best practice moderne.

## Funzionalità principali
- Autenticazione tramite Identity (con ruoli e conferma account)
- Policy di autorizzazione granulari (es. `AutoAziendale`, `ViewCustomers`)
- Gestione clienti tramite repository e DTO
- Integrazione con database multipli (ApplicationDbContext, NorthwindDbContext)
- Gestione errori e pipeline moderna
- Supporto per Dependency Injection

## Struttura del progetto
- **Controllers/**: Controller MVC per le varie funzionalità
- **Data/**: DbContext, Migrations e repository
- **Models/**: DTO e ViewModel
- **Views/**: Razor views per la UI
- **Areas/Identity/**: Gestione autenticazione e registrazione
- **wwwroot/**: Static files (CSS, JS, librerie)

## Sicurezza
- Autenticazione e ruoli tramite ASP.NET Identity
- Policy di autorizzazione per accesso alle risorse
- Validazione input tramite DataAnnotations
- HTTPS obbligatorio in produzione

## Come eseguire
1. Configura le stringhe di connessione in `appsettings.json`
2. Esegui le migration per i database
3. Avvia il progetto con `dotnet run` o da Visual Studio

## Best Practice seguite
- Clean Architecture (separazione tra Presentation, Application, Domain, Infrastructure)
- Uso di DTO per esposizione dati
- Validazione input lato server
- Async/await per tutte le operazioni I/O
- Dependency Injection per servizi e repository

## Note aggiuntive
- Per estendere la validazione o aggiungere nuove funzionalità, seguire i pattern già presenti nei controller e nei DTO.
- Per la gestione di grandi dataset, implementare paginazione e caching.
- Aggiungere test automatici per garantire la qualità del codice.

## Autore
Andrea Fabbri

---
Per domande o contributi, apri una issue o contatta l'autore.