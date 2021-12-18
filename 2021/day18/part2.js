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
    
    let nums = [];
    for await (const line of rl) {
        nums.push(JSON.parse(line));
    }

    function addRightMost(num, x) {
        if (Array.isArray(num)) {
            return [num[0], addRightMost(num[1], x)];
        } else {
            return num + x;
        }
    }
    
    function addLeftMost(num, x) {
        if (Array.isArray(num)) {
            return [addLeftMost(num[0], x), num[1]];
        } else {
            return num + x;
        }
    }

    function explodeIfNeeded(num, depth) {
        if (!Array.isArray(num)) {
            return [num, false];
        } else if (depth === 0) {
            return [0, true, [num[0], num[1]]];
        } else {
            for (let i = 0; i < 2; ++i) {
                const elem = num[i];
                let [newElem, exploded, remainder] = explodeIfNeeded(elem, depth - 1);
                if (exploded) {
                    let newNum = [];
                    newNum[i] = newElem;
                    newNum[1-i] = num[1-i];
                    if (remainder[0]) {
                        if (i > 0) {
                            newNum[i - 1] = addRightMost(num[i-1], remainder[0]);
                            remainder[0] = undefined;
                        }
                    }
                    if (remainder[1]) {
                        if (i < 1) {
                            newNum[i + 1] = addLeftMost(num[i + 1], remainder[1]);
                            remainder[1] = undefined;
                        }
                    }
                    return [newNum, exploded, remainder];
                }
            }
            return [num, false];
        }
    }

    function splitIfNeeded(num) {
        if (Array.isArray(num)) {
            for (let i = 0; i < 2; ++i) {
                const elem = num[i];
                let [newElem, split] = splitIfNeeded(elem);
                if (split) {
                    let newNum = [];
                    newNum[i] = newElem;
                    newNum[1-i] = num[1-i];
                    return [newNum, split];
                }
            }
            return [num, false];
        } else if (num > 9) {
            return [[Math.floor(num / 2), Math.ceil(num / 2)], true];
        } else {
            return [num, false];
        }
    }

    function reduce(num) {
        let exploded = false, split = false;
        do {
            exploded = false, split = false;
            
            [num, exploded] = explodeIfNeeded(num, 4);
            if (!exploded) {
                [num, split] = splitIfNeeded(num);
            }
        } while (exploded || split)
        return num;
    }
    
    function magnitude(num) {
        const magLeft = Array.isArray(num[0]) ? magnitude(num[0]) : num[0];        
        const magRight = Array.isArray(num[1]) ? magnitude(num[1]) : num[1];
        return 3 * magLeft + 2 * magRight;
    }

    let max = -Infinity;
    for (const pair of permutations(nums, 2)) {
        const result = magnitude(reduce(pair));

        if (result > max) {
            max = result;
        }
    }

    console.log(max);
}

processLineByLine();