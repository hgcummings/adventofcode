const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let floor = [];
    
    for (let y = 0; y < 1000; ++y) {
        floor[y] = [];
        for (let x = 0; x < 1000; ++x) {
            floor[y][x] = 0;
        }
    }
    
    for await (const line of rl) {
        const [start, end] = line.split(" -> ");
        let [x1, y1] = start.split(",").map(c => parseInt(c, 10));
        let [x2, y2] = end.split(",").map(c => parseInt(c, 10));
        
        if (x1 === x2) {
            if (y1 > y2) {
                [y1, y2] = [y2, y1];
            }
            for (let y = y1; y <= y2; ++y) {
                floor[y][x1] += 1;
            }
        } else if (y1 === y2) {
            if (x1 > x2) {
                [x1, x2] = [x2, x1];
            }
            for (let x = x1; x <= x2; ++x) {
                floor[y1][x] += 1;
            }
        }
    }
    
    let count = 0;
    
    for (let y = 0; y < 1000; ++y) {
        for (let x = 0; x < 1000; ++x) {
            if (floor[y][x] >= 2){
                count += 1;
            }
        }
    }
    
    console.log(count);
}

processLineByLine();