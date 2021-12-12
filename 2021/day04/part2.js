const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let numbers = null;
    let boards = [];
    let currentBoard = [];
    for await (const line of rl) {
        if (line.length === 0) {
            continue;
        }
        
        if (!numbers) {
            numbers = line.split(",").map(n => parseInt(n, 10));
        } else {
            let row = line
                .split(/\s+/)
                .map(n => parseInt(n, 10))
                .filter(isFinite);
            currentBoard.push(...row);
            if (currentBoard.length === 25) {
                boards.push(currentBoard);
                currentBoard = [];
            }
        }
        
    }
    
    function checkWin(board) {
        lines = _.chunk(board, 5); // rows
        
        for (let i = 0; i < 5; ++i) { // cols
            lines.push(board.filter((n, j) => j % 5 === i));
        }
        
        for (let line of lines) {
            if (!line.some(isFinite)) {
                return true;
            }
        }
        
        return false;
    }
    
    for (let number of numbers) {
        for (let boardIndex = boards.length - 1; boardIndex >= 0; boardIndex--) {
            let board = boards[boardIndex]; 
            for (let index in board) {
                if (board[index] === number) {
                    board[index] = NaN;
                    if (checkWin(board)) {
                        if (boards.length === 1) {
                            sum = board.reduce((prev, cur) => prev + (cur || 0), 0);
                            console.log(sum, number, sum * number);
                            return;
                        } else {
                            boards.splice(boardIndex, 1);
                        }
                    }
                }
            }
        }
    }
}

processLineByLine();