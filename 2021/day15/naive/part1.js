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
        cost.push(line.split("").map(_ => Infinity));
    }

    if (risk.length !== risk[0].length){
        console.log("Grid is not square");
        process.exit(1);
    }

    cost[0][0] = 0;

    for (let d = 1; d < risk.length * 2; ++d) {
        for (let y = Math.max(0, d - risk.length + 1); y <= Math.min(d, risk.length - 1); ++y) {
            const x = d - y;
            const prev = [];
            if (y > 0) {
                prev.push(cost[y-1][x]);
            }
            if (x > 0) {
                prev.push(cost[y][x-1]);
            }
            cost[y][x] = _.min(prev) + risk[y][x];
        }
    }
    
    console.log(cost[risk.length - 1][risk.length - 1]);
}

processLineByLine();