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

    let constants = [
        [1,14,12],
        [1,15,7],
        [1,12,1],
        [1,11,2],
        [26,-5,4],
        [1,14,15],
        [1,15,11],
        [26,-13,5],
        [26,-16,3],
        [26,-8,9],
        [1,15,2],
        [26,-8,3],
        [26,0,3],
        [26,-4,11]
    ]

    for (const c of constants) {
        console.log(`    
    w = Math.floor(input / ${Math.pow(10, --i)}) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / ${c[0]});
    x = x + ${c[1]};
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + ${c[2]}) * x;
    z = z + y;
    `)
    }

    console.log(`    return (z === 0);`);
    console.log("}");
}

processLineByLine();