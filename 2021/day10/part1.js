const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    const o = ["(","[","{","<"];
    const c = [")","]","}",">"];
    const p = [3,57,1197,25137];
    
    let points = 0;
    for await (const line of rl) {
        const unclosed = [];
        for (let char of line) {
            if (c.includes(char)) {
                if (unclosed.length && c.indexOf(char) === o.indexOf(unclosed[unclosed.length - 1])) {
                    unclosed.pop();
                } else {
                    points += p[c.indexOf(char)];
                    break;
                }
            } else if (o.includes(char)) {
                unclosed.push(char);
            }
        }
    }
    
    console.log(points);
}

processLineByLine();