const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');

const startTime = performance.now();
async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let state = [];
    for await (const line of rl) {
        state.push(line.split(""));
    }
    
    const h = state.length;
    const w = state[0].length;

    function createNewState() {
        let newState = [];
        for (let y = 0; y < h; ++y) {
            newState[y] = [];
            for (let x = 0; x < w; ++x) {
                newState[y][x] = ".";
            }
        }
        return newState;
    }

    let moved = true;
    for (let step = 1; moved; ++step) {
        console.log(step);

        moved = false;
        let newState = createNewState();

        // East
        for (let y = 0; y < h; ++y) {
            for (let x = 0; x < w; ++x) {
                switch (state[y][x]) {
                    case ">":
                        const target = (x + 1) % w;
                        if (state[y][target] === ".") {
                            newState[y][target] = ">";
                            moved = true;
                        } else {
                            newState[y][x] = ">";
                        }
                        break;
                    case "v":
                        newState[y][x] = "v";
                        break;
                    default:
                        break;
                }
            }
        }
        state = newState;
        newState = createNewState();

        // South
        for (let y = 0; y < h; ++y) {
            for (let x = 0; x < w; ++x) {
                switch (state[y][x]) {
                    case ">":
                        newState[y][x] = ">";
                        break;
                    case "v":
                        const target = (y + 1) % h;
                        if (state[target][x] === ".") {
                            newState[target][x] = "v";
                            moved = true;
                        } else {
                            newState[y][x] = "v";
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        state = newState;
    }

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();