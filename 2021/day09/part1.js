const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let rows = [];
    for await (const line of rl) {
        rows.push(line.split("").map(h => parseInt(h, 10)));
    }


    const height = rows.length;
    const width = rows[0].length;

    let risk = 0;
    for (let y = 0; y < height; ++y) {
      for (let x = 0; x < width; ++x) {
        const val = rows[y][x];
        if (x > 0 && rows[y][x - 1] <= val) {
          continue;
        } else if (x < width-1 && rows[y][x + 1] <= val) {
          continue;
        } else if (y > 0 && rows[y - 1][x] <= val)  {
          continue;
        } else if (y < height-1 && rows[y + 1][x] <= val) {
          continue;
        } else {
          risk += (1 + val);
        }
      }
    }

    console.log(risk);
  }
  
  processLineByLine();