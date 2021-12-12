const fs = require('fs');
const readline = require('readline');

async function processLineByLine() {
    const fileStream = fs.createReadStream('input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let increases = 0;
    let last = null;
    for await (const line of rl) {
        const current = parseInt(line, 10);
        if (last !== null && current > last) {
            increases += 1;
        }
        last = current;
    }
    
    console.log(increases);
}

processLineByLine();