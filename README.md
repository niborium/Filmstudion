# Filmstudion
Inlämningsuppgift av Robin Karlsson (WU21) för Dynamiska Webbsystem 2 (Filmstudion).\
Projektet startas med dotnet run i terminalen (mappen: Filmstudion), du kan då surfa in i webbläsaren på localhost adressen och den port som anges för att nå gränsnittet.\
API-dokumentationen når du via /swagger/index.html och beskrivning av endpoints (åtkomstpunkter) finner du nedan.

## Åtkomstpunkter
## FILMS
### /api/films
	# PUT
	Lägga till nya filmer (autentiserad som administratör)
### /api/films
	# GET
	Hämtar alla filmer.
### /api/films/{id}
	# GET
	Hämta information om en enkild film.
### /api/films/{id}
	# PATCH
	Ändra informationen om en film om du är (autentiserad som administratör)
### /api/films/{id}
	# PUT
	Ändra antalet tillgängliga exemplar som kan lånas från varje film. (autentiserad som administratör)
### /api/films/rent?Id={id}&studioid={studioid}
	# POST
	Kunna låna en kopia av en film om kopior finns tillgängliga. (autentiserad filmstudio)
	Notera: parametern {id} motsvarar id:t för filmen som ska lånas och {studioid} id:t för studion som ska låna filmen.
### /api/films/return?Id={id}&studioid={studioid}
	# POST
	Returnera en lånad kopia av en film. (autentiserad filmstudio)
	Notera: parametern {id} motsvarar id:t för filmen som ska lånas och {studioid} id:t för studion som ska låna filmen.
## FILMSTUDIOS
### /api/filmstudio/register
	# POST
	Registrera ny filmstudio.

### /api/filmstudios
	# GET
	Hämta alla filmstudior.
### /api/filmstudio/{id}
	# GET
	Hämta information om en enskild filmstudio.
### /api/mystudio/rentals
	# GET
	Filmstudio måste kunna hämta vilka filmer denna studio för närvarande lånat. (autentiserad filmstudio)

## USERS
### /api/users/register
	# POST
	Registrera dig som administratör, ej för filmstudios som ska använda ovan åtkomstpunkt /api/filmstudio/register.
### /api/users/authenticate
	# POST
	Autentisera dig som administratör eller filmstudio.
