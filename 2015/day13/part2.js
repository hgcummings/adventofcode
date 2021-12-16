const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const { permutations } = require('combinatorial-generators');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    const people = [];
    const rels = {};

    const expr = /(?<a>[A-Za-z]+) would (?<dir>(gain)|(lose)) (?<mag>[0-9]+) happiness units by sitting next to (?<b>[A-Za-z]+)./;
    for await (const line of rl) {
        const {a, b, dir, mag} = expr.exec(line).groups;

        if (!people.includes(a)) {
            people.push(a);
            rels[a] = {};
        }

        rels[a][b] = mag * (dir === "gain" ? 1 : -1);
    }

    let maxScore = -Infinity;

    people.push("self");

    for (let p of permutations(people)) {
        let score = 0;
        for (let i = 0; i < p.length; ++i) {
            let left = p[i];
            let right = p[(i + 1) % p.length];
            if (left === "self" || right === "self") {
                continue;
            }
            score += rels[left][right] + rels[right][left];
        }
        if (score > maxScore) {
            maxScore = score;
        }
    }

    console.log(maxScore);
}

processLineByLine();