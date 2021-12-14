const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let rules = [];
    let polymer;
    for await (const line of rl) {
        if (!polymer) {
            polymer = line;
        } else if (line !== "") {
            [from, to] = line.split(" -> ");
            rules.push({from, to});
        }
    }

    for (let i = 0; i < 40; ++i) {
        console.log(i);
        let newPolymer = "";
        let prev = "";
        for (const char of polymer) {
            let foundRule = false;    
            for (const rule of rules) {
                if (rule.from === (prev + char)) {
                    newPolymer += rule.to + char;
                    foundRule = true;
                }    
            }
            if (!foundRule) {
                newPolymer += char;
            }
            prev = char;
        }
        polymer = newPolymer;
    }
    
    const counts = {};
    for (const char of polymer) {
        if (!counts[char]) {
            counts[char] = 1;
        } else {
            counts[char] += 1;
        }
    }

    console.log(counts);
    const sortedCounts = Object.values(counts).concat();
    sortedCounts.sort((a, b) => a - b);
    console.log(sortedCounts[sortedCounts.length - 1] - sortedCounts[0]);
}

processLineByLine();