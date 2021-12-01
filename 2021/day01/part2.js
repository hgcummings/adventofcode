const fs = require('fs');
const readline = require('readline');

async function processLineByLine() {
    const fileStream = fs.createReadStream('input.txt');
  
    const rl = readline.createInterface({
      input: fileStream,
      crlfDelay: Infinity
    });
  
    let increases = 0;
    let window = [];
    for await (const line of rl) {
        const next = parseInt(line, 10);
        window.push(next);
        if (window.length === 5) {
          window.shift();
        }
        if (window.length === 4) {
          if (window[3] > window[0]) {
            increases += 1;
          }
        }
    }

    console.log(increases);
  }
  
  processLineByLine();