const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    function isNice(str) {
        const pairs = [];
        
        let foundDoublePair = false;
        let foundRepeat2 = false;
        
        let prev = "";
        let prev2 = "";
        for (const char of str) {
            if (char === prev2 && char !== prev) {
                foundRepeat2 = true;
            }
            
            if (!foundDoublePair) {
                const pair = prev + char;
                const idx = pairs.indexOf(pair);
                
                if (idx !== -1 && idx != pairs.length - 1) {
                    foundDoublePair = true;
                } else {
                    pairs.push(pair);
                }
            }
            prev2 = prev;
            prev = char;
        }
        
        return foundRepeat2 && foundDoublePair;
    }
    
    let count = 0;
    for await (const line of rl) {
        if (isNice(line)) {
            count += 1;
        }
    }
    
    console.log(count);
}

processLineByLine();