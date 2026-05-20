# Manual Tècnic — MilAventures

## 1. Arquitectura del Sistema

MilAventures segueix el patró de disseny MVVM (Model-View-ViewModel)
implementat en 4 projectes separats dins d'una solució Visual Studio:

- **MilAventures.Model**: Capa de dades. Conté les entitats EF6,
  el DbContext i els repositoris.
- **MilAventures.ViewModel**: Capa de lògica de presentació.
  Conté els ViewModels, Commands i el NavigationService.
- **MilAventures.View**: Capa de presentació WPF.
  Conté les Views XAML, Converters i Styles.
- **MilAventures.Reports**: Capa d'informes.
  Conté els DataSets i informes Crystal Reports.
- **MilAventures.Tests**: Capa de testing.
  Conté els tests unitaris i d'integració MSTest.

[captura de l'Explorador de Solucions amb els 5 projectes]

## 2. Tecnologies Utilitzades

| Tecnologia | Versió | Ús |
|---|---|---|
| C# | .NET Framework 4.7.2 | Llenguatge principal |
| WPF | .NET Framework 4.7.2 | Interfície d'usuari |
| Entity Framework | 6.0 | Accés a dades (Code First) |
| SQL Server Express | LocalDB | Base de dades |
| Crystal Reports | 13.0 | Generació d'informes |
| MSTest | v1 | Testing unitari i d'integració |
| MahApps.Metro.IconPacks | 6.0 | Icones Material Design |

## 3. Base de Dades

La base de dades MilAventuresDB conté 7 taules relacionades:

- **Category**: Categories d'activitats i equipament
- **Guide**: Guies titulats de l'agència
- **Client**: Clients de l'agència
- **Equipment**: Material tècnic disponible
- **EquipmentStatus**: Estats del material (disponible, manteniment...)
- **Activity**: Activitats d'aventura disponibles
- **BookingStatus**: Estats de les reserves
- **Booking**: Reserves realitzades pels clients
- **BookingLine**: Línies detallades de cada reserva

[captura del diagrama de la BD al SSMS]

## 4. Configuració de la Connexió

La cadena de connexió es configura a l'App.config del projecte View:

```xml
<add name="MilAventuresConnection"
     connectionString="Data Source=localhost\SQLExpress;
     Initial Catalog=MilAventuresDB;
     User ID=usuari;Password=contrasenya;Encrypt=False"
     providerName="System.Data.SqlClient" />
```

[captura del App.config]

## 5. Patró MVVM

El flux de dades segueix el patró MVVM:

- La **View** (XAML) es vincula a les propietats del **ViewModel**
  mitjançant DataBinding.
- El **ViewModel** conté la lògica de presentació i usa
  **RelayCommand** per als botons.
- El **Model** (repositoris EF) accedeix a la base de dades
  de forma transparent.
- El **NavigationService** gestiona la navegació entre pantalles
  i l'obertura de dialogs.

[captura del flux MVVM amb els projectes]

## 6. Repositoris

Tots els repositoris hereten de GenericRepository<T> que implementa
les operacions CRUD bàsiques. Els repositoris específics sobreescriuen
els mètodes quan necessiten carregar relacions (Include).

[captura del GenericRepository]

## 7. Informes Crystal Reports

Els 4 informes es generen mitjançant DataSets tipats:

1. **Llistat d'Activitats**: Informe simple amb totes les activitats
2. **Reserves per Estat**: Informe agrupat per estat de reserva
3. **Estadístiques**: Totals i mitjanes de reserves
4. **Reserves per Dates**: Reserves filtrades per rang de dates

[captura d'un informe obert]

## 8. Tests

El projecte MilAventures.Tests conté 7 tests:

- 3 tests unitaris positius (càlcul preu, stock, dificultat)
- 3 tests unitaris negatius/validació (dates, preu negatiu, participants)
- 1 test d'integració (crear i recuperar categoria a la BD)

[captura del Test Explorer amb els 7 tests en verd]

## 9. Requisits del Sistema

- Windows 10 o superior
- Visual Studio 2022
- SQL Server Express (inclosa amb VS)
- Crystal Reports Runtime per a VS 2022
- .NET Framework 4.7.2
