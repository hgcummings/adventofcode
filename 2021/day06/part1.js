const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let fish;
    for await (const line of rl) {
        fish = line.split(",");
        break;
    }

    for (let day = 0; day < days; ++day) {
      const count = fish.length;
      for (let i = 0; i < count; ++i) {
        if (fish[i] === 0) {
          fish[i] = 6;
          fish.push(8);
        } else {
          fish[i] -= 1;
        }
      }
    }

    console.log(fish.length);
  }
  
  processLineByLine();