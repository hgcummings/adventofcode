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
        if (num % 1000000 === 0) {
            console.log(num);
        }
        const input = num.toString(10).split("").map(d => parseInt(d, 10));
        if (input.some(sf => sf === 0)) {
            continue;
        }
        if (isValid(input)) {
            console.log(input);
            break;
        }
    }

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();

function isValid(input) {
    let w = 0, x = 0, y = 0, z = 0;
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (1 === 0) { return false; }
    z = z / 1
    x = x + 14
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 12
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (1 === 0) { return false; }
    z = z / 1
    x = x + 15
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 7
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (1 === 0) { return false; }
    z = z / 1
    x = x + 12
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 1
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (1 === 0) { return false; }
    z = z / 1
    x = x + 11
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 2
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (26 === 0) { return false; }
    z = z / 26
    x = x + -5
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 4
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (1 === 0) { return false; }
    z = z / 1
    x = x + 14
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 15
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (1 === 0) { return false; }
    z = z / 1
    x = x + 15
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 11
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (26 === 0) { return false; }
    z = z / 26
    x = x + -13
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 5
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (26 === 0) { return false; }
    z = z / 26
    x = x + -16
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 3
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (26 === 0) { return false; }
    z = z / 26
    x = x + -8
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 9
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (1 === 0) { return false; }
    z = z / 1
    x = x + 15
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 2
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (26 === 0) { return false; }
    z = z / 26
    x = x + -8
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 3
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (26 === 0) { return false; }
    z = z / 26
    x = x + 0
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 3
    y = y * x
    z = z + y
    w = input.shift();
    x = x * 0
    x = x + z
    if (x < 0 || 26 <= 0) { return false; }
    x = x % 26
    if (26 === 0) { return false; }
    z = z / 26
    x = x + -4
    x = x === w ? 1 : 0;
    x = x === 0 ? 1 : 0;
    y = y * 0
    y = y + 25
    y = y * x
    y = y + 1
    z = z * y
    y = y * 0
    y = y + w
    y = y + 11
    y = y * x
    z = z + y
    return (z === 0);
}
