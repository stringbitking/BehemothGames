var canvas, ctx, gameLoop,
	upDown, downDown;

var playerPad, enemyPad, images, ball;

var screenHeight, screenWidth;
screenWidth = 1000;
screenHeight = 500;

var roomId = parseInt($("#gameId").val());
var isMainPlayer = false;
var frameCounter = 0;

// Declare a proxy to reference the hub.
var spaceHub = $.connection.spaceHub;

spaceHub.client.toAllRegisterShip = function (ship) {
    if (enemyPad && ship.id == enemyPad.id) {
        if (ship.isRejoining) {
            spaceHub.server.sendShip(playerShip, roomId);
        }
    }
    else {
        console.log("Registering ship with id: " + ship.id);

        var newPad = new PlayerPad(ship.x, ship.y);
        newPad.id = ship.id;
        enemyPad = newPad;

        spaceHub.server.sendShip(playerPad, roomId);
    }
};

spaceHub.client.addMessage = function (msg) {

    console.log(msg);
};

spaceHub.client.displayWinner = function () {
    ctx.save();
    ctx.fillStyle = "blue";
    ctx.font = "bold 65px Arial";
    if (Math.abs(ball.x - playerPad.x) < 100) {
        
        ctx.fillText("You loose!", 200, 300);
        
    }
    else {
        ctx.fillText("You win!", 200, 300);
    }
    ctx.restore();
};

spaceHub.client.refreshShipPosition = function (ship) {

    if (ship.id != playerPad.id) {
        if (enemyPad) {
            enemyPad.y = ship.y;
            enemyPad.x = ship.x;
        }
    }
};

spaceHub.client.refreshBallPosition = function (newBall) {

    ball.x = newBall.x;
    ball.y = newBall.y;
    ball.velX = newBall.velX;
    ball.velY = newBall.velY;
};

spaceHub.client.makeMainPlayer = function () {
    playerPad.x = 10;
    isMainPlayer = true;
};

spaceHub.client.startGame = function () {
    ball = new Ball(screenWidth / 2, screenHeight / 2);
    ball.velX = -3;
};

$.connection.hub.start().done(function () {

    spaceHub.server.joinGameRoom(roomId);
    loadImages();

    playerPad = new PlayerPad(screenWidth - 50, screenHeight / 2);
    registerPad();
});

//---------------------- The Game -----------------------
function init() {
    canvas = document.createElement('canvas');
    document.getElementById("game-container").appendChild(canvas);
    canvas.width = screenWidth;
    canvas.height = screenHeight;
    ctx = canvas.getContext('2d');

    window.addEventListener('keydown', keyDown);
    window.addEventListener('keyup', keyUp);



    gameLoop = setInterval(loop, 1000 / 50);

}

function loop() {
    ctx.fillStyle = 'black';
    ctx.fillRect(0, 0, screenWidth, screenHeight);
    checkKeys();

    updateBall();

    checkCollisions();

    renderPads();
    renderBall();

    // refresh ball
    if (frameCounter % 20 == 0) {
        if (isMainPlayer) {
            spaceHub.server.refreshBall(ball, roomId);
        }
    }

    frameCounter++;
}

function checkCollisions() {
    if (!ball) {
        return;
    }

    if((ball.x > playerPad.x && ball.x < (playerPad.x + playerPad.width)) &&
        (ball.y > playerPad.y && ball.y < (playerPad.y + playerPad.height))) {
        console.log("bounce");
        ball.velX = ball.velX * (-1);
        if (ball.velX > 0) {
            ball.x = playerPad.x + playerPad.width + 2;
        }
        else {
            ball.x = playerPad.x - 2;
        }
    }

    if ((ball.x > enemyPad.x && ball.x < (enemyPad.x + enemyPad.width)) &&
        (ball.y > enemyPad.y && ball.y < (enemyPad.y + enemyPad.height))) {
        console.log("bounce");
        ball.velX = ball.velX * (-1);
        if (ball.velX > 0) {
            ball.x = enemyPad.x + enemyPad.width + 2;
        }
        else {
            ball.x = enemyPad.x - 2;
        }
    }

    if (ball.y < 10 || ball.y > screenHeight - 10) {
        ball.velY = ball.velY * (-1);
    }

    if (ball.x < 1 || ball.x > screenWidth - 1) {
        gameOver();
        ball.velX = ball.velX * (-1);
    }
}

function gameOver() {
    clearInterval(gameLoop);
    ctx.fillStyle = 'black';
    ctx.fillRect(0, 0, screenWidth, screenHeight);

    ctx.save();
    ctx.fillStyle = "blue";
    ctx.font = "bold 65px Arial";
    ctx.fillText("GAME OVER!", 200, 200);
    ctx.restore();

    spaceHub.server.checkWin(roomId);
}

function updateBall() {
    if (ball) {
        ball.update();
    }
}

function renderBall() {
    if (ball) {
        ball.render(ctx);
    }
}

function renderPads() {
    if (enemyPad) {
        enemyPad.render(ctx);
    }

    playerPad.render(ctx);
}

function loadImages() {
    images = [];
    var imgPlayerPad = new Image();
    imgPlayerPad.src = "../../img/ranger.jpg";
    images['player-pad'] = imgPlayerPad;

    var imgFireball = new Image();
    imgFireball.src = "../../img/fireball.png";
    images['fireball'] = imgFireball;
}

function registerPad() {
    var localId = localStorage.getItem("id");
    console.log("is main pl: " + isMainPlayer);
    if (!localId) {
        requester.getJSON("/api/GamesApi/register").then(function (data) {
            playerPad.id = data;
            localStorage.setItem("id", playerPad.id);
            spaceHub.server.sendShip(playerPad, roomId);
            init();
        });
    }
    else {
        playerPad.id = localId;
        playerPad.isRejoining = true;
        localStorage.setItem("id", playerPad.id);
        spaceHub.server.sendShip(playerPad, roomId);
        playerPad.isRejoining = false;
        init();
    }

}

//---------------------- Events -----------------------
function checkKeys() {

    if (upDown) {
        if (playerPad.y >= 0) {
            playerPad.y -= 10;
            spaceHub.server.refreshShip(playerPad, roomId);
        }
    } else if (downDown) {
        if (playerPad.y <= screenHeight - playerPad.height) {
            playerPad.y += 10;
            spaceHub.server.refreshShip(playerPad, roomId);
        }
    }
}

function keyDown(e) {
    e.preventDefault();
    if (e.keyCode == 38) {
        upDown = true;
    } else if (e.keyCode == 40) {
        downDown = true;
    }

}

function keyUp(e) {

    if (e.keyCode == 38) {
        upDown = false;
    } else if (e.keyCode == 40) {
        downDown = false;
    }
}


//---------------------- Classes -----------------------

function PlayerPad(x, y) {
    this.id = 0;
    this.x = x;
    this.y = y;
    this.width = 40;
    this.height = 60;
    this.isRejoining = false;
    this.lives = 1;

    this.render = function (ctx) {
        var self = this;
        ctx.save();

        ctx.drawImage(images['player-pad'], this.x, this.y, this.width, this.height);

        ctx.restore();
    }
}

function Ball(x, y) {

    this.x = x;
    this.y = y;
    this.velX = Math.random() * 4 - 2;
    this.velY = Math.random() * 4 - 2;
    this.size = 20;

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
