<div align="center">
  <img src="logo-my-default.png" alt="MilAventures Logo" width="120"/>

# MilAventures

### Recuperació — Projecte Final de Disseny d'Interfícies · 2n DAM
  
  ![C#](https://img.shields.io/badge/C%23-.NET%204.7.2-purple)
  ![WPF](https://img.shields.io/badge/UI-WPF-blue)
  ![EF6](https://img.shields.io/badge/ORM-Entity%20Framework%206-green)
  ![SQL Server](https://img.shields.io/badge/DB-SQL%20Server-red)
</div>

---

## Descripció

MilAventures és una aplicació d'escriptori desenvolupada en C# amb WPF per a la gestió integral d'una agència de turisme d'aventura. Permet gestionar activitats (barranquisme, ràfting, senderisme...), guies titulats, equipament tècnic i reserves de clients des d'una única interfície professional.

---

## Tecnologies

| Tecnologia | Versió | Ús |
|---|---|---|
| C# | .NET Framework 4.7.2 | Llenguatge principal |
| WPF + XAML | .NET Framework 4.7.2 | Interfície d'usuari |
| Entity Framework | 6.0 Code First | Accés a dades |
| SQL Server Express | LocalDB | Base de dades |
| Crystal Reports | 13.0 | Informes |
| MSTest | v1 | Testing |
| MahApps.Metro.IconPacks | 6.0 | Icones |

---

<br>

## Estructura de la Solució

```
MilAventures.sln
├── MilAventures.Model       → Entitats EF, DbContext, Repositoris
├── MilAventures.ViewModel   → ViewModels, Commands, NavigationService
├── MilAventures.View        → Views XAML, Converters, Styles
├── MilAventures.Reports     → Informes Crystal Reports
└── MilAventures.Tests       → Tests MSTest (7 tests)
```

---

<br>

## Requisits Previs

- Windows 10 o superior
- Visual Studio 2022
- SQL Server Express (inclosa amb VS 2022)
- Crystal Reports Runtime per a VS 2022
- .NET Framework 4.7.2

---

<br>

## Inicialització

**1. Clona el repositori:**

```bash
git clone https://github.com/[usuari]/MilAventures.git
```

**2. Crea la base de dades:**

Obre SSMS i executa els scripts en ordre:

```
database/Schema.sql             ← Estructura de taules
database/Schema_With_Data.sql   ← Dades de prova
```

**3. Configura la connexió:**

Edita `App.config` del projecte `MilAventures.View` i ajusta:

```xml
<add name="MilAventuresConnection"
     connectionString="Data Source=localhost\SQLExpress;
     Initial Catalog=MilAventuresDB;
     User ID=el_teu_usuari;
     Password=la_teva_contrasenya;
     Encrypt=False"
     providerName="System.Data.SqlClient" />
```

**4. Compila i executa:**

Estableix `MilAventures.View` com a projecte d'inici i prem `F5`.

---

<br>

## Funcionalitats

- ✅ Gestió de Guies (CRUD + foto)
- ✅ Gestió de Clients (CRUD + foto)
- ✅ Gestió d'Activitats (CRUD + dificultat + preu)
- ✅ Gestió d'Equipament (CRUD + control de stock)
- ✅ Gestió de Reserves (CRUD + línies + estats)
- ✅ 4 Informes Crystal Reports (PDF exportable)
- ✅ 7 Tests MSTest (3 unitaris + 3 validació + 1 integració)
- ✅ Patró MVVM amb 3 projectes separats

---

<br>

## Disseny — Figma

[🎨 Veure prototip a Figma](https://www.figma.com/design/moVlOJSoRxPfSjLCoGuej3/ProjFin-DI?node-id=1-3&t=gpPWbEunjOLKMTN2-1)

---

<br>

## Documentació

- [📖 Manual Tècnic](docs/Manual_Tecnic.md)
- [📘 Manual d'Usuari](docs/Manual_Usuari.pdf)

<br>

---

<div align="center">
  <sub> DAM · Disseny d'Interfícies · Curs 2025-2026</sub>
</div>
