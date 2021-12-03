const fs = require('fs');
const readline = require('readline');

async function processLineByLine() {
    const fileStream = fs.createReadStream('input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let lines = [];
    for await (const line of rl) {
        lines.push(line);
    }

    let oxygen = lines.concat();
    for (let i = 0; oxygen.length > 1; ++i) {
        let count = 0;
        const threshold = oxygen.length / 2;
        for (const value of oxygen) {
            if (value[i] === '1') {
                count += 1;
            }
        }
        oxygen = oxygen.filter(x => count >= threshold ? x[i] == '1' : x[i] == '0');
    }

    let co2 = lines.concat();
    for (let i = 0; co2.length > 1; ++i) {
        let count = 0;
        const threshold = co2.length / 2;
        for (const value of co2) {
            if (value[i] === '1') {
                count += 1;
            }
        }

        co2 = co2.filter(x => count < threshold ? x[i] == '1' : x[i] == '0');
    }

    oxygen = parseInt(oxygen[0], 2);
    co2 = parseInt(co2[0], 2);

    console.log(oxygen, co2, oxygen * co2);
  }
  
  processLineByLine();