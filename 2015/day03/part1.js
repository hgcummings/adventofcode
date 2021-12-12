const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let instructions;
    for await (const line of rl) {
        instructions = line.split("");
        break;
    }
    
    let houses = [];
    let pos = [instructions.length, instructions.length];
    
    houses[pos[0]] = [];
    houses[pos[0]][pos[1]] = 1;
    
    for (let inst of instructions) {
        switch (inst) {
            case ">":
                pos[0] += 1;
                break;
            case "<":
                pos[0] -= 1;
                break;
            case "v":
                pos[1] += 1;
                break;
            case "^":
                pos[1] -= 1;
                break;
            default:
                break;
        }
        
        houses[pos[0]] = houses[pos[0]] || [];
        houses[pos[0]][pos[1]] = (houses[pos[0]][pos[1]] || 0) + 1;
    }
    
    let count = 0;
    for (let row of houses) {
        if (row) {
            for (let house of row) { 
                if (house) {
                    count += 1;
                }
            }
        }
    }
    
    console.log(count);
}

processLineByLine();