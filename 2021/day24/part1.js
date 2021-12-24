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

    for (let num = 99999999999999; ; --num) {
        if (num % 10000000 === 0) {
            console.log(num);
            console.log(`(${Math.round(performance.now() - startTime) / 1000}s elapsed)`);
        }
        if (isValid(num)) {
            console.log(num);
            break;
        }
    }

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();

function isValid(input) {
    let w = 0, x = 0, y = 0, z = 0;
    
    w = Math.floor(input / 10000000000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 1);
    x = x + 14;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 12) * x;
    z = z + y;
    
    
    w = Math.floor(input / 1000000000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 1);
    x = x + 15;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 7) * x;
    z = z + y;
    
    
    w = Math.floor(input / 100000000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 1);
    x = x + 12;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 1) * x;
    z = z + y;
    
    
    w = Math.floor(input / 10000000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 1);
    x = x + 11;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 2) * x;
    z = z + y;
    
    
    w = Math.floor(input / 1000000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 26);
    x = x + -5;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 4) * x;
    z = z + y;
    
    
    w = Math.floor(input / 100000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 1);
    x = x + 14;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 15) * x;
    z = z + y;
    
    
    w = Math.floor(input / 10000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 1);
    x = x + 15;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 11) * x;
    z = z + y;
    
    
    w = Math.floor(input / 1000000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 26);
    x = x + -13;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 5) * x;
    z = z + y;
    
    
    w = Math.floor(input / 100000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 26);
    x = x + -16;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 3) * x;
    z = z + y;
    
    
    w = Math.floor(input / 10000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 26);
    x = x + -8;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 9) * x;
    z = z + y;
    
    
    w = Math.floor(input / 1000) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 1);
    x = x + 15;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 2) * x;
    z = z + y;
    
    
    w = Math.floor(input / 100) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 26);
    x = x + -8;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 3) * x;
    z = z + y;
    
    
    w = Math.floor(input / 10) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 26);
    x = x + 0;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 3) * x;
    z = z + y;
    
    
    w = Math.floor(input / 1) % 10;
    if (w === 0) { return false; }
    x = z;
    if (x < 0) { return false; }
    x = x % 26;
    z = Math.floor(z / 26);
    x = x + -4;
    x = x === w ? 0 : 1;
    y = (25 * x) + 1;
    z = z * y;
    y = (w + 11) * x;
    z = z + y;
    
    return (z === 0);
}
