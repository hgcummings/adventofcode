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

    const constants = [
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
    ];

    function findValid(prevZ, c, prefix) {
        if (c < 6) {
            console.log(prefix);
            console.log(`(${Math.round(performance.now() - startTime) / 1000}s elapsed)`);
        }

        /*  The following code should be equivalent to...
            x = z;
            x = x % 26;
            z = Math.floor(z / ${c[0]});
            x = x + ${c[1]};
            x = x === w ? 0 : 1;
            y = (25 * x) + 1;
            z = z * y;
            y = (w + ${c[2]}) * x;
            z = z + y;
            if (z < 0) { return false; }
        */

        let initX = prevZ % 26;
        // z is positive due to the check before the recursive call,
        // and constants[c] is only ever positive (1 or 26), so we
        // can use ~~ as a more efficient Math.trunc here. 
        let initZ = ~~(prevZ/constants[c][0]);
        initX = initX + constants[c][1];

        for (let w = 9; w > 0; --w) {
            let x = initX === w ? 0 : 1;
            let y = (25 * x) + 1;
            let z = initZ * y;
            y = (w + constants[c][2]) * x;
            z = z + y;
            if (c === 13) {
                if (z === 0) {
                    return prefix + w;
                }
            } else {
                if (z < 0) {
                    continue;
                }
                let next = findValid(z, c + 1, prefix + (w * Math.pow(10, 13 - c)));
                if (next) {
                    return next;
                } else {
                    continue;
                }
            }
        }
        return false;
    }

    console.log(findValid(0,0,0));

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();