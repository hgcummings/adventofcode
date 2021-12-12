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
    
    let scores = [];
    for await (const line of rl) {
        const unclosed = [];
        let invalid = false;
        for (let char of line) {
            if (c.includes(char)) {
                if (unclosed.length && c.indexOf(char) === o.indexOf(unclosed[unclosed.length - 1])) {
                    unclosed.pop();
                } else if (unclosed.length) {
                    invalid = true;
                    break;
                }
            } else if (o.includes(char)) {
                unclosed.push(char);
            }
        }
        if (invalid) {
            continue;
        }
        let score = 0;
        while (unclosed.length) {
            char = unclosed.pop();
            score *= 5;
            score += (1 + o.indexOf(char));
        }
        if (score > 0) {
            scores.push(score);
        }
    }
    
    scores.sort((a, b) => a - b);
    console.log(scores[Math.floor(scores.length / 2)]);
}

processLineByLine();