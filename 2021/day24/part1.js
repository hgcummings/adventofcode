const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');
const ArrayKeyedMap = require('array-keyed-map')

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

let states = new ArrayKeyedMap();
states.set([0,0,0],0);

const test = [9,9,9,4,1,9,1,1,1,9,1,1,4,9];

for (let i = 0; i < constants.length; ++i) {
    const newStates = new ArrayKeyedMap();
    let duplicates = 0;
    for (const [state, val] of states.entries()) {
        // for (let d = test[i]; d === test[i]; --d) {
        for (let d = 9; d > 0; --d) {
            let [x, y, z] = state;
            let w = d;
            // console.log("--------");
            // console.log("a", w, x, y, z);
            x = z;
            if (x < 0) {
                continue;
            }
            x = x % 26;
            z = Math.trunc(z / constants[i][0]);
            // console.log("b", w, x, y, z);
            x = x + constants[i][1];
            x = x === w ? 0 : 1;
            y = (25 * x) + 1;
            z = z * y;
            // console.log("c", w, x, y, z);
            y = (w + constants[i][2]) * x;
            z = z + y;
            // console.log("d", w, x, y, z);
            if (i < 13 || z === 0) {
                if (newStates.has([x,y,z])) {
                    duplicates += 1;
                }
                if (!newStates.has([x,y,z])) {
                    newStates.set([x,y,z], val + (d * (Math.pow(10, 13 - i))));
                }
            }
        }
    }
    states = newStates;
    console.log("------");
    console.log(duplicates);
    console.log(states.size);
}

let maxVal = -Infinity;
for (const val of states.values()) {
    if (val > maxVal) {
        maxVal = val;
    }
}
console.log(maxVal);
console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);