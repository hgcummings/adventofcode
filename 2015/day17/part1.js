const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');

const startTime = performance.now();

function findSolutions(target, containers, base) {
    let solutions = [];
    for (let i = 0; i < containers.length; ++i) {
        let container = containers[i];
        if (container === target) {
            solutions.push(base.concat(container));
        } else if (container < target) {
            solutions = solutions.concat(findSolutions(
                target - container,
                containers.slice(i + 1, containers.length),
                base.concat(container)
            ));
        }
    }
    return solutions;
}

async function processLineByLine() {
    const fileName = process.argv[2] || 'input.txt';
    const fileStream = fs.createReadStream(fileName);
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let target = parseInt(process.argv[3] || "150");
    let containers = [];
    for await (const line of rl) {
        containers.push(parseInt(line, 10));
    }

    solutions = findSolutions(target, containers, []);

    console.log(solutions.length);

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();