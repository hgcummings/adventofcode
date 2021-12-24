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

    const expr = /(?<op>[a-z]+) (?<a>\-?[0-9w-z]+) ?(?<b>\-?[0-9w-z]+)?/

    let instructions = [];
    for await (const line of rl) {
        const {op, a, b} = expr.exec(line).groups;
        instructions.push({
            op,
            a: isFinite(parseInt(a, 10)) ? parseInt(a, 10) : a,
            b: isFinite(parseInt(b, 10)) ? parseInt(b, 10) : b
        });
    }

    console.log("function isValid(input) {")
    console.log("    let w = 0, x = 0, y = 0, z = 0;")
    let i = 14;
    for (const instr of instructions) {
        switch (instr.op) {
            case "inp":
                console.log(`    ${instr.a} = Math.floor(input / ${Math.pow(10, --i)}) % 10;`);
                console.log(`    if (${instr.a} === 0) { return false; }`);
                break;
            case "add":
                console.log(`    ${instr.a} = ${instr.a} + ${instr.b};`);
                break;
            case "mul":
                console.log(`    ${instr.a} = ${instr.a} * ${instr.b};`);
                break;
            case "div":
                console.log(`    if (${instr.b} === 0) { return false; }`);
                console.log(`    ${instr.a} = ${instr.a} / ${instr.b};`);
                break;
            case "mod":
                console.log(`    if (${instr.a} < 0 || ${instr.b} <= 0) { return false; }`);
                console.log(`    ${instr.a} = ${instr.a} % ${instr.b};`);
                break;
            case "eql":
                console.log(`    ${instr.a} = ${instr.a} === ${instr.b} ? 1 : 0;`);
                break;
        }
    }
    console.log(`    return (z === 0);`);
    console.log("}");
}

processLineByLine();