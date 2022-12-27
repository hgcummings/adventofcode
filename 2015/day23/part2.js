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
    
    let instructions = [];
    for await (const line of rl) {
        instructions.push(line);
    }

    console.log(instructions.length);

    const mem = {
        a: 1,
        b: 0
    };

    let i = 0;
    while (i >= 0 && i < instructions.length) {
        const instruction = instructions[i];
        console.log(`${i}: ${instruction} (${JSON.stringify(mem)})`);

        if (instruction.startsWith("hlf")) {
            mem[instruction[4]] /= 2;
        } else if (instruction.startsWith("tpl")) {
            mem[instruction[4]] *= 3;
        } else if (instruction.startsWith("inc")) {
            mem[instruction[4]] += 1;
        } else if (instruction.startsWith("jmp")) {
            i += parseInt(instruction.split(" ")[1], 10);
            continue;
        } else if ((instruction.startsWith("jie") && mem[instruction[4]] % 2 === 0) ||
                   (instruction.startsWith("jio") && mem[instruction[4]] === 1)) {
            i += parseInt(instruction.split(", ")[1], 10);
            continue;
        }

        i += 1;
    }

    console.log(mem);

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();