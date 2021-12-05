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
    const size = 1000;

    for (let y = 0; y < size; ++y) {
        floor[y] = [];
        for (let x = 0; x < size; ++x) {
            floor[y][x] = 0;
        }
    }
  
    for await (const line of rl) {
        const [start, end] = line.split(" -> ");
        let [x1, y1] = start.split(",").map(c => parseInt(c, 10));
        let [x2, y2] = end.split(",").map(c => parseInt(c, 10));

        const xd = Math.sign(x2 - x1);
        const yd = Math.sign(y2 - y1);

        let y = y1;
        let x = x1;
        
        floor[y][x] += 1;

        do {
            x += xd;
            y += yd;
            floor[y][x] += 1;
        } while (x != x2 || y != y2);
    }

    if (size < 100) {
        for (let y = 0; y < floor[0].length; ++y) {
            console.log(floor[y].join());
        }
    }

    let count = 0;

    for (let y = 0; y < size; ++y) {
        for (let x = 0; x < size; ++x) {
            if (floor[y][x] >= 2){
                count += 1;
            }
        }
    }

    console.log(count);
  }
  
  processLineByLine();