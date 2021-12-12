const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let crabs;
    for await (const line of rl) {
        crabs = line.split(",");
        break;
    }
    
    let min = _.min(crabs);
    let max = _.max(crabs);
    
    let positions = _.range(min, max);
    let costs = positions.map(p => _.sumBy(crabs, crab => Math.abs(crab - p)));
    
    console.log(_.min(costs));
}

processLineByLine();