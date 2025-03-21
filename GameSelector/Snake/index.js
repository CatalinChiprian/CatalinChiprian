const gameBoard = document.querySelector("#gameBoard");
const ctx = gameBoard.getContext("2d");
const scoreText = document.querySelector("#scoreText");
const highscore = document.querySelector("#highScore");
const resetButton = document.querySelector("#resetBtn");
const borderCollisionButton = document.querySelector("#borderCollisionBtn");
const BackToMenuButton = document.querySelector('#backToMenu');
const gameWidth = gameBoard.width;
const gameHeight = gameBoard.height;
const boardBackground = "white";
const snakeColor = "lightgreen";
const snakeheadColor = "darkslategray"
const snakeBorder = "darkgreen";
const foodColor = "red";
const unitSize = 25;
let timeoutId;
let moveTimeout = 100;
let turnTimeout;
let running = false;
let borderCollision = false;
let xVelocity = 25;
let yVelocity = 0;
let foodX;
let foodY;
let score = 0;
let snake = [
    {x:unitSize * 2, y:0},
    {x:unitSize, y:0},
    {x:0, y:0}
];


BackToMenuButton.addEventListener('click', () => {
    window.location.href = "../index.html";
});
window.addEventListener("keydown", changeDirection);
resetButton.addEventListener("click", resetGame);
borderCollisionButton.addEventListener("click", () => {
    if (running) return;
    borderCollision = !borderCollision;
    borderCollisionButton.style.backgroundColor = borderCollision ? "green" : "red";
});


gameStart();

function  gameStart() {
    running = true;
    scoreText.textContent = score;
    createFood();
    drawFood();
    nextTick();
}

function nextTick() {
    if (running) {
        timeoutId = setTimeout(() => {
            clearBoard();
            drawFood();
            moveSnake();
            checkGameOver();
            drawSnake();
            nextTick();
    }, moveTimeout);
    } else {
        displayGameOver();
    }
}

function clearBoard() {
    ctx.fillStyle = boardBackground;
    ctx.fillRect(0, 0, gameWidth, gameHeight);
}

function createFood() {
    function randomFood(min, max) {
        return Math.round((Math.random() * (max - min) + min) / unitSize) * unitSize;
    }
    foodX = randomFood(0, gameWidth - unitSize);
    foodY = randomFood(0, gameHeight - unitSize);
    // check if snake is not on food
    snake.forEach((snakePart) => {
        if (snakePart.x == foodX && snakePart.y == foodY) {
            createFood();
        }
    });
}

function drawFood() {
    ctx.fillStyle = foodColor;
    ctx.fillRect(foodX, foodY, unitSize, unitSize);
}

function moveSnake() {
    const head = { x: snake[0].x + xVelocity,
                     y: snake[0].y + yVelocity };

    if (!borderCollision) {
        switch (true) {
            case head.x >= gameWidth:
                head.x = 0;
                break;
            case head.x < 0:
                head.x = gameWidth - unitSize;
                break;
            case head.y >= gameHeight:
                head.y = 0;
                break;
            case head.y < 0:
                head.y = gameHeight - unitSize;
                break;
        }
    }
    snake.unshift(head);
    if (head.x == foodX && head.y == foodY) {
        score += 1;
        moveTimeout -= 0.5;
        scoreText.textContent = score;
        createFood();
    } else {
        snake.pop();
    }
}

function drawSnake() {
    ctx.fillStyle = snakeColor;
    ctx.strokeStyle = snakeBorder;
    snake.forEach((snakePart) => {
        if (snakePart == snake[0]) {
            ctx.fillStyle = snakeheadColor;
            ctx.strokeStyle = snakeColor;
        }
        else {
            ctx.fillStyle = snakeColor;
            ctx.strokeStyle = snakeBorder;
        }
        ctx.fillRect(snakePart.x, snakePart.y, unitSize, unitSize);
        ctx.strokeRect(snakePart.x, snakePart.y, unitSize, unitSize);
    })
}

function changeDirection(event) {
    const keyPressed = event.keyCode;
    const LEFT = 37;
    const LEFT2 = 65;
    const RIGHT = 39;
    const RIGHT2 = 68;
    const UP = 38;
    const UP2 = 87;
    const DOWN = 40;
    const DOWN2 = 83;

    const goingUp = (yVelocity == -unitSize);
    const goingDown = (yVelocity == unitSize);
    const goingLeft = (xVelocity == -unitSize);
    const goingRight = (xVelocity == unitSize);

    switch(true) {
        case (keyPressed === LEFT || keyPressed === LEFT2) && !goingRight:
            turnTimeout = setTimeout(() => {
                xVelocity = -unitSize;
                yVelocity = 0;
            }, 50);
            break;
        case (keyPressed === RIGHT || keyPressed === RIGHT2) && !goingLeft:
            turnTimeout = setTimeout(() => {
                xVelocity = unitSize;
                yVelocity = 0;
            }, 50);
            break;
        case (keyPressed === UP || keyPressed === UP2) && !goingDown:
            turnTimeout = setTimeout(() => {
                xVelocity = 0;
                yVelocity = -unitSize;
            }, 50);
            break;
        case (keyPressed === DOWN || keyPressed === DOWN2) && !goingUp:
            turnTimeout = setTimeout(() => {
                xVelocity = 0;
                yVelocity = unitSize;
            }, 50);
            break;
    }
}

function checkGameOver() {

    if (borderCollision) {
        if (snake[0].x >= gameWidth || snake[0].x < 0 || snake[0].y >= gameHeight || snake[0].y < 0) {
            running = false;
        }
    }

    for (let i = 1; i < snake.length; i++) {
        if (snake[i].x == snake[0].x && snake[i].y == snake[0].y) {
            running = false;
        }
    }
}

function displayGameOver() {
    ctx.font = "50px MV Boli";
    ctx.fillStyle = "black";
    ctx.textAlign = "center";
    ctx.fillText("Game Over!", gameWidth / 2, gameHeight / 2);
    running = false;
    if (score > parseInt(highscore.textContent)) {
        highscore.textContent = score;
    }
}

function resetGame() {
    if (score > parseInt(highscore.textContent)) {
        highscore.textContent = score;
    }
    score = 0;
    scoreText.textContent = score;
    xVelocity = unitSize;
    yVelocity = 0;
    snake = [
        {x:unitSize * 2, y:0},
        {x:unitSize, y:0},
        {x:0, y:0}
    ];
    clearTimeout(timeoutId);
    moveTimeout = 100;
    gameStart();
}