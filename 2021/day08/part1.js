const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let counts = [0,0,0,0,0,0,0,0,0,0];
    for await (const line of rl) {
        const [allIns, allOuts] = line.split(" | ");
        const outs = allOuts.split(" ");
        for (let out of outs) {
            switch (out.length) {
                case 2:
                    counts[1] += 1;
                    break;
                case 4:
                    counts[4] += 1;
                    break;
                case 3:
                    counts[7] += 1;
                    break;
                case 7:
                    counts[8] += 1;
                    break;
                default:
                    break;
            }
        }
    }
    
    console.log(_.sum(counts));
}

processLineByLine();