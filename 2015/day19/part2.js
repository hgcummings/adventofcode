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

    let molecules = [molecule];
    let step = 0;

    while (molecules.indexOf("e") === -1) {
        
        const newMolecules = [];
        for (const molecule of molecules) {
            for (const replacement of replacements) {
                let index = molecule.indexOf(replacement.to);
                while (index !== -1) {
                    newMolecule = `${molecule.substring(0,index)}${replacement.from}${molecule.substring(index + replacement.to.length)}`;
                    newMolecules.push(newMolecule);
                    index = molecule.indexOf(replacement.to, index + 1)
                }
            }
        }
        
        molecules = [newMolecules.sort((a, b) => a.length - b.length)[0]];
        console.log(++step, molecules[0]);
    }

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();