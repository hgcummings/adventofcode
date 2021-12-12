const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let lights = [];

    for (let y = 0; y < 1000; ++y) {
        lights[y] = [];
        for (let x = 0; x < 1000; ++x) {
            lights[y][x] = 0;
        }
    }

    const expr = /(?<cmd>[a-z\s]+)\s(?<x1>[0-9]+),(?<y1>[0-9]+) through (?<x2>[0-9]+),(?<y2>[0-9]+)/;
    for await (const line of rl) {
        const {cmd, x1, x2, y1, y2} = expr.exec(line).groups;
        for (let y = parseInt(y1, 10); y <= parseInt(y2, 10); ++y) {
            for (let x = parseInt(x1, 10); x <= parseInt(x2, 10); ++x) {
                switch (cmd) {
                    case "toggle":
                        lights[y][x] += 2;
                        break;
                    case "turn on":
                        lights[y][x] += 1;
                        break;
                    case "turn off":
                        lights[y][x] = Math.max(0, lights[y][x] - 1);
                        break;
                    default:
                        console.log("Unrecognised command ", cmd);
                        break;
                }
            }
        }
    }
    
    let total = 0;
    for (let y = 0; y < 1000; ++y) {
        for (let x = 0; x < 1000; ++x) {
            if (lights[y][x]) {
                total += lights[y][x];
            }
        }
    }

    console.log(total);
}

processLineByLine();