"use strict";

const app = {
    content: document.getElementById('content'),
    logMessage: document.getElementById('logMessage'),
    rentMessage: document.getElementById('rentMessage'),
    regBtn: document.getElementById('reg'),
    logBtn: document.getElementById('login'),
    startpageBtn: document.getElementById('startpage'),
    studioBtn: document.getElementById('studios'),
    filmerBtn: document.getElementById('filmer'),
    jwtholder: "",
    id: 0
};

app.startpageBtn.addEventListener('click', function () {
    app.rentMessage.innerHTML = "";

    if (app.jwtholder === "") {
        app.logMessage.innerHTML = "";
    }

    app.content.innerHTML = `<div id="intro">
    <h2>Välkommen!</h2>
    <h3>Filmer</h3>
    <p>Här kan du se alla tillgängliga filmer som besökare</p>
    <h3>Registrera</h3>
    <p>Du måste vara registrerad filmstudio för att komma åt funktioner som att se era lånade filmer, låna exemplar och återlämna lånade exemplar.</p>
    <h3>Logga in</h3>
    <p>Inloggning för filmstudio (kräver användarnamn och lösenord)</p>
    <h3>API Dokumentation (för utvecklare)</h3>
    <p>Gå till följande sida: <a href="/swagger/index.html">API Dokumentation</a></p>
    </div>`;
});

app.filmerBtn.addEventListener('click', function getFilms() {
    app.rentMessage.innerHTML = "";

    if (app.jwtholder === "") {
        fetch('/api/films/')
            .then(resp => resp.json())
            .then(films => showFilms(films))
    }
    else {
        fetch('/api/films/', {
            method: 'GET',
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
                'Authorization': `Bearer ${app.jwtholder}`
            }
        })
            .then(resp => resp.json())
            .then(films => showFilms(films))
    }
});

async function showFilms(films) {
    if (app.jwtholder === "") {
        app.logMessage.innerHTML = "";
    }

    app.content.innerHTML = `<div id="filmsDiv"></div>`;
    let filmsDiv = document.getElementById('filmsDiv');

    for (var i = 0; i < films.length; i++) {

        let film = films[i];
        if (app.jwtholder !== "") {
            if (film.availablefCopies === 0) {
                filmsDiv.innerHTML += `<div id='film2'><h3>Inga tillgängliga kopior</h3><h4>Film Id: ${film.filmId}</h4><h4>${film.name}</h4>
                <p>Regissör:   ${film.director}</p>
                <p>Utgiven år:     ${film.releaseYear}</p>
                <p>Ursprung:       ${film.country}</p>
                </div>`
            }
            else {
                filmsDiv.innerHTML += `<div id='film'><h3>Film Id: ${film.filmId}</h3><h3>${film.name}</h3>
                <p>Regissör:   ${film.director}</p>
                <p>Utgiven år:     ${film.releaseYear}</p>
                <p>Ursprung:       ${film.country}</p>
                <p>Tillgängliga kopior: ${film.availablefCopies}</p></div>`
            }
        }
        else {
            filmsDiv.innerHTML += `<div id='film'><h3>${film.name}</h3>
            <p>Regissör:   ${film.director}</p>
            <p>Utgiven år:     ${film.releaseYear}</p>
            <p>Ursprung:       ${film.country}</p></div>`
        }
    }

    if (app.jwtholder !== "") {
        filmsDiv.innerHTML += `<div id='film'><h2>Låna</h2>
            <form id="rentForm">
                <div class="form-group">
                    <label for="filmId">Ange Film Id:</label><br />
                    <input type="text" class="form-control" id="filmId">
                </div>
                <div>
                    <br /><button type="submit" class="btn btn-dark" id="rent">LÅNA FILM</button>
                </div>
            </form></div>`;

        let rentForm = document.getElementById('rentForm');

        rentForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            let id = document.getElementById('filmId').value;

            let response = await fetch('/api/films/rent?Id=' + id + '&studioid=' +app.id, {
                method: 'POST',
                headers: {
                    'Content-type': 'application/json; charset=UTF-8',
                    'Authorization': `Bearer ${app.jwtholder}`
                }
            })
            let result = await response.json();
            showRented(result);
        });
    }
}

async function showRented(result) {
    app.rentMessage.innerHTML = `<h4 class="message">Ni har nu lånat ${result.filmName}</h4>`;
    getUpdatedFilms();
}

app.studioBtn.addEventListener('click', function () {
    fetch('/api/filmstudios/')
        .then(resp => resp.json())
        .then(studios => showPublicFilmStudios(studios))
});

async function showPublicFilmStudios(filmStudios) {
    app.rentMessage.innerHTML = "";

    if (app.jwtholder === "") {
        app.logMessage.innerHTML = "";
    }

    app.content.innerHTML = `<div id="studiosDiv"></div>`;
    let studiosDiv = document.getElementById('studiosDiv');

    for (var i = 0; i < filmStudios.length; i++) {
        let studio = filmStudios[i];
        studiosDiv.innerHTML += `<div id="studio"><h3>${studio.name}</h3>`
    }
}

app.regBtn.addEventListener('click', function () {
    app.rentMessage.innerHTML = "";

    if (app.jwtholder !== "") {
        getUserStudio(app.id)
    }

    if (app.jwtholder === "") {
        app.logMessage.innerHTML = "";

        app.content.innerHTML =
            `<h2>Registrera Filmstudio</h2>
            <form id="registerForm">
                <div class="form-group">
                    <label for="name">Filmstudio</label><br />
                    <input type="text" class="form-control" id="name" required>
                </div>
                <div class="form-group">
                    <label for="location">Ort</label><br />
                    <input type="text" class="form-control" id="city" required>
                </div>
                <div class="form-group">
                    <label for="email">E-post</label><br />
                    <input type="email" class="form-control" id="email" required>
                </div>
                <div class="form-group">
                    <label for="password">Lösenord</label><br />
                    <input type="password" class="form-control" id="password" required><br />
                </div>
                <div>
                    <br /><button type="submit" class="btn btn-dark" id="submitRegister">Registrera</button>
                </div>
            </form>`

        register();
    }
});

async function register() {

    let registerForm = document.getElementById('registerForm');

    registerForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        let name = document.getElementById('name').value;
        let city = document.getElementById('city').value;
        let email = document.getElementById('email').value;
        let password = document.getElementById('password').value;

        fetch('/api/filmstudios/register', {
            method: 'POST',
            body: JSON.stringify({
                Name: name,
                City: city,
                Email: email,
                Password: password
            }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            },
        })
        showLogin()
    });
}

app.logBtn.addEventListener('click', function showLogin() {
    app.rentMessage.innerHTML = "";

    if (app.jwtholder === "") {
        app.logMessage.innerHTML = "";
    }

    app.content.innerHTML =
        `<h2>Logga in</h2>
            <form id="loginForm">
                <div class="form-group">
                    <label for="Email">E-post</label><br />
                    <input type="email" class="form-control" id="email">
                </div>
                <div class="form-group">
                    <label for="Password">Lösenord</label><br />
                    <input type="password" class="form-control" id="password"><br />
                </div>
                <div>
                    <br /><button type="submit" class="btn btn-dark" id="submitLogin">Logga in </button>
                </div>
            </form>`;
    loginUser();
});

async function showLogin() {
    app.rentMessage.innerHTML = "";

    if (app.jwtholder === "") {
        app.logMessage.innerHTML = "";
    }

    app.content.innerHTML =
        `<h2>Logga in</h2>
            <form id="loginForm">
                <div class="form-group">
                    <label for="Email">E-post</label><br />
                    <input type="email" class="form-control w-25" id="email">
                </div>
                <div class="form-group">
                    <label for="Password">Lösenord</label><br />
                    <input type="password" class="form-control w-25" id="password"><br />
                </div>
                <div>
                    <br /><button type="submit" class="btn btn-dark" id="submitLogin">Logga in </button>
                </div>
            </form>`;
    loginUser();
}

async function loginUser() {
    let loginForm = document.getElementById("loginForm");

    loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        let email = document.getElementById('email').value;
        let password = document.getElementById('password').value;

        let response = await fetch('/api/users/authenticate', {
            method: 'POST',
            body: JSON.stringify({
                Email: email,
                Password: password
            }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            },
        })

        let user = await response.json();
        app.jwtholder = user.token;
        app.id = user.filmStudioId;

        if (app.jwtholder !== undefined) {
            app.filmerBtn.innerHTML = 'LÅNA FILM'
            app.regBtn.innerHTML = 'LÅNADE FILMER';
            app.logBtn.innerHTML = 'LOGGA UT';
            getUserStudio(app.id);
        }
        if (app.jwtholder === undefined) {
            app.content.innerHTML = '<h1>Ingen filmstudio registrerad.</h1>';
        }

        app.logBtn.addEventListener('click', function () {

            if (app.jwtholder !== "") {
                app.jwtholder = "";
                app.id = 0;
                app.content.innerHTML = "";
                app.logMessage.innerHTML = `<h2>Du är utloggad</h2><h2>Välkommen åter!</h2>`;

                app.filmerBtn.innerHTML = 'Filmer'
                app.regBtn.innerHTML = 'REGISTRERA';
                app.logBtn.innerHTML = 'LOGGA IN';
            }
        });
    });
}

function getUserStudio(id) {
    fetch('/api/FilmStudios/' + id)
        .then(resp => resp.json())
        .then(studio => showStudio(studio))
}

function showStudio(studio) {
    getRentedFilms();
    app.logMessage.innerHTML = `<h2>Välkommen! Du är inloggad för <span id="spanStudio">` + studio.name + `</span></h2>`;
    app.rentMessage.innerHTML = "";
}

function getRentedFilms() {

    fetch('/api/mystudio/rentals', {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
            'Authorization': `Bearer ${app.jwtholder}`
        }
    })
        .then(resp => resp.json())
        .then(rented => showRentedFilms(rented))
}

async function showRentedFilms(rented) {

    app.content.innerHTML = `<div id="filmsDiv"></div>`;
    let filmsDiv = document.getElementById('filmsDiv');

    if (rented.length === 0) {
        filmsDiv.innerHTML += `<div><h2>Ni har inga aktiva lån</h2></div>`;
    }

    else if (rented.length >= 1) {
        filmsDiv.innerHTML += `<div><h2>[Ni har följande aktiva lån]</h2></div>`;

        for (var i = 0; i < rented.length; i++) {
            let film = rented[i];
            let id = film.filmId;
            filmsDiv.innerHTML += `<div id='film'><h3>${film.filmName}</h3><p>Film Id: ` + id + `</p></div>`
        }

        filmsDiv.innerHTML += `<div id='returnDiv'><h2>[Återlämna]</h2>
            <form id="returnForm">
                <div class="form-group">
                    <label for="FilmId">Ange Film Id:</label><br />
                    <input type="text" class="form-control" id="filmId">
                </div>
                <div>
                    <br /><button type="submit" class="btn btn-dark" id="return">ÅTERLÄMNA FILM</button>
                </div>
            </form></div>`;

        let returnForm = document.getElementById('returnForm');

        returnForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            let id = document.getElementById('filmId').value;

            let response = await fetch('/api/films/return?Id=' + id + '&studioid=' +app.id, {
                method: 'PUT',
                headers: {
                    'Content-type': 'application/json; charset=UTF-8',
                    'Authorization': `Bearer ${app.jwtholder}`
                }
            })

            let result = await response.json();
            showReturned(result);
        });
    }
}

async function showReturned(result) {
    app.rentMessage.innerHTML = `<h4 class="message">${result.filmName} är nu återlämnad.</h4>`;

    getRentedFilms();
}

async function getUpdatedFilms() {
    fetch('/api/films', {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
            'Authorization': `Bearer ${app.jwtholder}`
        }
    })
        .then(resp => resp.json())
        .then(films => showFilms(films))
}