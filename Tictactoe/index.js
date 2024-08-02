const cells = document.querySelectorAll('.cell');
const statusText = document.querySelector('#statusText');
const restartButton = document.querySelector('#restartButton');
const resetButton = document.querySelector('#resetButton');
const backButtons = document.querySelectorAll('#backButton');
const onePlayerButton = document.querySelector('#onePlayerButton');
const twoPlayerButton = document.querySelector('#twoPlayerButton');
const difficultyButtons = {
    easy: document.querySelector('#easyButton'),
    hard: document.querySelector('#hardButton')
};
const scoreboard = document.querySelector('#scoreboard');
const menuContainer = document.querySelector('#menuContainer');
const gameContainer = document.querySelector('#gameContainer');
const diffcultyContainer = document.querySelector('#difficultyContainer');
const winConditions = [
    [0, 1, 2],
    [3, 4, 5],
    [6, 7, 8],
    [0, 3, 6],
    [1, 4, 7],
    [2, 5, 8],
    [0, 4, 8],
    [2, 4, 6]
];
let options = ["", "", "", "", "", "", "", "", ""];
let currentPlayer = "Player1";
let currentPlayers = 2;
let humanPlayer = "Player1";
let currentDifficulty;
let aiWon = false;
let roundFinished = false;

let players = {
    
    Player1: "X",
    Player2: "O",
    Draw: "D"
};

let winsNumber = {
    Player1: 0,
    Player2: 0,
    Draw: 0
};

let gameActive = false;


backButtons.forEach(button => {
    button.addEventListener('click', BackToMenu);
});

onePlayerButton.addEventListener('click', Setup1PlayerGame);

twoPlayerButton.addEventListener('click', () => {
    startGame(2);
});


function Setup1PlayerGame() {
    menuContainer.style.display = 'none';
    diffcultyContainer.style.display = 'block';
    difficultyButtons.easy.addEventListener('click', () => {
        startGame(1, 'easy');
    });
    difficultyButtons.hard.addEventListener('click', () => {
        startGame(1, 'hard');
    });
}

function startGame(players, difficulty) {
    menuContainer.style.display = 'none';
    diffcultyContainer.style.display = 'none';
    gameContainer.style.display = 'block';
    currentPlayers = players;
    currentDifficulty = difficulty;
    initializeGame();
}

function initializeGame() {
    cells.forEach(cell => {
        cell.addEventListener('click', cellClicked);
    });
    restartButton.addEventListener('click', RestartGame);
    resetButton.addEventListener('click', ResetScoreboard);
    statusText.textContent = `${currentPlayer}'s turn`;
    gameActive = true;
}

function cellClicked() {
    const index = this.getAttribute("cellIndex");

    if (options[index] !== "" || !gameActive) {
        return;
    }

    updateCell(this, index);
    checkWinner();

    if (currentPlayers === 1 && gameActive && currentDifficulty === 'easy') {
        easyAi();
    }
    else if (currentPlayers === 1 && gameActive && currentDifficulty === 'hard') {
        hardAi();
    }
}

function easyAi() {
    setTimeout(() => {
    let emptyCells = [];
    options.forEach((option, index) => {
        if (option === "") {
            emptyCells.push(index);
        }
    });

    if (emptyCells.length > 0) {
        const randomIndex = emptyCells[Math.floor(Math.random() * emptyCells.length)];
        const cell = document.querySelector(`[cellIndex="${randomIndex}"]`);
        updateCell(cell, randomIndex);
        checkWinner();
    }
}, 200); // Delay AI move by 2 milliseconds
}

function hardAi() {
    setTimeout(() => {
        let bestScore = -Infinity;
        let move;
        for (let i = 0; i < options.length; i++) {
            if (options[i] === "") {
                options[i] = "O";
                let score = minimax(options, 0, false);
                options[i] = "";
                if (score > bestScore) {
                    bestScore = score;
                    move = i;
                }
            }
        }
        const cell = document.querySelector(`[cellIndex="${move}"]`);
        updateCell(cell, move);
        checkWinner();
    }, 200); // Delay AI move by 2 milliseconds
}

function minimax(board, depth, isMaximizing) {
    let scores = {
        O: 1,
        X: -1,
        tie: 0
    };

    let result = checkWinnerForMinimax();
    if (result !== null) {
        return scores[result];
    }

    if (isMaximizing) {
        let bestScore = -Infinity;
        for (let i = 0; i < board.length; i++) {
            if (board[i] === "") {
                board[i] = "O";
                let score = minimax(board, depth + 1, false);
                board[i] = "";
                bestScore = Math.max(score, bestScore);
            }
        }
        return bestScore;
    } else {
        let bestScore = Infinity;
        for (let i = 0; i < board.length; i++) {
            if (board[i] === "") {
                board[i] = "X";
                let score = minimax(board, depth + 1, true);
                board[i] = "";
                bestScore = Math.min(score, bestScore);
            }
        }
        return bestScore;
    }
}

function checkWinnerForMinimax() {
    for (let i = 0; i < winConditions.length; i++) {
        const winCondition = winConditions[i];
        let a = options[winCondition[0]];
        let b = options[winCondition[1]];
        let c = options[winCondition[2]];

        if (a === "" || b === "" || c === "") {
            continue;
        }

        if (a === b && b === c) {
            return a;
        }
    }

    if (!options.includes("")) {
        return "tie";
    }

    return null;
}

function updateCell(cell, index) {
    options[index] = currentPlayer === "Player1" ? "X" : "O";
    cell.textContent = options[index];
}

function changePlayer() {
    currentPlayer = (currentPlayer === "Player1") ? "Player2" : "Player1";
    statusText.textContent = `${currentPlayer}'s turn`;
}

function checkWinner() {
    let roundWon = false;

    for (let i = 0; i < winConditions.length; i++) {
        const winCondition = winConditions[i];
        let a = options[winCondition[0]];
        let b = options[winCondition[1]];
        let c = options[winCondition[2]];

        if (a === "" || b === "" || c === "") {
            continue;
        }

        if (a === b && b === c) {
            roundWon = true;
            break;
        }
    }

    if (roundWon) {
        statusText.textContent = `${currentPlayer} wins!`;
        winsNumber[currentPlayer]++;
        if (currentPlayers === 1 && currentPlayer !== humanPlayer) {
            aiWon = true;
        }
        else if(currentPlayers === 1 && currentPlayer == humanPlayer) {
            aiWon = false;
        }
        updateScoreboard();
        gameActive = false;
        roundFinished = true;
        return;
    }
    else if (!options.includes("")) {
        statusText.textContent = "Draw!";
        currentPlayer = "Player1";
        winsNumber.Draw++;
        updateScoreboard();
        roundFinished = true;
        gameActive = false;
        aiWon = false;
        return;
    }
    else {
        changePlayer();
        roundFinished = false;
    }
}

function updateScoreboard() {
    scoreboard.textContent = `Score: Player1 - ${winsNumber.Player1} | Player2 - ${winsNumber.Player2} | Draw - ${winsNumber.Draw}`;
}

function ResetScoreboard() {
    winsNumber = {
        Player1: 0,
        Player2: 0,
        Draw: 0
    };
    updateScoreboard();
}

function BackToMenu() {
    Clear();
    ResetScoreboard();
    currentPlayer = "X";
    gameContainer.style.display = 'none';
    menuContainer.style.display = 'block';
    diffcultyContainer.style.display = 'none';
}

function RestartGame() {
    Clear();
    gameActive = true
        if (!roundFinished && aiWon)
        changePlayer();
    statusText.textContent = `${currentPlayer}'s turn`;
        
    if (aiWon) {
        if (currentDifficulty === 'easy') {
            easyAi();
        }
        else
            hardAi();
    }
}

function Clear() {
    cells.forEach(cell => {
        cell.textContent = "";
    });
    options = ["", "", "", "", "", "", "", "", ""];
}