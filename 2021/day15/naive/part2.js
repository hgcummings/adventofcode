const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let risk = [];
    let cost = [];
    for await (const line of rl) {
        risk.push(line.split("").map(r => parseInt(r, 10)));
    }

    if (risk.length !== risk[0].length){
        console.log("Grid is not square");
        process.exit(1);
    }

    const baseSize = risk.length;
    const fullSize = baseSize * 5;

    for (let y = 0; y < fullSize; ++y){
        cost[y] = [];
        for (let x = 0; x < fullSize; ++x) {
            cost[y][x] = Infinity;
        }
    }

    cost[0][0] = 0;
    
    function r(x, y) {
        const base = risk[y % baseSize][x % baseSize];
        let incr = base + Math.floor(y / baseSize) + Math.floor(x / baseSize);
        return incr > 9 ? (incr - 9) : incr;
    }

    for (let d = 1; d < fullSize * 2; ++d) {
        for (let y = Math.max(0, d - fullSize + 1); y <= Math.min(d, fullSize - 1); ++y) {
            const x = d - y;
            const prev = [];
            if (y > 0) {
                prev.push(cost[y-1][x]);
            }
            if (x > 0) {
                prev.push(cost[y][x-1]);
            }
            cost[y][x] = _.min(prev) + r(x, y);
        }
    }
    
    console.log(cost[fullSize - 1][fullSize - 1]);
}

processLineByLine();