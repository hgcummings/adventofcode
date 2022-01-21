const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');

const startTime = performance.now();
async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let replacements = [];
    let molecule;
    for await (const line of rl) {
        if (!line.length) {
            continue;
        }
        const parts = line.split(" => ");
        if (parts.length === 1) {
            molecule = parts[0];
        } else {
            replacements.push({
                from: parts[0],
                to: parts[1]
            })
        }
    }

    let outputs = [];
    for (const replacement of replacements) {
        let index = molecule.indexOf(replacement.from);
        while (index !== -1) {
            outputs.push(`${molecule.substring(0,index)}${replacement.to}${molecule.substring(index + replacement.from.length)}`);
            index = molecule.indexOf(replacement.from, index + 1)
        }
    }

    console.log(outputs.length);
    outputs = _.uniq(outputs);
    console.log(outputs.length);

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();