const {performance} = require('perf_hooks');

const startTime = performance.now();

const first = 20151125;
const multiplier = 252533;
const divisor = 33554393;

let current = first;
let cycleLength = 0;
do {
    current = (current * multiplier) % divisor;
    cycleLength++;
} while (current != first);

const [row, col] = [2981, 3075];
const diagonal = row + col - 1;

const index = ((((diagonal - 1) / 2) * diagonal) + col) % cycleLength;

current = first;
for (let j = 1; j < index; ++j) {
    current = (current * multiplier) % divisor;
}

console.log(current);

console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);