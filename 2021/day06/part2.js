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

    const pops = [0,0,0,0,0,0,0,0,0];
    for (let initialFish of fish) {
      pops[initialFish] += 1;
    }

    const days = 256;
    for (let day = 0; day < days; ++day) {
      const newFish = pops[0];
      for (let age = 1; age < 9; ++age) {
        pops[age - 1] = pops[age];
      }
      pops[8] = newFish;
      pops[6] += newFish;
    }

    console.log(_.sum(pops));
  }
  
  processLineByLine();