# FriendsDebt

Podstawa API w .NET 10 z MediatR, FluentValidation, ASP.NET Core Identity,
Entity Framework Core, PostgreSQL i Swagger UI.

## Uruchomienie w Dockerze

```bash
docker compose up --build
```

Swagger UI będzie dostępny pod adresem `http://localhost:8080/swagger`.
Migracje EF Core są automatycznie stosowane przy starcie kontenera API.
Dane PostgreSQL i klucze Data Protection są przechowywane w nazwanych wolumenach
Dockera, więc przetrwają ponowne utworzenie kontenerów.

## Logowanie w Swagger UI

1. Wywołaj `POST /api/auth/register` z adresem e-mail i silnym hasłem.
2. Wywołaj `POST /api/auth/login?useCookies=false` z tymi samymi danymi.
3. Skopiuj wartość `accessToken` z odpowiedzi.
4. Kliknij **Authorize** i wklej sam token (bez prefiksu `Bearer`).

Po autoryzacji można wywołać `GET /api/profile` oraz `PUT /api/profile`.

## Migracje

Z katalogu głównego rozwiązania:

```bash
dotnet ef migrations add MigrationName \
  --project FriendsDebt.Persistence \
  --startup-project FriendsDebt.Api
```

Poza Dockerem domyślny connection string wskazuje PostgreSQL na `localhost:5432`.
Można go nadpisać zmienną `FRIENDSDEBT_DB_CONNECTION_STRING`.

## Audyt encji

Encje wymagające audytu powinny dziedziczyć po `AuditableEntity`. Zapis zmian
wymaga jawnego przekazania e-maila wykonawcy, niezależnie od tego, czy ma konto:

```csharp
await dbContext.SaveChangesAsync(userEmail, cancellationToken);
```

Dla zalogowanego użytkownika można użyć standardowej wersji — e-mail zostanie
pobrany z bieżącego kontekstu HTTP:

```csharp
await dbContext.SaveChangesAsync(cancellationToken);
```

Jeśli żądanie jest anonimowe i zawiera zmiany encji audytowalnych, należy użyć
przeciążenia przyjmującego `userEmail`.

Dodanie encji ustawia autora, czas UTC i status `Active`. Modyfikacja uzupełnia
pola modyfikacji, a usunięcie jest zamieniane na soft delete ze statusem
`Inactive` i informacją o osobie dezaktywującej.
