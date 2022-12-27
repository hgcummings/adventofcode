const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');
const { combinations } = require("combinatorial-generators");

const startTime = performance.now();
async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let weights = [];
    for await (const line of rl) {
        weights.push(parseInt(line, 10));
    }

    const targetWeight = _.sum(weights) / 3;

    for (let size = 1;; size++) {
        let bestQe = Infinity;
        for (let combination of combinations(weights, size)) {
            if (_.sum(combination) === targetWeight) {
                const qe = _.reduce(combination, _.multiply);
                if (qe < bestQe) {
                    bestQe = qe;
                    console.log(combination);
                }
            }
        }
        if (bestQe < Infinity) {
            console.log("Best QE: ", bestQe);
            break;
        }
    }

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();