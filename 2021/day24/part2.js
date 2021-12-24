const _ = require('lodash');
const {performance} = require('perf_hooks');

const startTime = performance.now();

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

let states = new Map();
states.set(0,0);

for (let i = 0; i < constants.length; ++i) {
    const newStates = new Map();
    for (const [state, val] of states.entries()) {
        for (let d = 9; d > 0; --d) {
            let x, y;
            let z = state;
            let w = d;
            x = z;
            if (x < 0) {
                continue;
            }
            x = x % 26;
            z = ~~(z / constants[i][0]);
            x = x + constants[i][1];
            x = x === w ? 0 : 1;
            y = (25 * x) + 1;
            z = z * y;
            y = (w + constants[i][2]) * x;
            z = z + y;
            if (i < 13 || z === 0) {
                const newVal = val + (d * (Math.pow(10, 13 - i)));
                if (!(newStates.has(z) && newStates.get(z) <= newVal)) {
                    newStates.set(z, newVal);
                }
            }
        }
    }
    states = newStates;
    console.log(states.size);
}

let minVal = Infinity;
for (const val of states.values()) {
    if (val < minVal) {
        minVal = val;
    }
}
console.log(minVal);
console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);