const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let total = 0;
    for await (const line of rl) {
        const dims = line.split("x").map(d => parseInt(d, 10)).sort((a, b) => a - b);
        total += (3 * dims[0] * dims[1]) + (2 * dims[0] * dims[2]) + (2 * dims[1] * dims[2]);
    }
    
    console.log(total);
}

processLineByLine();