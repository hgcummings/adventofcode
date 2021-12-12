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
        total += (line.length - eval(line).length);
    }
    
    console.log(total);
}

processLineByLine();