const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');

const startTime = performance.now();
async function processLineByLine() {
    let presents = 0;
    let number;
    for (number = 500000; presents < 34000000; ++number) {
        let max = number;
        presents = 0;
        for (let i = 1; i < max; ++i) {
            if (number % i === 0) {
                max = number / i;
                if (number <= 50 * i) {
                    presents += 11 * i;
                }
                if (max !== i && number <= 50 * max) {
                    presents += 11 * max;
                }
            }
        }
        if (number % 10000 === 0) {
            console.log(number, presents);
        }
    }
    console.log(number - 1);

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();