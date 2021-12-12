const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let digits;
    for await (const line of rl) {
        digits = line;
    }

    function lookAndSay(input) {
        let count = 1;
        let current = input[0];
        let output = "";
        for (let i = 1; i < input.length; ++i) {
            if (input[i] === current) {
                count += 1;
            } else {
                output += count;
                output += current;
                count = 1;
                current = input[i];
            }
        }
        
        output += count;
        output += current;
        return output;
    }
    
    for (let i = 0; i < 50; ++i) {
        digits = lookAndSay(digits);
    }

    console.log(digits.length);
}

processLineByLine();