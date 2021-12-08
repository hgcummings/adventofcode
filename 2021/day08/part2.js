const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let sum = 0;
    for await (const line of rl) {
      const [allIns, allOuts] = line.split(" | ");
      const ins = allIns.split(" ").map(segs => segs.split(""));
      const outs = allOuts.split(" ");

      let digits = [];

      digits[1] = ins.filter(d => d.length === 2)[0];
      digits[7] = ins.filter(d => d.length === 3)[0];
      digits[4] = ins.filter(d => d.length === 4)[0];
      digits[8] = ins.filter(d => d.length === 7)[0];

      for (let digit of ins.filter(d => d.length === 6)) {
        if (_.difference(digit, _.union(digits[7], digits[4])).length === 1) {
          digits[9] = digit;
        } else if (_.difference(digit, digits[7]).length === 3) {
          digits[0] = digit;
        } else {
          digits[6] = digit;
        }
      }

      for (let digit of ins.filter(d => d.length === 5)) {
        if (_.difference(digit, digits[1]).length === 3) {
          digits[3] = digit;
        } else if (_.difference(digit, digits[4]).length === 3) {
          digits[2] = digit;
        } else {
          digits[5] = digit;
        }
      }

      const normalisedDigits = digits.map(digit => digit.sort().join(""));
      const normalisedOuts = outs.map(digit => digit.split("").sort().join(""));

      const number = normalisedOuts.reduce((cur, val, pos) => {
        digit = normalisedDigits.indexOf(val);
        return cur + (digit * Math.pow(10, outs.length - pos - 1));
      }, 0);

      console.log(number);
      sum += number;
    }

    console.log(sum);
  }
  
  processLineByLine();