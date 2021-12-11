const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let octs = [];
    for await (const line of rl) {
        octs.push(line.split("").map(o => parseInt(o, 10)));
    }

    const height = octs.length;
    const width = octs[0].length;

    let count = 0;
    for (let step = 0; step < 100; ++step) {
      for (let y = 0; y < height; ++y) {
        for (let x = 0; x < width; ++x) {
          octs[y][x] += 1;
        }
      }

      let flashed;
      do {
        const flashes = [];
        flashed = false;

        for (let y = 0; y < height; ++y) {
          for (let x = 0; x < width; ++x) {
            if (octs[y][x] > 9) {
              flashed = true;
              count += 1;
              flashes.push({x, y});
              octs[y][x] = -Infinity;
            }
          }
        }

        for (const flash of flashes) {
          const {x, y} = flash;
          if (x > 0) {
            octs[y][x - 1] += 1;
          }
          if (x < width - 1) {
            octs[y][x + 1] += 1;
          }
          if (y > 0) {
            octs[y - 1][x] += 1;
          }
          if (y < height - 1) {
            octs[y + 1][x] += 1;
          }
          if (x > 0 && y > 0) {
            octs[y - 1][x - 1] += 1;
          }
          if (x > 0 && y < height - 1) {
            octs[y + 1][x - 1] += 1;
          }
          if (x < width - 1 && y > 0) {
            octs[y - 1][x + 1] += 1;
          }
          if (x < width - 1 && y < height - 1) {
            octs[y + 1][x + 1] += 1;
          }
        }
      } while(flashed)

      for (let y = 0; y < height; ++y) {
        for (let x = 0; x < width; ++x) {
          if (octs[y][x] === -Infinity) {
            octs[y][x] = 0;
          }
        }
      }

      // console.log(step + 1);
      // for (let y = 0; y < height; ++y) {
      //   console.log(octs[y].join(""));
      // }
    }

    console.log(count);
  }
  
  processLineByLine();