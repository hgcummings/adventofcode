const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');
const {combinationsWithReplacement} = require('combinatorial-generators');

const startTime = performance.now();
async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    const ingredients = {};
    const pattern = /(?<name>[A-Za-z]+): capacity (?<cap>\-?[0-9]+), durability (?<dur>\-?[0-9]+), flavor (?<fla>\-?[0-9]+), texture (?<tex>\-?[0-9]+), calories (?<cal>\-?[0-9]+)/;
    for await (const line of rl) {
        const {name, cap, dur, fla, tex, cal} = pattern.exec(line).groups;
        ingredients[name] = {cap, dur, fla, tex, cal};
    }

    let bestScore = -Infinity;
    for (const mix of combinationsWithReplacement(Object.keys(ingredients), 100)) {
        let counts = _.countBy(mix, elem => elem);
        const totals = {cap:0, dur:0, fla:0, tex:0, cal:0};
        for (const ingredient in counts) {
            for (const key in totals) {
                totals[key] += (counts[ingredient] * ingredients[ingredient][key]);
            }
        }
        if (totals.cal !== 500) {
            continue;
        }

        const score = Math.max(0, totals.cap) * Math.max(0, totals.dur) * 
                        Math.max(0, totals.fla) * Math.max(0, totals.tex);
        if (score > bestScore) {
            bestScore = score;
        }
    }

    console.log(bestScore);
    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();