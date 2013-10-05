var canvas, ctx, gameLoop,
	leftDown, rightDown;

var playerShip, friendlyShips,
    bullets, invaders, particles,
    images, projectiles, stars;

var screenHeight, screenWidth;
screenWidth = 1000;
screenHeight = 500;

init();

function init() {

    canvas = document.createElement('canvas');
    document.getElementById("game-container").appendChild(canvas);
    canvas.width = screenWidth;
    canvas.height = screenHeight;
    ctx = canvas.getContext('2d');

    window.addEventListener('keydown', keyDown);
    window.addEventListener('keyup', keyUp);

    bullets = [];
    projectiles = [];
    particles = [];
    invaders = [];

    resetStars();
    resetInvaders();
    loadImages();
    playerShip = new PlayerShip(screenWidth / 2, screenHeight - 80);
    gameLoop = setInterval(loop, 1000 / 50);

}

function loop() {
    ctx.fillStyle = 'black';
    ctx.fillRect(0, 0, screenWidth, screenHeight);
    checkKeys();

    updateBullets();

    updateInvaders();

    checkCollisions();

    renderStars();
    renderBullets();
    renderInvaders();

    updateParticles();
    updateStars();

    if (playerShip.lives > 0) {
        playerShip.render(ctx);
    }

    // refresh invaders

}

function resetInvaders() {
    invaders = [];

    for (var col = 0; col < 10; col++) {
        for (var row = 0; row < 3; row++) {
            var invader = new Invader(col * 80 + 50, row * 60 + 30);
            invaders.push(invader);

        }
    }
}

function resetStars() {
    stars = [];

    for (var i = 0; i < 50; i++) {
        var star = new Star(Math.random() * screenWidth, Math.random() * screenHeight);
        stars.push(star);
    }
}

function updateBullets() {
    for (var i = 0; i < bullets.length; i++) {

        bullets[i].update();
    }

    for (var i = 0; i < projectiles.length; i++) {

        projectiles[i].update();
    }
}

function updateParticles() {
    for (var i = 0; i < particles.length; i++) {
        particles[i].update();
        particles[i].render(ctx);
    }
}

function renderBullets() {
    for (var i = 0; i < bullets.length; i++) {

        bullets[i].render(ctx);
    }

    for (var i = 0; i < projectiles.length; i++) {

        projectiles[i].render(ctx);
    }
}

function updateInvaders() {
    var hittingWall = false;

    for (var i = 0; i < invaders.length; i++) {
        if (invaders[i].x <= 0 || (invaders[i].x >= screenWidth - 40)) {
            hittingWall = true;
            break;
        }
    }

    for (var i = 0; i < invaders.length; i++) {

        if (hittingWall) {
            invaders[i].velX = invaders[i].velX * (-1);
        }

        invaders[i].update();
    }
}

function renderInvaders() {
    for (var i = 0; i < invaders.length; i++) {

        invaders[i].render(ctx);
    }
}

function renderStars() {
    for (var i = 0; i < stars.length; i++) {
        stars[i].render(ctx);
    }
}

function updateStars() {
    for (var i = 0; i < stars.length; i++) {
        stars[i].update();
    }
}

function makeExplosion(x, y) {
    for (var i = 0; i < 15; i++) {

        var p = new Particle(x, y);
        particles.push(p);

    }
}

function loadImages() {
    images = [];
    var imgPlayerShip = new Image();
    imgPlayerShip.src = "../../img/player-ship.png";
    images['player-ship'] = imgPlayerShip;

    var imgInvaderShip = new Image();
    imgInvaderShip.src = "../../img/invader.png";
    images['invader-ship'] = imgInvaderShip;

    var imgFireball = new Image();
    imgFireball.src = "../../img/fireball.png";
    images['fireball'] = imgFireball;

    var imgStar = new Image();
    imgStar.src = "../../img/star.png";
    images['star'] = imgStar;
}

function checkCollisions() {

    for (var i = 0; i < bullets.length; i++) {
        var bullet = bullets[i];

        for (var j = 0; j < invaders.length; j++) {
            var invader = invaders[j];

            if ((bullet.x > invader.x) && (bullet.x < invader.x + invader.width)
				&& (bullet.y > invader.y) && (bullet.y < invader.y + invader.height)) {

                invaders.splice(j, 1);
                j--;
                bullets.splice(i, 1);
                i--;

                makeExplosion(invader.x + invader.width / 2, invader.y + invader.height / 2)

            }
        }
    }

}

function checkKeys() {

    if (leftDown) {
        if (playerShip.x >= 0) {
            playerShip.x -= 10;
        }
    } else if (rightDown) {
        if (playerShip.x <= screenWidth - 50) {
            playerShip.x += 10;
        }
    }
}

function keyDown(e) {
    e.preventDefault();
    if (e.keyCode == 37) {
        leftDown = true;
    } else if (e.keyCode == 39) {
        rightDown = true;
    }

    if (e.keyCode == 32) {
        var bullet = new Bullet(playerShip.x + playerShip.width / 2, playerShip.y);
        bullets.push(bullet);
    }
}

function keyUp(e) {

    if (e.keyCode == 37) {
        leftDown = false;
    } else if (e.keyCode == 39) {
        rightDown = false;
    }
}


//---------------------- Classes -----------------------

function PlayerShip(x, y) {
    this.id = 0;
    this.x = x;
    this.y = y;
    this.width = 60;
    this.height = 40;
    this.isRejoining = false;
    this.lives = 1;

    this.render = function (ctx) {
        var self = this;
        ctx.save();

        ctx.drawImage(images['player-ship'], this.x, this.y, 50, 50);

        ctx.restore();
    }
}

function Bullet(x, y) {
    this.id = playerShip.id;
    this.x = x;
    this.y = y;

    this.update = function () {
        this.y -= 20;

    }
    this.render = function (ctx) {
        ctx.fillStyle = 'white';
        ctx.fillRect(this.x, this.y, 4, 10);
    }
}

function Projectile(x, y) {
    this.x = x;
    this.y = y;

    this.update = function () {
        this.y += 1;

    }
    this.render = function (ctx) {
        ctx.fillStyle = 'red';
        ctx.fillRect(this.x, this.y, 4, 10);
    }
}

function Invader(x, y) {

    this.x = x;
    this.y = y;
    this.width = 60;
    this.height = 40;
    this.velX = 1;

    this.update = function () {
        this.x += this.velX;

        var rndNum = Math.floor((Math.random() * 1000) + 1);

        if (rndNum > 999) {
            var projectile = new Projectile(this.x + 20, this.y + 20);
            projectiles.push(projectile);
        }
    }

    this.render = function (ctx) {
        ctx.save();

        ctx.drawImage(images['invader-ship'], this.x, this.y, 50, 50);

        ctx.restore();
    }
}

function Particle(x, y) {

    this.x = x;
    this.y = y;
    this.velX = Math.random() * 20 - 10;
    this.velY = Math.random() * 20 - 10;
    this.size = 10;
    this.update = function () {
        this.x += this.velX;
        this.y += this.velY;

    }
    this.render = function (ctx) {
        ctx.save();

        ctx.drawImage(images['fireball'], this.x, this.y, this.size, this.size);

        ctx.restore();
    }

}

function Star(x, y) {

    this.x = x;
    this.y = y;
    this.size = Math.random() * 15 + 1;
    this.velY = (this.size - 5) / 10;

    if (this.velY < 0) {
        this.velY = this.size / 10;
    }

    this.update = function () {
        this.y += this.velY;

        if (this.y >= screenHeight) {
            this.y = 0;
            this.x = Math.random() * screenWidth;
        }
    }

    this.render = function (ctx) {
        ctx.save();

        if (0 < this.x && this.x < screenWidth &&
            0 < this.y && this.y < screenHeight)
            ctx.drawImage(images['star'], this.x, this.y, this.size, this.size);

        ctx.restore();
    }

}








