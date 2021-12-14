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

    function addCount(counts, key, value) {
        if (counts[key]) {
            counts[key] += value;
        } else {
            counts[key] = value;
        }
    }

    let pairs = {};
    for (let i = 0; i < polymer.length - 1; ++i) {
        const pair = polymer[i] + polymer[i+1];
        addCount(pairs, pair, 1);
    }

    for (let i = 0; i < 40; ++i) {
        let newPairs = {};

        for (const pair in pairs) {
            let foundRule = false;    
            for (const rule of rules) {
                if (rule.from === (pair)) {
                    addCount(newPairs, pair[0] + rule.to, pairs[pair]);
                    addCount(newPairs, rule.to + pair[1], pairs[pair]);
                    foundRule = true;
                }
            }
            if (!foundRule) {
                addCount(newPairs, pair, pairs[pair]);
            }
        }
        pairs = newPairs;
    }
    
    const counts = {};
    for (const pair in pairs) {
        addCount(counts, pair[0], pairs[pair]);
        addCount(counts, pair[1], pairs[pair]);
    }

    console.log(counts);
    const sortedCounts = Object.values(counts).concat().map(c => Math.ceil(c / 2));
    sortedCounts.sort((a, b) => a - b);
    console.log(sortedCounts[sortedCounts.length - 1] - sortedCounts[0]);
}

processLineByLine();