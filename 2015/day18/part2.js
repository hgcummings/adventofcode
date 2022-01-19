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
    
    let lines = [];
    for await (const line of rl) {
        lines.push(line);
    }

    const height = lines.length;
    const width = lines[0].length;

    for(let step = 0; step < 100; ++step) {
        lines[0] = '#' + lines[0].substring(1, width - 1) + '#';
        lines[height-1] = '#' + lines[height-1].substring(1, width - 1) + '#';

        const newLines = [];
        for (let y = 0; y < height; ++y) {
            let newLine = "";
            for (let x = 0; x < width; ++x) {
                let neighbours = 0;
                for (let ny = Math.max(y-1, 0); ny < Math.min(y + 2, height); ++ny) {
                    for (let nx = Math.max(x-1, 0); nx < Math.min(x + 2, width); ++nx) {
                        if ((nx !== x || ny !== y) && lines[ny][nx] === "#") {
                            neighbours += 1;
                        }
                    }
                }
                if (lines[y][x] === "#") {
                    newLine += (neighbours === 2 || neighbours === 3) ? "#" : ".";
                } else {
                    newLine += (neighbours === 3) ? "#" : ".";
                }
            }
            
            newLines[y] = newLine;
        }
        lines = newLines;
    }
    lines[0] = '#' + lines[0].substring(1, width - 1) + '#';
    lines[height-1] = '#' + lines[height-1].substring(1, width - 1) + '#';

    const lightsOn = _.sumBy(lines, line => line.replace(/\./g, "").length);
    console.log(lightsOn);

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();