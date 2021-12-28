const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');
const sues = require('./input.json');

const startTime = performance.now();

const constraints = {
    "children": 3,
    "cats": 7,
    "samoyeds": 2,
    "pomeranians": 3,
    "akitas": 0,
    "vizslas": 0,
    "goldfish": 5,
    "trees": 3,
    "cars": 2,
    "perfumes": 1
}

for (const id in sues) {
    const sue = sues[id];
    let match = true;
    for (const key in sue) {
        switch (key) {
            case "cats":
            case "trees":
                if (sue[key] <= constraints[key]) {
                    match = false;
                }
                break;
            case "pomeranians":
            case "goldfish":
                if (sue[key] >= constraints[key]) {
                    match = false;
                }
                break;
            default:
                if (sue[key] != constraints[key]) {
                    match = false;
                }
                break;
        }
        if (!match) {
            break;
        }
    }
    if (match) {
        console.log(id);
    }
}

console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
